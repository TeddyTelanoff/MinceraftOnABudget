using UnityEngine;

public class Player : Entity
{
	public static Player ClientPlayer { get; private set; }

	public int renderDistance;
	public float turnSpeed;
	public float reach;

	public bool Grounded
	{
		get
		{
			bool tl = Physics.Raycast(transform.position - ccollider.bounds.extents / 2, -Vector3.up, ccollider.bounds.extents.y + 0.01f, LayerMask.NameToLayer("Ground"));
			bool br = Physics.Raycast(transform.position + ccollider.bounds.extents / 2, -Vector3.up, ccollider.bounds.extents.y + 0.01f, LayerMask.NameToLayer("Ground"));
			bool tr = Physics.Raycast(transform.position - new Vector3((ccollider.bounds.extents / 2).x, 0), -Vector3.up, ccollider.bounds.extents.y + 0.01f, LayerMask.NameToLayer("Ground"));
			bool bl = Physics.Raycast(transform.position + new Vector3(0, (ccollider.bounds.extents / 2).y), -Vector3.up, ccollider.bounds.extents.y + 0.01f, LayerMask.NameToLayer("Ground"));
			bool middle = Physics.Raycast(transform.position, -Vector3.up, ccollider.bounds.extents.y + 0.01f, LayerMask.NameToLayer("Ground"));


			return tl || br || tr || bl || middle;
		}
	}

	public Vector3Int WorldPosition { get => new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z)); }
	public (Vector2Int ChunkPosition, Vector3Int PositionInChunk) ChunkPosition { get => World.CurrentWorld.WorldPositionToChunkPosition(WorldPosition); }

	public bool LookingAtBlock { get; private set; }
	public Vector3Int SelectedBlockWorldSpace { get; private set; }
	public (Chunk Chunk, Vector3Int PositionInChunk) SelectedBlockChunkSpace { get; private set; }

	private Collider ccollider { get; set; }
	private new Camera camera { get; set; }
	private Vector2 rotation;

	protected override void Awake()
	{
		base.Awake();
		ccollider = GetComponent<CapsuleCollider>();
		camera = GetComponentInChildren<Camera>();
		ClientPlayer = this;
	}

	protected override void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.A))
			rb.AddForce(-transform.right * speed);
		if (Input.GetKey(KeyCode.D))
			rb.AddForce(transform.right * speed);
		if (Input.GetKey(KeyCode.W))
			rb.AddForce(transform.forward * speed);
		if (Input.GetKey(KeyCode.S))
			rb.AddForce(-transform.forward * speed);
		if (Input.GetKey(KeyCode.Space) && Grounded)
			rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

		RaycastHit hit;
		LookingAtBlock = Physics.Raycast(camera.ViewportPointToRay(Vector3.one / 2), out hit, reach);
		if (LookingAtBlock)
		{
			Vector3 point = hit.point + Vector3.one / 2 - hit.normal / 4;
			SelectedBlockWorldSpace = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), Mathf.FloorToInt(point.z));
			SelectedBlockChunkSpace = (hit.transform.GetComponent<Chunk>(), new Vector3Int(Mathf.Abs(SelectedBlockWorldSpace.x % Chunk.Size.x), Mathf.Abs(SelectedBlockWorldSpace.y % Chunk.Size.y), Mathf.Abs((SelectedBlockWorldSpace.z + 1) % Chunk.Size.z)));

			if (Input.GetMouseButtonDown(0) && SelectedBlockChunkSpace.Chunk)
			{
				SelectedBlockChunkSpace.Chunk.blocks[SelectedBlockChunkSpace.PositionInChunk.x, SelectedBlockChunkSpace.PositionInChunk.y, SelectedBlockChunkSpace.PositionInChunk.z].Type = Block.VOID;
				SelectedBlockChunkSpace.Chunk.GenerateMesh();
			}

			if (Input.GetMouseButtonDown(1) && SelectedBlockChunkSpace.Chunk)
			{
				Vector3 npoint = point + hit.normal / 2;
				Vector3Int placeBlock = new Vector3Int(Mathf.FloorToInt(npoint.x), Mathf.FloorToInt(npoint.y), Mathf.FloorToInt(npoint.z));
				Vector3Int placeBlockInChunk = new Vector3Int(Mathf.Abs(placeBlock.x % Chunk.Size.x), Mathf.Abs(placeBlock.y % Chunk.Size.y), Mathf.Abs((placeBlock.z + 1) % Chunk.Size.z));
				SelectedBlockChunkSpace.Chunk.blocks[placeBlockInChunk.x, placeBlockInChunk.y, placeBlockInChunk.z].Type = Block.DIRT_BLOCK;
				SelectedBlockChunkSpace.Chunk.GenerateMesh();
			}
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{
			rotation.x += Input.GetAxis("Mouse X") * turnSpeed;
			rotation.y += Input.GetAxis("Mouse Y") * turnSpeed;
			rotation = new Vector2(rotation.x % 360, Mathf.Clamp(rotation.y, -90, 90));
			transform.localRotation = Quaternion.Euler(new Vector3 { y = rotation.x });
			camera.transform.localRotation = Quaternion.Euler(new Vector3 { x = -rotation.y });
		}

		if (transform.position.y < -32)
			transform.position = new Vector3(8, 32, 8);

		base.FixedUpdate();
	}

	private void OnDrawGizmos()
	{
		if (LookingAtBlock)
			Gizmos.DrawWireCube(SelectedBlockWorldSpace, Vector3.one);
	}
}
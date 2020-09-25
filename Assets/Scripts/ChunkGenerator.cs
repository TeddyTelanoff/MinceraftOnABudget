using UnityEngine;

[RequireComponent(typeof(World))]
public class ChunkGenerator : MonoBehaviour
{
	public World world { get; private set; }

	private void Awake()
	{
		world = GetComponent<World>();
	}

    private void FixedUpdate()
    {
        for (int x = -Player.ClientPlayer.renderDistance; x < Player.ClientPlayer.renderDistance; x++)
			for (int y = -Player.ClientPlayer.renderDistance; y < Player.ClientPlayer.renderDistance; y++)
            {
				GenerateChunk(new Vector2Int(x, y) + world.WorldPositionToChunkPosition(new Vector3Int(Mathf.FloorToInt(Player.ClientPlayer.transform.position.x),
					Mathf.FloorToInt(Player.ClientPlayer.transform.position.y), Mathf.FloorToInt(Player.ClientPlayer.transform.position.z))).chunkPosition);
            }
		foreach (Vector2Int chunkPosition in world.Chunks.Keys)
		{
			if (((chunkPosition.x >= Player.ClientPlayer.ChunkPosition.chunkPosition.x - Player.ClientPlayer.renderDistance) &&
				(chunkPosition.x <= Player.ClientPlayer.ChunkPosition.chunkPosition.x + Player.ClientPlayer.renderDistance)) &&
				((chunkPosition.y >= Player.ClientPlayer.ChunkPosition.chunkPosition.y - Player.ClientPlayer.renderDistance) &&
				(chunkPosition.y <= Player.ClientPlayer.ChunkPosition.chunkPosition.y + Player.ClientPlayer.renderDistance)))
				RenderChunk(chunkPosition);
			else
				DeRenderChunk(chunkPosition);
        }
	}

    public void GenerateChunk(Vector2Int position)
	{
		if (world.Chunks.ContainsKey(position))
			return;

		GameObject chunkObject = new GameObject();
		chunkObject.name = $"Chunk {position}";
		chunkObject.layer = LayerMask.NameToLayer("Block");
		chunkObject.transform.position = new Vector3Int(position.x, 0, position.y) * Chunk.Size;

		chunkObject.AddComponent<Chunk>().position = position;
		chunkObject.GetComponent<MeshRenderer>().sharedMaterial = world.blockMaterial;
		chunkObject.GetComponent<MeshCollider>().sharedMaterial = world.blockphysicsMaterial;

		for (int x = 0; x < world.ChunkSize.x; x++)
			for (int z = 0; z < world.ChunkSize.z; z++)
			{
				Vector3Int worldPosition = world.ChunkPositionToWorldPosition(position, new Vector3Int(x, 0, z));

				int y = Mathf.FloorToInt(Mathf.Clamp(Mathf.PerlinNoise(world.noiseSeed + worldPosition.x * world.noiseStep, world.noiseSeed + worldPosition.z * world.noiseStep) * world.ChunkSize.y, 0, world.ChunkSize.y));
				chunkObject.GetComponent<Chunk>().blocks[x, y, z].Type = Block.GRASS_BLOCK;
			}
		world.Chunks.Add(position, chunkObject.GetComponent<Chunk>());
	}

	

	public void GenerateChunkMesh(Vector2Int position)
    {
		world.Chunks[position].GenerateMesh();
	}
	
	public void RenderChunk(Vector2Int position)
    {
		if (world.Chunks[position].hasGenerated)
			world.Chunks[position].ShowMesh();
		else
			world.Chunks[position].GenerateMesh();
	}

	public void DeRenderChunk(Vector2Int position)
	{
		world.Chunks[position].DestroyMesh();
	}
}
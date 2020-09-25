using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
	public static Vector3Int Size { get => World.CurrentWorld.ChunkSize; }

	public Vector2Int position { get; set; }
	public Block[,,] blocks { get; private set; }
	public Mesh mesh { get; private set; }
	public List<Vector3> vertices { get; private set; }
	public List<int> triangles { get; private set; }
	public List<Vector2> uv { get; private set; }

	private void Awake()
	{
		blocks = new Block[Size.x, Size.y, Size.z];
		mesh = new Mesh();
		vertices = new List<Vector3>();
		triangles = new List<int>();
		uv = new List<Vector2>();
	}

	public void GenerateMesh()
	{
		for (int x = 0; x < Size.x; x++)
			for (int y = 0; y < Size.x; y++)
				for (int z = 0; z < Size.x; z++)
					if (blocks[x, y, z].type != Block.VOID)
					{
						GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
						cube.transform.position = World.CurrentWorld.ChunkPositionToWorldPosition(position, new Vector3Int(x, y, z));
					}

		mesh.Clear();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = uv.ToArray();

		GetComponent<MeshFilter>().sharedMesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}
}
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
						Vector3Int position = new Vector3Int(x, y, z);

						// Front
						//if (IsVoid(position + Vector3.forward))
							GenerateFace(position, Vector3.up, Vector3.right, false);
						// Back
						//if (IsVoid(position - Vector3.forward))
							GenerateFace(position - Vector3.forward, Vector3.up, Vector3.right, true);
						// Left
						//if (IsVoid(position - Vector3.right))
							GenerateFace(position + Vector3.right, Vector3.up,- Vector3.forward, false);
						// Right
						//if (IsVoid(position + Vector3.right))
							GenerateFace(position, Vector3.up, -Vector3.forward, true);
						// Top
						//if (IsVoid(position + Vector3.up))
							GenerateFace(position + Vector3.up, -Vector3.forward, Vector3.right, false);
						// Bottom
						//if (IsVoid(position - Vector3.up))
							GenerateFace(position, -Vector3.forward, Vector3.right, true);
					}

		mesh.Clear();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = uv.ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.RecalculateTangents();

		GetComponent<MeshFilter>().sharedMesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	public bool IsVoid(Vector3 position)
	{
		if (!((position.x >= 0 && position.x < Size.x) && (position.y >= 0 && position.y < Size.y) && (position.z >= 0 && position.z < Size.z)))
			return true;
		return blocks[(int)position.x, (int)position.y, (int)position.z].type == Block.VOID;
	}

	public void GenerateFace(Vector3 position, Vector3 up, Vector3 right, bool reversed)
	{
		int vertexStart = vertices.Count;

		Vector3 corner = position - Vector3.one / 2;
		vertices.Add(corner);
		vertices.Add(corner + up);
		vertices.Add(corner + up + right);
		vertices.Add(corner + right);

		if (reversed)
		{
			triangles.Add(vertexStart + 0);
			triangles.Add(vertexStart + 1);
			triangles.Add(vertexStart + 2);

			triangles.Add(vertexStart + 0);
			triangles.Add(vertexStart + 2);
			triangles.Add(vertexStart + 3);
		}
		else
		{
			triangles.Add(vertexStart + 0);
			triangles.Add(vertexStart + 2);
			triangles.Add(vertexStart + 1);

			triangles.Add(vertexStart + 0);
			triangles.Add(vertexStart + 3);
			triangles.Add(vertexStart + 2);
		}
	}
}
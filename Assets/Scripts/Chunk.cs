﻿using System.Collections.Generic;
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
	public bool hasGenerated { get; private set; }
	public bool visible { get => GetComponent<MeshFilter>().sharedMesh != null; }

	private void Awake()
	{
		blocks = new Block[Size.x, Size.y, Size.z];
		for (int x = 0; x < Size.x; x++)
			for (int y = 0; y < Size.x; y++)
				for (int z = 0; z < Size.x; z++)
				{
					blocks[x, y, z].Chunk = this;
					blocks[x, y, z].PositionInChunk = new Vector3Int(x, y, z);
				}
		mesh = new Mesh();
		vertices = new List<Vector3>();
		triangles = new List<int>();
		uv = new List<Vector2>();
	}

	public void ShowMesh()
	{
		mesh.Clear();
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = uv.ToArray();
		mesh.RecalculateNormals();

		GetComponent<MeshFilter>().sharedMesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	public void DestroyMesh()
	{
		GetComponent<MeshFilter>().sharedMesh = null;
		GetComponent<MeshCollider>().sharedMesh = null;
	}

	public void GenerateMesh()
	{
		hasGenerated = true;

		vertices = new List<Vector3>();
		triangles = new List<int>();
		uv = new List<Vector2>();

		for (int x = 0; x < Size.x; x++)
			for (int y = 0; y < Size.x; y++)
				for (int z = 0; z < Size.x; z++)
					if (blocks[x, y, z].Type != Block.VOID)
					{
						Vector3Int blockPosition = new Vector3Int(x, y, z);
						int blockType = blocks[x, y, z].Type;

						// Front
						if (blockPosition.z < Size.z - 1 && blocks[blockPosition.x, blockPosition.y, blockPosition.z + 1].Type == Block.VOID || blockPosition.z == Size.z - 1)
							GenerateFace(blockType, blockPosition, Vector3.up, Vector3.right, false);
						// Back
						if (blockPosition.z > 0 && blocks[blockPosition.x, blockPosition.y, blockPosition.z - 1].Type == Block.VOID || blockPosition.z == 0)
							GenerateFace(blockType, blockPosition - Vector3.forward, Vector3.up, Vector3.right, true);
						// Left
						if (blockPosition.x < Size.x - 1 && blocks[blockPosition.x + 1, blockPosition.y, blockPosition.z].Type == Block.VOID || blockPosition.x == Size.x - 1)
							GenerateFace(blockType, blockPosition + Vector3.right, Vector3.up, -Vector3.forward, false);
						// Right
						if (blockPosition.x > 0 && blocks[blockPosition.x - 1, blockPosition.y, blockPosition.z].Type == Block.VOID || blockPosition.x == 0)
							GenerateFace(blockType, blockPosition, Vector3.up, -Vector3.forward, true);
						// Top
						if (blockPosition.y < Size.y - 1 && blocks[blockPosition.x, blockPosition.y + 1, blockPosition.z].Type == Block.VOID || blockPosition.y == Size.y - 1)
							GenerateFace(blockType, blockPosition + Vector3.up, -Vector3.forward, Vector3.right, false);
						// Bottom
						if (blockPosition.y > 0 && blocks[blockPosition.x, blockPosition.y - 1, blockPosition.z].Type == Block.VOID || blockPosition.y == 0)
							GenerateFace(blockType, blockPosition, -Vector3.forward, Vector3.right, true);
					}

		DestroyMesh();
		ShowMesh();
	}

	public void GenerateFace(int texture, Vector3 position, Vector3 up, Vector3 right, bool reversed)
	{
		int vertexStart = vertices.Count;

		Vector3 corner = position - Vector3.one / 2;
		vertices.Add(corner);
		vertices.Add(corner + up);
		vertices.Add(corner + up + right);
		vertices.Add(corner + right);

		Vector4 uvs = Block.TextureCoordinents[texture];
		uv.Add(new Vector2(uvs.x, uvs.w));
		uv.Add(new Vector2(uvs.x, uvs.y));
		uv.Add(new Vector2(uvs.z, uvs.y));
		uv.Add(new Vector2(uvs.z, uvs.w));

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
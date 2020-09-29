using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(World))]
public class ChunkGenerator : MonoBehaviour
{
	public World World { get; private set; }

	private List<Vector2Int> deRender { get; set; } = new List<Vector2Int>();

	private void Awake()
	{
		World = GetComponent<World>();
	}

	private void FixedUpdate()
	{
		for (int x = -Player.ClientPlayer.renderDistance - 1; x <= Player.ClientPlayer.renderDistance; x++)
			for (int y = -Player.ClientPlayer.renderDistance - 1; y <= Player.ClientPlayer.renderDistance; y++)
			{
				if (!World.ActiveChunks.ContainsKey(new Vector2Int(x, y) + Player.ClientPlayer.ChunkSpace.ChunkPosition))
				{
					StartCoroutine(GenerateChunk(new Vector2Int(x, y) + Player.ClientPlayer.ChunkSpace.ChunkPosition));

					//if (World.Chunks.ContainsKey(new Vector2Int(x, y) + Player.ClientPlayer.ChunkSpace.ChunkPosition))
						//StartCoroutine(RenderChunk(new Vector2Int(x, y) + Player.ClientPlayer.ChunkSpace.ChunkPosition));
				}
			}

		foreach (Vector2Int chunkPos in World.ActiveChunks.Keys)
			if ((chunkPos.x < -Player.ClientPlayer.renderDistance - 1 || chunkPos.x > Player.ClientPlayer.renderDistance + 1) ||
				(chunkPos.y < -Player.ClientPlayer.renderDistance - 1 || chunkPos.y > Player.ClientPlayer.renderDistance + 1))
			{
				deRender.Add(chunkPos);
			}
		for (int i = deRender.Count - 1; i >= 0; i--)
        {
			Vector2Int chunkPos = deRender[i];

			World.ActiveChunks.Remove(chunkPos);
			StartCoroutine(DeRenderChunk(chunkPos));

			deRender.RemoveAt(i);
		}
	}

	public IEnumerator GenerateChunk(Vector2Int position)
	{
		if (!World.Chunks.ContainsKey(position))
		{
			GameObject chunkObject = new GameObject();
			chunkObject.name = $"Chunk {position}";
			chunkObject.layer = LayerMask.NameToLayer("Block");
			chunkObject.transform.position = new Vector3Int(position.x, 0, position.y) * Chunk.Size;

			chunkObject.AddComponent<Chunk>().position = position;
			chunkObject.GetComponent<MeshRenderer>().sharedMaterial = World.blockMaterial;
			chunkObject.GetComponent<MeshCollider>().sharedMaterial = World.blockphysicsMaterial;

			for (int x = 0; x < World.ChunkSize.x; x++)
				for (int z = 0; z < World.ChunkSize.z; z++)
				{
					Vector3Int worldPosition = World.ChunkSpaceToWorldSpace(position, new Vector3Int(x, 0, z));

					int y = Mathf.FloorToInt(Mathf.Clamp(Mathf.PerlinNoise(World.noiseSeed + worldPosition.x * World.noiseStep, World.noiseSeed + worldPosition.z * World.noiseStep) * World.ChunkGenerationHeight, 0, World.ChunkGenerationHeight));
					chunkObject.GetComponent<Chunk>().blocks[x, y, z].Type = Block.GRASS_BLOCK;
					for (int by = 0; by < y; by++)
						chunkObject.GetComponent<Chunk>().blocks[x, by, z].Type = Block.DIRT_BLOCK;
				}
			if (!World.Chunks.ContainsKey(position))
				World.Chunks.Add(position, chunkObject.GetComponent<Chunk>());
			if (!World.ActiveChunks.ContainsKey(position))
				World.ActiveChunks.Add(position, chunkObject.GetComponent<Chunk>());

			StartCoroutine(RenderChunk(position));
		}

		yield return null;
	}

	

	public IEnumerator GenerateChunkMesh(Vector2Int position)
	{
		World.Chunks[position].GenerateMesh();

		yield return null;
	}
	
	public IEnumerator RenderChunk(Vector2Int position)
	{
		if (World.Chunks[position].hasGenerated)
			World.Chunks[position].ShowMesh();
		else
			World.Chunks[position].GenerateMesh();

		yield return null;
	}

	public IEnumerator DeRenderChunk(Vector2Int position)
	{
		World.Chunks[position].DestroyMesh();
		Debug.Log($"DeRendering {position}");

		yield return null;
	}
}
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChunkGenerator))]
public class World : MonoBehaviour
{
	public static World CurrentWorld;

	public Material blockMaterial;
	public PhysicMaterial blockphysicsMaterial;
	public Vector3Int ChunkSize;
	public int ChunkGenerationHeight;
	public float noiseStep;
	public float noiseSeed;

	public Dictionary<Vector2Int, Chunk> Chunks { get; private set; }
	public Dictionary<Vector2Int, Chunk> ActiveChunks { get; set; }
	public ChunkGenerator chunkGenerator { get; private set; }
	

	private void Awake()
	{
		Chunks = new Dictionary<Vector2Int, Chunk>();
		ActiveChunks = new Dictionary<Vector2Int, Chunk>();
		chunkGenerator = GetComponent<ChunkGenerator>();
	}

	public (Vector2Int chunkPosition, Vector3Int positionInChunk) WorldSpaceToChunkSpace(Vector3Int worldPos)
	{
		Vector2Int chunkPosition = new Vector2Int(Mathf.FloorToInt((worldPos.x - (worldPos.x % ChunkSize.x)) / (float)ChunkSize.x), Mathf.FloorToInt((worldPos.z - (worldPos.z % ChunkSize.z)) / (float)ChunkSize.z));
		Vector3Int positionInChunk = new Vector3Int(worldPos.x - (ChunkSize.x * chunkPosition.x + 1), worldPos.y, worldPos.z - (ChunkSize.z * chunkPosition.x + 1));

		return (chunkPosition, positionInChunk);
	}

	public Vector3Int ChunkSpaceToWorldSpace(Vector2Int chunkPosition, Vector3Int positionInChunk)
	{
		return new Vector3Int(chunkPosition.x * ChunkSize.x + positionInChunk.x, positionInChunk.y, chunkPosition.y * ChunkSize.z + positionInChunk.z);
	}
}
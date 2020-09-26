using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChunkGenerator))]
public class World : MonoBehaviour
{
	public static World CurrentWorld;

	public Material blockMaterial;
	public PhysicMaterial blockphysicsMaterial;
	public Vector3Int ChunkSize;
	public float noiseStep;
	public float noiseSeed;

	public Dictionary<Vector2Int, Chunk> Chunks { get; private set; }
	public ChunkGenerator chunkGenerator { get; private set; }
	

	private void Awake()
	{
		Chunks = new Dictionary<Vector2Int, Chunk>();
		chunkGenerator = GetComponent<ChunkGenerator>();
	}

	public (Vector2Int chunkPosition, Vector3Int positionInChunk) WorldPositionToChunkPosition(Vector3Int worldPos)
	{
		Vector2Int chunkPosition = new Vector2Int((worldPos.x - (worldPos.x % ChunkSize.x)) / ChunkSize.x, (worldPos.z - (worldPos.z % ChunkSize.z)) / ChunkSize.z);
		Vector3Int positionInChunk = new Vector3Int(worldPos.x % ChunkSize.x, worldPos.y, worldPos.z % ChunkSize.z);

		if (positionInChunk.x < 0)
			positionInChunk.x = -positionInChunk.x;
		if (positionInChunk.y < 0)
			positionInChunk.y = -positionInChunk.y;
		if (positionInChunk.z < 0)
			positionInChunk.z = -positionInChunk.z;

		return (chunkPosition, positionInChunk);
	}

	public Vector3Int ChunkPositionToWorldPosition(Vector2Int chunkPosition, Vector3Int positionInChunk)
	{
		return new Vector3Int(chunkPosition.x * ChunkSize.x + positionInChunk.x, positionInChunk.y, chunkPosition.y * ChunkSize.z + positionInChunk.z);
	}
}
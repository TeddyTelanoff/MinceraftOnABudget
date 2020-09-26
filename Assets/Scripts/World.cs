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
		Vector2Int chunkPosition = new Vector2Int(Mathf.FloorToInt((float)worldPos.x / ChunkSize.x - (float)(worldPos.x % ChunkSize.x) / ChunkSize.x),
			Mathf.FloorToInt((float)worldPos.z / ChunkSize.z - (float)(worldPos.z % ChunkSize.z) / ChunkSize.z));
		Vector3Int positionInChunk = new Vector3Int(Mathf.Abs(worldPos.x % ChunkSize.x), Mathf.Abs(worldPos.y), Mathf.Abs(worldPos.z % ChunkSize.z));

		/*if (positionInChunk.x < 0)
			positionInChunk.x = ChunkSize.x - Mathf.Abs(positionInChunk.x);
		if (positionInChunk.y < 0)
			positionInChunk.y = ChunkSize.y - Mathf.Abs(positionInChunk.y);
		if (positionInChunk.z < 0)
			positionInChunk.z = ChunkSize.z - Mathf.Abs(positionInChunk.z);*/

		return (chunkPosition, positionInChunk);
	}

	public Vector3Int ChunkPositionToWorldPosition(Vector2Int chunkPosition, Vector3Int positionInChunk)
	{
		return new Vector3Int(chunkPosition.x * ChunkSize.x + positionInChunk.x, positionInChunk.y, chunkPosition.y * ChunkSize.z + positionInChunk.z);
	}
}
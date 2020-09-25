using UnityEngine;

public struct Block
{
	public const int VOID = 0, GRASS_BLOCK = 1, DIRT_BLACK = 2;

	public Chunk chunk { get; set; }
	public Vector3Int positionInChunk { get; set; }
	public Vector3Int position { get; set; }
	public int type { get; set; }
}
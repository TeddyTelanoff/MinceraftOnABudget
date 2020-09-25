using UnityEngine;

public struct Block
{
	public const int VOID = 0, GRASS_BLOCK = 1, DIRT_BLACK = 2;

	public Chunk Chunk { get; set; }
	public Vector3Int PositionInChunk { get; set; }
	public int Type { get; set; }
}
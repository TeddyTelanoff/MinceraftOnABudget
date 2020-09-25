using System.Collections.Generic;
using UnityEngine;

public struct Block
{
	public const int VOID = 0, GRASS_BLOCK = 1, DIRT_BLOCK = 2;
	public static readonly Dictionary<int, Vector4> TextureCoordinents = new Dictionary<int, Vector4>()
	{
		[VOID] = Vector4.zero,
		[GRASS_BLOCK] = new Vector4(0.25f, 1f, 0.5f, 0.75f),
		[DIRT_BLOCK] = new Vector4(0.5f, 1f, 0.75f, 0.75f)
	};

	public Chunk Chunk { get; set; }
	public Vector3Int PositionInChunk { get; set; }
	public int Type { get; set; }
}
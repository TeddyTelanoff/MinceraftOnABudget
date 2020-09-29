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
	public static Block BlockAt(Vector3Int worldSpace)
	{
		var chunkSpace = World.CurrentWorld.WorldSpaceToChunkSpace(worldSpace);
		if (!World.CurrentWorld.Chunks.ContainsKey(chunkSpace.chunkPosition))
			return new Block();
		return World.CurrentWorld.Chunks[chunkSpace.chunkPosition].blocks[chunkSpace.positionInChunk.x, chunkSpace.positionInChunk.y, chunkSpace.positionInChunk.z];
	}

	public static Dictionary<Vector3Int, Block> blocks = new Dictionary<Vector3Int, Block>();

	public Chunk Chunk { get; set; }
	public Vector3Int Position { get; set; }
	public Vector3Int PositionInChunk { get; set; }
	public int Type { get; set; }
}
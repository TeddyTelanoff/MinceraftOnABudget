using UnityEngine;

[RequireComponent(typeof(World))]
public class ChunkGenerator : MonoBehaviour
{
	public World world { get; private set; }

	private void Awake()
	{
		world = GetComponent<World>();

		GenerateChunk(Vector2Int.zero);
		RenderChunk(Vector2Int.zero);
	}

	public void GenerateChunk(Vector2Int position)
	{
		GameObject chunkObject = new GameObject();
		chunkObject.name = $"Chunk {position}";
		chunkObject.layer = LayerMask.NameToLayer("Block");
		chunkObject.transform.position = new Vector3Int(position.x, 0, position.y) * Chunk.Size;

		chunkObject.AddComponent<Chunk>().position = position;
		chunkObject.GetComponent<MeshRenderer>().sharedMaterial = world.blockMaterial;

		for (int x = 0; x < world.ChunkSize.x; x++)
			for (int z = 0; z < world.ChunkSize.z; z++)
			{
				int y = Mathf.FloorToInt(Mathf.Clamp(Mathf.PerlinNoise(world.noiseSeed + x * world.noiseStep, world.noiseSeed + z * world.noiseStep) * world.ChunkSize.y, 0, world.ChunkSize.y));
				chunkObject.GetComponent<Chunk>().blocks[x, y, z].Type = Block.GRASS_BLOCK;
			}
		world.Chunks.Add(position, chunkObject.GetComponent<Chunk>());
	}

	public void RenderChunk(Vector2Int position)
    {
		world.Chunks[position].GenerateMesh();
	}
}
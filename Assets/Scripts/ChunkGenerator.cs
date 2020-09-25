using UnityEngine;

[RequireComponent(typeof(World))]
public class ChunkGenerator : MonoBehaviour
{
	public World world { get; private set; }

	private void Awake()
	{
		world = GetComponent<World>();

		GenerateChunk(new Vector2Int(0, 0));
	}

	public void GenerateChunk(Vector2Int position)
	{
		GameObject chunkObject = new GameObject();
		chunkObject.name = $"Chunk {position}";
		chunkObject.layer = LayerMask.NameToLayer("Block");
		chunkObject.transform.position = new Vector3Int(position.x, 0, position.y) * Chunk.Size;

		chunkObject.AddComponent<Chunk>().position = position;

		for (int x = 0; x < world.ChunkSize.x; x++)
			for (int z = 0; z < world.ChunkSize.z; z++)
			{
				int y = Mathf.FloorToInt(Mathf.Clamp(Mathf.PerlinNoise(world.noiseSeed + x * world.noiseStep, world.noiseSeed + z * world.noiseStep) * world.ChunkSize.y, 0, world.ChunkSize.y));
				chunkObject.GetComponent<Chunk>().blocks[x, y, z].type = Block.GRASS_BLOCK;
			}
		chunkObject.GetComponent<Chunk>().GenerateMesh();
	}
}
using UnityEngine;

[RequireComponent(typeof(World))]
public class ChunkGenerator : MonoBehaviour
{
    public World world { get; private set; }

    private void Awake()
    {
        world = GetComponent<World>();
    }

    public void GenerateChunk(Vector2Int position)
    {
        GameObject chunkObject = new GameObject();
        chunkObject.name = $"Chunk {position}";
        chunkObject.layer = LayerMask.NameToLayer("Block");
        chunkObject.transform.position = new Vector3Int(position.x, 0, position.y) * Chunk.Size;

        chunkObject.AddComponent<Chunk>();

    }
}
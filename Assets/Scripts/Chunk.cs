using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Chunk : MonoBehaviour
{
	public static Vector3Int Size { get => World.CurrentWorld.ChunkSize; }

	public Vector2Int position { get; set; }
	public Block[,,] blocks { get; private set; }
	public Mesh mesh { get; } = new Mesh();

	public void GenerateMesh()
    {
		GetComponent<MeshFilter>().sharedMesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
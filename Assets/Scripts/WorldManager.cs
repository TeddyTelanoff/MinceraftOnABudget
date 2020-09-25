using UnityEngine;

public class WorldManager : MonoBehaviour
{
	public World world;

	private void Awake()
	{
		GameManager.WorldManager = this;
		OpenWorld(world);
	}

	public void OpenWorld(World world)
	{
		World.CurrentWorld = world;
	}
}
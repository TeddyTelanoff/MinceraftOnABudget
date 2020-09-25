using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public static WorldManager WorldManager;

	private void Awake()
	{
		Instance = this;
	}
}
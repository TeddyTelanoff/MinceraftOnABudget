using UnityEngine;

public class Player : Entity
{
	public static Player ClientPlayer;

    protected override void Awake()
    {
        base.Awake();

        ClientPlayer = this;
    }


}
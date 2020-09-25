using UnityEngine;

public class Player : Entity
{
	public static Player ClientPlayer { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        ClientPlayer = this;
    }

    protected override void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(-Vector3.right * speed);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(Vector3.right * speed);
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(Vector3.forward * speed);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(-Vector3.forward * speed);
        if (Input.GetKeyDown(KeyCode.Space))
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

        base.FixedUpdate();
    }
}
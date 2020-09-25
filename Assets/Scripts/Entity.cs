using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour
{
	public float speed;
	public float maxSpeed;
	public float jumpForce;

	protected Rigidbody rb { get; private set; }

	protected virtual void Awake()
	{
        rb = GetComponent<Rigidbody>();
		rb.drag = 5;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
	}

	protected virtual void FixedUpdate()
    {
		if (rb.velocity.magnitude > maxSpeed)
			rb.velocity = rb.velocity / rb.velocity.magnitude * maxSpeed;
    }
}

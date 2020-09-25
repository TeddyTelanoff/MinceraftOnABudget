using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour
{
	public float speed;
	public float maxSpeed;

	protected Rigidbody rb { get; private set; }

	protected virtual void Awake()
	{
        rb = GetComponent<Rigidbody>();
		rb.drag = 5;
	}
}

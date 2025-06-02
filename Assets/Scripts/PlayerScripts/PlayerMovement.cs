using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	// it is Physic
public class PlayerMovement : MonoBehaviour
{
	[Header("달리기 계수")]
	[Range(0, 3)]
	public float runSpeed;
	public float acceleration;
	int groundLayerMask;
	[SerializeField]
	private float friction;
	private Vector3 velocity;
	Rigidbody myRigidbody;
	CapsuleCollider Pcollider;

	private void Start()
    {
		groundLayerMask |= (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Obstacles"));
		myRigidbody = GetComponent<Rigidbody>();
		Pcollider = GetComponent<CapsuleCollider>();
	}

    private void FixedUpdate()
    {
        
    }


	public void Move(Vector3 direction, float checkRun)
	{
		Vector3 dir = Vector3.ProjectOnPlane(direction, RayToGround(groundLayerMask).normal).normalized;

		Vector3 acc = dir * acceleration * (1 + runSpeed * checkRun) * Time.fixedDeltaTime;
		velocity += acc;
		velocity = Vector3.ClampMagnitude(velocity, acceleration);
		velocity *= friction;
		if (dir == Vector3.zero && velocity.magnitude < 0.01f)
		{
			velocity = Vector3.zero;
		}
		myRigidbody.AddForce(transform.position + velocity * Time.fixedDeltaTime);

	}

	public RaycastHit RayToGround(int layerMask)
	{
		Ray ray = new Ray(transform.position, Vector3.down);
		Physics.SphereCast(ray, Pcollider.radius, out RaycastHit groundHitInfo, 100f, layerMask);
		return groundHitInfo;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	// it is Physic
public class PlayerMovement : MonoBehaviour
{
	[Header("달리기 계수")]
	[Range(0, 3)]
	public float runSpeed;
	public float walkspeed;
	private float acceleration;
	int groundLayerMask;
	[SerializeField]
	private float friction;
	private Vector3 velocity;
	Rigidbody myRigidbody;
	CapsuleCollider Pcollider;
	PlayerInput playerInput;
	public Vector3 inputDir = Vector3.zero;
	public float run = 0f;


	private void Start()
    {
		groundLayerMask |= (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Obstacles"));
		myRigidbody = GetComponent<Rigidbody>();
		Pcollider = GetComponent<CapsuleCollider>();
		playerInput = GetComponent<PlayerInput>();
	}

    private void FixedUpdate()
    {
		Move();
		if(inputDir != Vector3.zero)
        {
			Quaternion angle = Quaternion.LookRotation(new Vector3(inputDir.x, 0, inputDir.z));
			SetRotate(angle);
        }
    }
	public void SetRotate(Quaternion targetAngle)
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, 2.5f);
	}

	void Move()
	{
		Vector3 dir = Vector3.ProjectOnPlane(inputDir, RayToGround(groundLayerMask).normal).normalized;
		acceleration = walkspeed * (1 + run * runSpeed);
		Vector3 acc = dir * acceleration * Time.fixedDeltaTime;
		if (dir == Vector3.zero && myRigidbody.velocity.magnitude < 0.01f)
		{
			myRigidbody.velocity = Vector3.zero;
		}
		myRigidbody.AddForce(transform.position + acc * Time.fixedDeltaTime);
		myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, acceleration);
		myRigidbody.velocity = new Vector3(myRigidbody.velocity.x * friction, myRigidbody.velocity.y, myRigidbody.velocity.z * friction);
		Debug.Log(myRigidbody.velocity.magnitude);
	}

	public RaycastHit RayToGround(int layerMask)
	{
		Ray ray = new Ray(transform.position, Vector3.down);
		Physics.SphereCast(ray, Pcollider.radius, out RaycastHit groundHitInfo, 100f, layerMask);
		return groundHitInfo;
	}
}

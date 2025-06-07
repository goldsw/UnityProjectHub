using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	// it is Physic
public class PlayerMovement : MonoBehaviour
{
	[Header("달리기 계수")]
	[Range(0, 3)]
	public float runSpeed;
	[SerializeField]
	private float friction;
	[HideInInspector]
	public float run = 0f;

	public float walkspeed;
	public Vector3 inputDir = Vector3.zero;
	public float JumpPow;
	private Vector3 velocity;
	private float acceleration;
	Vector3 dir;
	PlayerInput playerInput;
	CapsuleCollider Pcollider;
	Rigidbody myRigidbody;
	int groundLayerMask;

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
		transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, 0.2f);
	}

	void Move()
	{
		dir = Vector3.ProjectOnPlane(inputDir, RayToGround(groundLayerMask).normal).normalized; // 바닥의 노멀 벡터를 찾아 바닥의 기울기에 따라 벡터를 바꿔줌, 경사진 바닥에서 잘 움직임
		acceleration = walkspeed * (1 + run * runSpeed); // 1 + run * runSpeed는 run이 1인 경우 연산됨 run이 0일시 1이 반환됨
		Vector3 acc = acceleration * Time.fixedDeltaTime * dir;
		if (dir == Vector3.zero && myRigidbody.velocity.magnitude < 0.01f)
		{
			myRigidbody.velocity = Vector3.zero;
		}
		myRigidbody.velocity += (acc * Time.fixedDeltaTime);
		myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, acceleration);
		myRigidbody.velocity = new Vector3(myRigidbody.velocity.x * friction, myRigidbody.velocity.y, myRigidbody.velocity.z * friction);

	}

	public void Jumping(Animator anim)
	{
		RaycastHit groundInfo = RayToGround(groundLayerMask);
		if (groundInfo.distance <= Pcollider.height / 2)
		{
			anim.SetTrigger("isJump");
			velocity = Vector3.up * JumpPow;
			myRigidbody.AddForce(Vector3.up * JumpPow, ForceMode.Impulse);

		}

	}

	public RaycastHit RayToGround(int layerMask)
	{
		Ray ray = new Ray(transform.position, Vector3.down);
		Physics.SphereCast(ray, Pcollider.radius, out RaycastHit groundHitInfo, 100f, layerMask);
		return groundHitInfo;
	}

    private void OnDrawGizmos()
    {
		if(myRigidbody != null)
		Gizmos.DrawLine(transform.position, myRigidbody.velocity *100f);
    }
}

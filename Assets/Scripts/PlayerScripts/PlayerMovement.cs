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
	public GameObject camera;
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
			Quaternion angle = Quaternion.LookRotation( new Vector3(inputDir.x, 0, inputDir.z));
			SetRotate(angle);
        }

    }
	public void SetRotate(Quaternion targetAngle)
	{
		Quaternion watchrotation = Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0);
		transform.rotation = watchrotation;
	}

	Vector3 GravityReaction(int layerMask)
    {
		float dot = Vector3.Dot(Vector3.up, RayToGround(groundLayerMask).normal.normalized);
		float sinTheta = Mathf.Sqrt(1.0f - dot * dot);
		Vector3 gravityReaction = CalculateGravityReaction(RayToGround(groundLayerMask).normal.normalized, layerMask) * ((myRigidbody.mass * 9.81f) * sinTheta);

		return gravityReaction;
    }
	public float tmp;
	Vector3 CalculateGravityReaction(Vector3 normal, int layerMask) // 정규화 되어있어야 함
    {
		Vector3 target = Vector3.up;
		Vector3 fixnormal = normal - ReturnMultiplyADot(normal, layerMask);
		Vector3 disVector = (target - fixnormal);
		Vector3 playerDisVec = transform.position + disVector;
		Debug.DrawLine(transform.position, playerDisVec, Color.red);
		
		return disVector;


    }

	Vector3 ReturnMultiplyADot(Vector3 scale, int layerMask)
    {
		scale = scale * (1 - Vector3.Dot(Vector3.up, RayToGround(groundLayerMask).normal.normalized));
		return scale;

	}
	float ReturnMultiplyADot(float scale, int layerMask)
	{
		scale = scale * (1 - Vector3.Dot(Vector3.up, RayToGround(groundLayerMask).normal.normalized));
		return scale;

	}

	void Move()
	{
		Quaternion watchrotation = Quaternion.Euler(0, camera.transform.rotation.eulerAngles.y, 0);
		dir = watchrotation * inputDir; //Vector3.ProjectOnPlane(inputDir, SphereRayToGround(groundLayerMask).normal); // 바닥의 노멀 벡터를 찾아 바닥의 기울기에
																										   // 따라 벡터를 바꿔줌, 경사진 바닥에서 잘 움직임
		acceleration = walkspeed * (1 + run * runSpeed); // 1 + run * runSpeed는 run이 1인 경우 연산됨 run이 0일시 1이 반환됨
		Vector3 acc = acceleration * dir;
		if (dir == Vector3.zero && myRigidbody.velocity.magnitude < 0.01f)
		{
			myRigidbody.velocity = Vector3.zero;
		}
		Vector3 GR = GravityReaction(groundLayerMask);
		//velocity += (acc);
		//velocity = new Vector3(velocity.x * friction, velocity.y, velocity.z * friction);
		//Vector3.ClampMagnitude(velocity, acceleration);
		myRigidbody.AddForce(GR);
		myRigidbody.velocity += (acc * Time.fixedDeltaTime);

		//myRigidbody.velocity = new Vector3(myRigidbody.velocity.x * friction, myRigidbody.velocity.y, myRigidbody.velocity.z * friction);
		myRigidbody.velocity = Vector3.ClampMagnitude(myRigidbody.velocity, acceleration);

		//myRigidbody.velocity = GR * Time.fixedDeltaTime;

	}

	public void Jumping(Animator anim)
	{
		RaycastHit groundInfo = SphereRayToGround(groundLayerMask);
		if (groundInfo.distance <= Pcollider.height / 2)
		{
			anim.SetTrigger("isJump");
			velocity += Vector3.up * JumpPow;
			myRigidbody.AddForce(Vector3.up * JumpPow, ForceMode.Impulse);

		}

	}

	public RaycastHit SphereRayToGround(int layerMask)
	{
		Ray ray = new Ray(transform.position, Vector3.down);
		Physics.SphereCast(ray, Pcollider.radius, out RaycastHit groundHitInfo, 100f, layerMask);
		return groundHitInfo;
	}
	public RaycastHit RayToGround(int layerMask)
	{
		Ray ray = new Ray(transform.position, Vector3.down);
		Physics.Raycast(ray, out RaycastHit groundHitInfo, 100f, layerMask);
		return groundHitInfo;
	}
	private void OnDrawGizmos()
    {

		//if(myRigidbody != null)
		//Gizmos.DrawLine(transform.position, myRigidbody.velocity *100f);
		//Gizmos.color = Color.red;
		Vector3 target = RayToGround(groundLayerMask).point + Vector3.up + ReturnMultiplyADot(Vector3.up, groundLayerMask);
		Gizmos.DrawLine(transform.position + RayToGround(groundLayerMask).normal.normalized, target);
		Gizmos.DrawSphere(RayToGround(groundLayerMask).point + RayToGround(groundLayerMask).normal.normalized, 0.1f);
		Gizmos.DrawSphere(target, 0.1f);
	}
}

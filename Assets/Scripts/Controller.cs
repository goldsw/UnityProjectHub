using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour {

	public float walkSpeed = 6;
	public float sprintSpeed = 12;
	public LayerMask layerMask;
	public PlayerIO _playerIO;
	public float JumpPow;
	public float acceleration;
	public float deceleration;
	private float runAcc = 1;


	private Vector3 currentVelocity = Vector3.zero;
	bool isRuning;
	float speed;
	Vector2 MoveVec;
	CapsuleCollider Pcollider;
	Rigidbody myRigidbody;
	Vector3 velocity;
	Animator anim;

	bool rotcacul = false;//테스트용 지우지 마세요
	bool isJump = false;//테스트용 지우지 마세요

	void Start () {
		_playerIO = new PlayerIO();
		myRigidbody = GetComponent<Rigidbody> ();
		anim = GetComponentInChildren<Animator>();
		Pcollider = GetComponent<CapsuleCollider>();
		speed = walkSpeed;
	}

	public void RotActionKey(InputAction.CallbackContext value)
    {
		rotcacul = value.started || value.performed; //눌렀는가. 아직 안땠는가
    }


	void Update () {
		LookMousePos();
		Move();
	}

	//void FixedUpdate()
	//{
	//
	//}

	//public void NewNextPos(float Angle)
    //{
	//
    //}
	public void OnMove(InputAction.CallbackContext value)
	{
		if (value.started)
			runAcc = 1f;

		MoveVec = value.ReadValue<Vector2>();
		anim.SetBool("isWalking", value.performed);
	}
	public void IsRun(InputAction.CallbackContext value)
	{
        if (value.started)
        {
			speed = sprintSpeed;
			anim.SetBool("isRun", true);
			isRuning = true;
			runAcc = 0.6f;
        }
		if (value.canceled)
		{
			speed = walkSpeed;
			anim.SetBool("isRun", false);
			isRuning = false;
			runAcc = 0.1f;


		}
	}

	void Move()
    {
		Vector3 targetVelocity = new Vector3(MoveVec.x, 0, MoveVec.y).normalized * speed;

		float lerpFactor = (targetVelocity.sqrMagnitude > 0) ? acceleration : deceleration;

		currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, lerpFactor * runAcc * Time.deltaTime);

		Debug.Log(runAcc);
		myRigidbody.MovePosition(myRigidbody.position + currentVelocity * Time.deltaTime);


    }
	public void Jump(InputAction.CallbackContext value)
    {
        if (value.started)
        {
			Jumping();
        }
    }
	void Jumping()
    {
		Debug.Log(IsGrounded());

		if (IsGrounded())
		{
			anim.SetTrigger("isJump");
			myRigidbody.AddForce(Vector3.up * JumpPow, ForceMode.Impulse);
			
		}
		
	}

	bool IsGrounded()
    {
		RaycastHit hitinfo;
		Physics.Raycast(transform.position, Vector3.down, out hitinfo);
		Debug.Log(hitinfo.distance);
		if (hitinfo.distance <= Pcollider.height / 2 + 0.1f)
			return true;
		else
			return false;

	}

	void LookMousePos() {
		Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());//얘는 XY만 가져오고 z를 따로 가져오지 않음 vector2
		RaycastHit hitinfo;
		Physics.Raycast(ray, out hitinfo, 100f, layerMask);// 괄호 누르고 컨트롤 + 스페이스바 layerMask는 저 레이어에 닿은것만 가져오겠다
		Vector3 mousePointDis = hitinfo.point - transform.position;
		mousePointDis.y = 0;
		transform.rotation = Quaternion.LookRotation(mousePointDis);
	}

    private void OnDrawGizmos()
    {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//얘는 마우스의 z좌표가 0이라고 가정한 vector3 값임
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, 100f, layerMask))
		{

			Gizmos.color = Color.red;
			Gizmos.DrawSphere(hit.point, 0.3f); 

		}
    }
}
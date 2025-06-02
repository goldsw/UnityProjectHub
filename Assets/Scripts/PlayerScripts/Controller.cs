using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour {
	private float acceleration;
	public float walkSpeed;
	public float sprintSpeed;
	public LayerMask layerMask;
	public PlayerIO _playerIO;
	public InputAction inputAction;
	public float JumpPow;
	public Vector3 direction;
	public float friction;


	Vector3 velocity = Vector3.zero;
	CapsuleCollider Pcollider;
	Rigidbody myRigidbody;
	Animator anim;

	bool rotcacul = false;
	bool isWalking = false;
	bool isRuning = false;
	bool isJump = false;

	int groundLayerMask;

    void Start () {
		_playerIO = new PlayerIO();
		_playerIO.Enable();
		inputAction = _playerIO.KeyBoard.Run;
		myRigidbody = GetComponent<Rigidbody> ();
		anim = GetComponentInChildren<Animator>();
		Pcollider = GetComponent<CapsuleCollider>();
        acceleration = walkSpeed;
		groundLayerMask |= (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Obstacles"));
    }

    public void RotActionKey(InputAction.CallbackContext value)
    {
		rotcacul = value.started || value.performed; //눌렀는가. 아직 안땠는가
    }


	void Update () {
		//LookMousePos();
		Debug.Log(inputAction.ReadValue<float>());
	}

    void FixedUpdate()
    {

		Move();
		Quaternion angle;
		if (direction != Vector3.zero)
		{
			angle = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
			SetRotate(angle);
		}
		//IsGrounded();
	}

	public void SetRotate(Quaternion targetAngle)
    {
		transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, 2.5f);
    }
    //public void NewNextPos(float Angle)
    //{
    //
    //}
    public void OnMove(InputAction.CallbackContext value)
	{
		isWalking = value.performed;
        if (value.started)
        {

		}
		if (value.performed)
        {
			
			direction = new Vector3(value.ReadValue<Vector2>().x, 0, value.ReadValue<Vector2>().y).normalized;
			anim.SetBool("isWalking", true);
			if(isRuning == false)
            {
				acceleration = walkSpeed;
            }
		}
        if (!value.performed)
        {
			anim.SetBool("isWalking", false);

		}
		if (value.canceled)
        {
			direction = Vector3.zero;
        }
	}


	public void IsRun(InputAction.CallbackContext value)
	{
		if (value.started)//눌렀다
        {
			isRuning = true;
        }
		if (value.performed)//누르고 있다
		{
			anim.SetBool("isRun", isRuning);
			acceleration = sprintSpeed;
			
		}
		if (value.canceled)//땠다
		{
			isRuning = false;
			anim.SetBool("isRun", isRuning);
		}
	}


	void Move()
	{
		Vector3 dir = Vector3.ProjectOnPlane(direction, RayToGround(groundLayerMask).normal).normalized;

		Vector3 acc = dir * acceleration * Time.fixedDeltaTime;
		velocity += acc;
		velocity = Vector3.ClampMagnitude(velocity, acceleration);
		velocity *= friction;
		if(dir == Vector3.zero && velocity.magnitude < 0.01f)
        {
			velocity = Vector3.zero;
        }
		myRigidbody.AddForce(transform.position + velocity * Time.fixedDeltaTime);

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

		if (myRigidbody != null)
		{
			Vector3 dir = Vector3.ProjectOnPlane(direction, RayToGround(groundLayerMask).normal).normalized;
			Gizmos.DrawLine(transform.position, transform.position + myRigidbody.velocity);
		}

	}
}
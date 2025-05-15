using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour {
	private float speed;
	public float walkSpeed;
	public float sprintSpeed;
	public LayerMask layerMask;
	public PlayerIO _playerIO;
	public float JumpPow;
	public Vector3 acceleration;
	[Range(0,1)]



	Vector3 velocity = Vector3.zero;
	CapsuleCollider Pcollider;
	Rigidbody myRigidbody;
	Animator anim;

	bool rotcacul = false;
	bool isRuning = false;
	bool isJump = false;

	int groundLayerMask;
    private RaycastHit groundHitInfo;

    void Start () {
		_playerIO = new PlayerIO();
		myRigidbody = GetComponent<Rigidbody> ();
		anim = GetComponentInChildren<Animator>();
		Pcollider = GetComponent<CapsuleCollider>();
        speed = walkSpeed;
		groundLayerMask |= (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Obstacles"));
    }

    public void RotActionKey(InputAction.CallbackContext value)
    {
		rotcacul = value.started || value.performed; //눌렀는가. 아직 안땠는가
    }


	void Update () {
		LookMousePos();
	}

    void FixedUpdate()
    {
		//IsGrounded();
		Move();
		
	}

    //public void NewNextPos(float Angle)
    //{
    //
    //}
    public void OnMove(InputAction.CallbackContext value)
	{
        if (value.started)
        {

		}
		if (value.performed)
        {
			acceleration = new Vector3(value.ReadValue<Vector2>().x, 0, value.ReadValue<Vector2>().y).normalized;
			anim.SetBool("isWalking", true);
			if(isRuning == false)
            {
				speed = walkSpeed;
            }
		}
        if (!value.performed)
        {
			anim.SetBool("isWalking", false);

		}
		if (value.canceled)
        {
			acceleration = Vector3.zero;
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
			speed = sprintSpeed;
			
		}
		if (value.canceled)//땠다
		{
			isRuning = false;
			anim.SetBool("isRun", isRuning);
		}
	}
	// _decIneria 는 멈출때의 관성이고, _inclnertia는 움직이려는 관성임. _check 값은 이걸 호출할 때 walk였는데 값이 바뀌면 코루틴을 멈춰줘야 하니까 넣은 값


	void Move()
	{
		Vector3 acc = acceleration * speed;
		if (Mathf.Abs(myRigidbody.velocity.y) <= 0.01f)
		{
			myRigidbody.velocity += acc * Time.fixedDeltaTime;
		}
		else
			myRigidbody.velocity += acc * Time.fixedDeltaTime;

//		myRigidbody.velocity *= 1f;
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
		IsGrounded();
		if (groundHitInfo.distance <= Pcollider.height / 2)
		{
			anim.SetTrigger("isJump");
			velocity = Vector3.up * JumpPow;
			myRigidbody.AddForce(Vector3.up * JumpPow, ForceMode.Impulse);
			
		}
		
	}

	void IsGrounded()
    {
		Ray ray = new Ray(transform.position, Vector3.down);
		Physics.SphereCast(ray, Pcollider.radius, out groundHitInfo, 100f, groundLayerMask);
		Debug.Log(groundLayerMask);

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
		if (groundHitInfo.transform != null)
		{
			Gizmos.DrawLine(transform.position, groundHitInfo.point);
			Gizmos.DrawSphere(groundHitInfo.point, Pcollider.radius);
		}
	}
}
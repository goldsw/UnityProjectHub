using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour {

	public float moveSpeed = 6;
	public LayerMask layerMask;
	public PlayerIO _playerIO;
	public float jumpSpeed;
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
	}

	public void RotActionKey(InputAction.CallbackContext value)
    {
		rotcacul = value.started || value.performed; //눌렀는가. 아직 안땠는가
    }


	void Update () {
		velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical")).normalized * moveSpeed;
		LookMousePos();
		myRigidbody.MovePosition (myRigidbody.position + velocity * Time.deltaTime);

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

		anim.SetBool("isWalking", value.performed);
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
			myRigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
			
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
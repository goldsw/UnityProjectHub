using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public Animator anim;
	private Vector3 direction;
	[HideInInspector] public PlayerMovement playerMovement;
	public PlayerIO _playerIO;
    private bool isRun = false;

    // Start is called before the first frame update


    private void Start()
    {
		_playerIO = new PlayerIO();
		_playerIO.Enable();
		anim = GetComponentInChildren<Animator>();
		playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {

	}
    private void OnDisable()
    {
    }

    private void Update()
    {
    }

    public void OnMove(InputAction.CallbackContext value)
	{

		if (value.started)
		{
		}
		if (value.performed)
		{

			direction = new Vector3(value.ReadValue<Vector2>().x, 0, value.ReadValue<Vector2>().y).normalized;
			playerMovement.inputDir = direction;
			anim.SetBool("isWalking", true);
		}
		if (!value.performed)
		{
			anim.SetBool("isWalking", false);

		}
		if (value.canceled)
		{
			playerMovement.inputDir = Vector3.zero;
		}
	}

	public void IsRun(InputAction.CallbackContext value)
	{
		if (value.started)//¥≠∑∂¥Ÿ
		{
			isRun = true;
		}
		if (value.performed)//¥©∏£∞Ì ¿÷¥Ÿ
		{
		}
		if (value.canceled)//∂Æ¥Ÿ
		{
			isRun = false;
		}
		anim.SetBool("isRun", isRun);
		playerMovement.run = value.ReadValue<float>();
		Debug.Log(value.ReadValue<float>());
	}
	public void Jump(InputAction.CallbackContext value)
	{
		if (value.started)
		{
			playerMovement.Jumping(anim);
		}
	}
}

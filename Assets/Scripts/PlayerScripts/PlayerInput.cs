using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public Animator anim;
	private Vector3 direction;
	[HideInInspector] public PlayerMovement playerMovement;
	public PlayerIO _playerIO;
	InputAction RunValue;
    // Start is called before the first frame update


    private void Start()
    {
		_playerIO = new PlayerIO();
		_playerIO.Enable();
		RunValue = _playerIO.KeyBoard.Run;
		anim = GetComponentInChildren<Animator>();
		playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {

	}
    private void OnDisable()
    {
		_playerIO.Disable();
    }

    private void Update()
    {
		Debug.Log(RunValue.ReadValue<float>());
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
			playerMovement.run = 1f;
		}
		if (value.performed)//¥©∏£∞Ì ¿÷¥Ÿ
		{
			anim.SetBool("isWalking", true);

		}
		if (value.canceled)//∂Æ¥Ÿ
		{
			playerMovement.run = 0f;
			anim.SetBool("isWalking", false);

		}
	}
	public void Jump(InputAction.CallbackContext value)
	{
		if (value.started)
		{

		}
	}
}

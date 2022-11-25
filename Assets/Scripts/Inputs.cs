using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
	//Singelton
	public static Inputs Instance { get; private set; }

	public PlayerControls Controls { get; private set; }

	public float Shift { get; private set; }
	public float LClick { get; private set; }

	private void Awake()
	{
		//Singelton
		if (Instance != null) Destroy(this);
		else Instance = this;

		Controls = new PlayerControls();

	}
	public void OnLeftClick(InputValue value)
	{
		Debug.Log("On Left Click called");
	}
	private void OnEnable()
	{
		Controls.Enable();
		Controls.Land.Shift.performed += _ => Shift = _.ReadValue<float>();
		Controls.Land.LeftClick.performed += _ => LClick = _.ReadValue<float>();
		Controls.Land.LeftClick.canceled+= _ => LClick = _.ReadValue<float>();
	}
	private void OnDisable()
	{
		Controls.Disable();
	}
}

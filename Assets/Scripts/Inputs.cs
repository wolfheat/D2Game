using UnityEngine;

public class Inputs : MonoBehaviour
{
	//Singelton
	public static Inputs Instance { get; private set; }

	public PlayerControls Controls { get; private set; }

	public float Shift { get; private set; }

	private void Awake()
	{
		//Singelton
		if (Instance != null) Destroy(this);
		else Instance = this;

		Controls = new PlayerControls();

	}
	private void OnEnable()
	{
		Controls.Enable();
		Controls.Land.Shift.performed += _ => Shift = _.ReadValue<float>();
	}
	private void OnDisable()
	{
		Controls.Disable();
	}
}

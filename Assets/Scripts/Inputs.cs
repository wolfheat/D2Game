using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
	//Singelton
	public static Inputs Instance { get; private set; }

	public PlayerControls Controls { get; private set; }
	public float Zooming { get; private set; }
	public bool PointerOverUI { get; private set; }

    private void Update()
	{
		PointerOverUI = EventSystem.current.IsPointerOverGameObject();
	}

	private bool? destroyed;

	private void Awake()
	{
		//Singelton
		if (Instance != null){	
			Destroy(this);
			return;
		}

		Instance = this;
        Controls = new PlayerControls();
	}
	public void OnLeftClick(InputValue value)
	{
		Debug.Log("On Left Click called");
	}
	private void OnEnable()
	{
		Debug.Log("Inputs onEnable run, Destroyed: "+destroyed);
		Controls.Enable();
		// ARE these aven needed?
		//Controls.Land.LeftClick.performed += _ => LClick = _.ReadValue<float>();
		//Controls.Land.LeftClick.canceled+= _ => LClick = _.ReadValue<float>();
		Debug.Log("Inputs onEnable run");
	}
	private void OnDisable()
	{
		if (destroyed == true)
		{
			Debug.Log("Inputs onDisable run for destroyed item");
			return;
		}

        //Controls.Land.LeftClick.performed -= _ => LClick = _.ReadValue<float>();
        //Controls.Land.LeftClick.canceled -= _ => LClick = _.ReadValue<float>();

        Controls.Disable();
	}
}

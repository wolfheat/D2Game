using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
	//Singelton
	public static Inputs Instance { get; private set; }

	public PlayerControls Controls { get; private set; }

	public float Shift { get; private set; }
	public float F { get; private set; }
	public float G { get; private set; }
	public float X { get; private set; }
	public float CTRL { get; private set; }
	//public float LClick { get; private set; }	
	public float Space { get; private set; }
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
			destroyed = true;
			Debug.Log("Inputs Copy deleted");
			Destroy(this);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(this);
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
		Controls.Land.CTRL.performed += _ => CTRL = _.ReadValue<float>();
		Controls.Land.Shift.performed += _ => Shift = _.ReadValue<float>();
		Controls.Land.Shift.canceled += _ => Shift = _.ReadValue<float>();
		Controls.Land.Space.performed += _ => Space = _.ReadValue<float>();
		Controls.Land.Space.canceled+= _ => Space = _.ReadValue<float>();
		Controls.Land.CTRL.canceled+= _ => CTRL = _.ReadValue<float>();
		Controls.Land.F.performed += _ => F = _.ReadValue<float>();
		Controls.Land.F.canceled+= _ => F = _.ReadValue<float>();
		Controls.Land.G.performed += _ => G = _.ReadValue<float>();
		Controls.Land.G.canceled+= _ => G = _.ReadValue<float>();
		Controls.Land.X.performed += _ => X = _.ReadValue<float>();
		Controls.Land.X.canceled+= _ => X = _.ReadValue<float>();
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
        Controls.Land.CTRL.performed -= _ => CTRL = _.ReadValue<float>();
        Controls.Land.Shift.performed -= _ => Shift = _.ReadValue<float>();
        Controls.Land.Shift.canceled -= _ => Shift = _.ReadValue<float>();
        Controls.Land.Space.performed -= _ => Space = _.ReadValue<float>();
        Controls.Land.Space.canceled -= _ => Space = _.ReadValue<float>();
        Controls.Land.CTRL.canceled -= _ => CTRL = _.ReadValue<float>();
        Controls.Land.F.performed -= _ => F = _.ReadValue<float>();
        Controls.Land.F.canceled -= _ => F = _.ReadValue<float>();
        Controls.Land.G.performed -= _ => G = _.ReadValue<float>();
        Controls.Land.G.canceled -= _ => G = _.ReadValue<float>();
        Controls.Land.X.performed -= _ => X = _.ReadValue<float>();
        Controls.Land.X.canceled -= _ => X = _.ReadValue<float>();


        Controls.Disable();
	}
}

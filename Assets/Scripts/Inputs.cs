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
		//Debug.Log("Enable: "+gameObject.GetInstanceID());
		Controls.Enable();		
	}
	private void OnDisable()
	{
		//Debug.Log("Disable: "+gameObject.GetInstanceID());
        Controls.Disable();
	}
}

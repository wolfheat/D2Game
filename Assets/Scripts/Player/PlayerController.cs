using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class ClickInfo
{
	public ClickType type;
	public PlayerActionType actionType;
	public Vector3 pos = Vector3.zero;
	//Constructor
	public ClickInfo(ClickType t, PlayerActionType pt, Vector3 p) { type = t; actionType = pt; pos = p; }	
}
public enum ClickType {Left,Right}
public enum PlayerActionType {Attack, PowerAttack ,Move, Gather, Undefined}

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Collider attackCollider;
	[SerializeField] private LayerMask GroundLayers;
	[SerializeField] private LayerMask Resource;
	[SerializeField] private LayerMask TerrainLayers;
	[SerializeField] private LayerMask UI;
	[SerializeField] private TextMeshProUGUI stateText;

	private PlayerStateControl playerState;
	private Camera mainCamera;
	private WayPointController WayPointController;
	private NavMeshAgent navMeshAgent;
	private SoundMaster soundmaster;

	private ClickInfo clickInfo;
	private ClickInfo savedAction;
	private ClickInfo wayPointToShow;
		
	private float attackTime = 1.22f;
	public float attackSpeedMultiplier = 1.8f;

	private const float StopDistance = 0.1f;
	private const float MinGatherDistance = 1f;
	private float mouseClickTimer = 0f;
	private const float HoldMouseDuration = 0.2f;
	private const float RotationSpeed = 900f;
	private bool attackLock = false;
	private bool attackDidHit = false;
	private bool holdPosition = false;

	private Coroutine gatherCoroutine;

	private void OnEnable()
	{
		mainCamera = Camera.main;
		Inputs.Instance.Controls.Land.LeftClick.performed += _ => MouseClick();
		Inputs.Instance.Controls.Land.LeftClick.canceled += _ => ShowWaypointIfAvailable();
		Inputs.Instance.Controls.Land.RightClick.performed += _ => MouseRightClick();
		Inputs.Instance.Controls.Land.RightClick.canceled += _ => ShowWaypointIfAvailable();
		Inputs.Instance.Controls.Land.Shift.started += Shift;
		Inputs.Instance.Controls.Land.Shift.canceled += Shift;
	}

	private void Start()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		playerState = GetComponent<PlayerStateControl>();
		WayPointController = FindObjectOfType<WayPointController>();
		soundmaster = FindObjectOfType<SoundMaster>();


	}
	private void Update()
	{
		CheckIfReachedTarget();
		GetPlayerInput();
	}

	private void CheckIfReachedTarget()
	{
		if(navMeshAgent.remainingDistance < 0.1f && playerState.State == PlayerState.MoveTo)
		{
			playerState.SetState(PlayerState.Idle);
			// animator.Play("Idle"); Used to have this, needed for smooth transition or removable?
			navMeshAgent.isStopped = true;
		}
	}
	
	public void SetToPosition(Vector3 pos)
	{
		transform.position = pos;
	}


	private void GetPlayerInput()
	{
		mouseClickTimer += Time.deltaTime;
		if (Inputs.Instance.LClick == 1f && mouseClickTimer > HoldMouseDuration)
		{
			// Left button is held
			MouseClick();
			mouseClickTimer = 0;
		}else if(Inputs.Instance.LClick != 1f) mouseClickTimer = 0;
    }
	

	public void EnableNavMesh(bool enable)
	{
		if (navMeshAgent) navMeshAgent.enabled = enable;
		else Debug.LogError("No Nav Mesh available");
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.layer == LayerMask.NameToLayer("Default"))
		{
			StopInPlace();

		}
		if (hit.gameObject.layer == LayerMask.NameToLayer("Enemies"))
		{
			StopInPlace();
		}
	}

	private void StopInPlace()
	{
		// get position X distance in front of player
		navMeshAgent.SetDestination(transform.position+transform.forward*StopDistance);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Arrows"))
		{
			//Debug.Log("DESTROY ARROW");
			Arrow arrow = collision.gameObject.GetComponent<Arrow>();
			//Debug.Log("Hit By Arrow");
			arrow.transform.parent = transform;
			//arrow.Destroy();
		}	
	}
	private void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
		{
			Enemy enemy = collider.gameObject.GetComponent<Enemy>();
			enemy.Hit(25);
			attackDidHit = true;
		}		
	}

	private bool GetClickInfo(ClickType type)
	{
		// Clicking UI element, ignore gameplay clicks
		if (Inputs.Instance.PointerOverUI){	return false;}

		// Get Player ActionType = Gather, Move, Attack, Powerattack
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
		RaycastHit hit;
        Physics.Raycast(ray.origin, ray.direction, out hit, 1000f);
		Vector3 clickPoint = new Vector3(hit.point.x,0,hit.point.z);

        PlayerActionType playerActionType = DeterminePLayerActionType(ray, type, out hit);

        NavMeshHit navhit;
        if (!NavMesh.SamplePosition(clickPoint, out navhit, 1f, NavMesh.AllAreas))
        {
            // The clicked point is outside the NavMesh
            // Find the closest point on the NavMesh to the clicked point
            navhit.position = GetClosestClickPointWhenClickingOutside(clickPoint);            
        }

		Vector3 clickPosition = playerActionType == PlayerActionType.Gather ? hit.point : navhit.position;

		// Set ClickInfo
        clickInfo = new ClickInfo(type, playerActionType, clickPosition);
		return true;
		
	}

    private PlayerActionType DeterminePLayerActionType(Ray ray,ClickType type , out RaycastHit hit)
    {
        bool clickOnResource = Physics.Raycast(ray.origin, ray.direction, out hit, 1000f, layerMask: Resource);
        PlayerActionType playerActionType = PlayerActionType.Move;
        if (holdPosition) playerActionType = (type == ClickType.Right ? PlayerActionType.PowerAttack : PlayerActionType.Attack);
        else if (clickOnResource) playerActionType = PlayerActionType.Gather;
		return playerActionType;
    }

    private Vector3 GetClosestClickPointWhenClickingOutside(Vector3 point)
    {		
		Vector3 pointInPlane = new Vector3(point.x,transform.position.y,point.z);
		Debug.Log("Point in plane: "+pointInPlane);
		Vector3 calculatedDestination = transform.position;
		Vector3 step = (pointInPlane - transform.position).normalized*0.3f;
		Vector3 pointToCheck = calculatedDestination;
		
		int steps = 0;

		Debug.Log("CalculatedDestination starts at: "+calculatedDestination+" with stepsize: "+step);
		

		NavMeshHit navhit;

		while (NavMesh.SamplePosition(pointToCheck,out navhit,0.1f,NavMesh.AllAreas) && steps < 200) 
		{
			calculatedDestination = pointToCheck;
            pointToCheck += step;
			steps++;
		}
		Debug.Log("Exited derived click point after iterations: "+steps+" point: "+calculatedDestination);
		return calculatedDestination;
    }

    private void DoClickAction()
	{
		if(gatherCoroutine!=null && clickInfo.actionType != PlayerActionType.Gather) AnimationStopGatheringEvent();
		//Debug.Log("Do action of type: "+clickInfo.actionType);
		switch (clickInfo.actionType)
		{
			case PlayerActionType.Attack:
				StartCoroutine(AttackAt()); 
				StoreWaypointToShow(clickInfo);
				break;
			case PlayerActionType.PowerAttack:
				StartCoroutine(AttackAt());
				StoreWaypointToShow(clickInfo);
				break;
			case PlayerActionType.Move:
				NavigateTo(); 
				StoreWaypointToShow(clickInfo);
				break;
			case PlayerActionType.Gather:
				gatherCoroutine = StartCoroutine(GatherAt(clickInfo.pos));
				break;
			default:	
			break;
		}
	}
	private void NavigateTo()
	{
		navMeshAgent.isStopped = false;
		playerState.SetState(PlayerState.MoveTo);		
		navMeshAgent.SetDestination(clickInfo.pos);
	}

	private IEnumerator GatherAt(Vector3 pos)
	{
		//If to far move closer
		if((transform.position-pos).magnitude > MinGatherDistance) 
		{
			playerState.SetState(PlayerState.MoveToGather);
			navMeshAgent.isStopped = false;
			navMeshAgent.SetDestination(pos);
			Debug.Log("Setting Move To Gatherpoint for navmesh");
		}
		while ((transform.position - pos).magnitude > MinGatherDistance)
		{ 
			yield return null;		
		}
		navMeshAgent.SetDestination(transform.position);

		playerState.SetState(PlayerState.Gather);
		soundmaster.PlaySFX(SoundMaster.SFX.Gather);		
	}

	private IEnumerator AttackAt()
	{
		Debug.Log("Starting Attack");
		attackLock = true;
		// Workaround for going from attack to attack
		if (playerState.State == PlayerState.AttackSwordSwing)
		{
			playerState.SetState(PlayerState.Idle);
			yield return null;			
		}
		
		//Face position
		Vector3 endLookDirection = (clickInfo.pos-transform.position).normalized;
		Quaternion startRotation = transform.rotation;
		Quaternion targetRotation = Quaternion.LookRotation(endLookDirection,Vector3.up);

		// Rotate at constant speed but with max timeout
		bool rotationComplete = false;
		while (!rotationComplete)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed*Time.deltaTime);
			if(Quaternion.Angle(transform.rotation,targetRotation)<10f) rotationComplete = true;
			yield return null;
		}
		transform.rotation = targetRotation;
		playerState.SetState(PlayerState.AttackSwordSwing);

		// Wait For Attack to finish
		yield return new WaitForSeconds(attackTime/attackSpeedMultiplier);

		attackLock = false;
		
		yield return null;
		LoadAction();
	}

	private void LoadAction()
	{
		if (savedAction != null)
		{
			clickInfo = savedAction;
			DoClickAction();
			savedAction = null;
		}
		else
		{
			playerState.SetState(PlayerState.Idle);
		}
	}

	private void SaveAction()
	{
		//Debug.Log("Saving action");
		savedAction = clickInfo;
		StoreWaypointToShow(clickInfo);		
	}

	private void StoreWaypointToShow(ClickInfo savedAction)
	{
		//Debug.Log("Saving wayPointToShow: "+savedAction);
		wayPointToShow = savedAction;
	}

	// --------CONTROLLER INPUTS--------------------------------------------------------
	private void ShowWaypointIfAvailable() // Left/Right-Click Released
	{
		if (wayPointToShow != null)
		{
			//Debug.Log("There is a waypoint to show here");
			WayPointController.ShowWaypoint(wayPointToShow);
		}
		wayPointToShow = null;
	}

	private void MouseRightClick()
	{
		bool validClick = GetClickInfo(ClickType.Right);
		if (!validClick) Debug.Log("Unvalid click point (Right)");
		else
		{
			if (attackLock) { 
				SaveAction(); 
				Debug.Log("STORE INPUT, ATTACKLOCK ACTIVE"); 
			}
			else DoClickAction();
		}
	}

	private void MouseClick()
	{
		bool validClick = GetClickInfo(ClickType.Left);
		if (!validClick) Debug.Log("Unvalid click point (Left)");
		else
		{
			if (attackLock) { 
				SaveAction(); 
				Debug.Log("STORE INPUT, ATTACKLOCK ACTIVE");
			}
			else DoClickAction();
		}
	}

	private void Shift(InputAction.CallbackContext context)
	{
		if (context.phase == InputActionPhase.Started)
		{
			holdPosition = true;
			StopInPlace();
		}
		else if (context.phase == InputActionPhase.Canceled)
		{
			holdPosition = false;
		}		
	}

	// --------ANIMATION EVENTS--------------------------------------------------------
	private void AnimationStopGatheringEvent()
	{
		
		StopCoroutine(gatherCoroutine);
		gatherCoroutine = null;
		//Debug.Log("Gathering Complete Event");
		navMeshAgent.destination = transform.position;
		playerState.SetState(PlayerState.Idle);
		soundmaster.StopSFX();
	}
	private void AnimationStepEvent()
	{
		//Debug.Log("Animation Step Event");
		soundmaster.StopStepSFX();
		soundmaster.PlayStepSFX();
	}

	private IEnumerator AnimationShootArrowEvent()
	{
		//Debug.Log("Shoot Arrow Event");
		yield return new WaitForSeconds(0.05f);
		soundmaster.PlaySFX(SoundMaster.SFX.ShootArrow);
	}
	
	private IEnumerator AnimationSwordSwingEvent()
	{
		//Debug.Log("SwordSwing Event");

		attackDidHit = false;
		attackCollider.enabled = true;
		yield return new WaitForSeconds(0.05f);
		attackCollider.enabled = false;

		if (attackDidHit) soundmaster.PlaySFX(SoundMaster.SFX.SwordHit);
		else soundmaster.PlaySFX(SoundMaster.SFX.SwingSwordMiss);

	}
}

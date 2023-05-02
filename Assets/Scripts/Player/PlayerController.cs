using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class ClickInfo
{
	public ClickType type;
	public PlayerActionType actionType;
	public Vector3 pos = Vector3.zero;
	public Vector3 aim = Vector3.zero;
	public ClickInfo(ClickType t, PlayerActionType pt, Vector3 p, Vector3 a) { type = t; actionType = pt; pos = p; aim = a; }	
}
public enum ClickType {Left,Right}
public enum WeaponType {Sword,Bow}
public enum PlayerActionType {Attack, PowerAttack ,Move, Gather, Undefined}


public class PlayerController : PlayerUnit
{
	[SerializeField] private Collider attackCollider;
	[SerializeField] private LayerMask GroundLayers;
	[SerializeField] private LayerMask ResourceLayer;
	[SerializeField] private LayerMask TerrainLayers;
	[SerializeField] private LayerMask UI;
	[SerializeField] private TextMeshProUGUI stateText;
	[SerializeField] private GameObject[] weapons;

    private PlayerAnimationEventController playerAnimationEventController;
	private Camera mainCamera;
	private WayPointController WayPointController;
	private SoundMaster soundmaster;

	private ClickInfo clickInfo;
	private ClickInfo savedAction;
	private ClickInfo wayPointToShow;
		
	private WeaponType currentWeapon = WeaponType.Sword;

	private ResourceNode activeNode;
    public ResourceNode ActiveNode { get { return activeNode; }}

    // Settings - Constants
	private const float StopDistance = 0.1f;
	private const float MinGatherDistance = 1.5f;
	private const float HoldMouseDuration = 0.2f;
	private const float RotationSpeed = 900f;


	private float mouseClickTimer = 0f;
	private bool attackLock = false;
	private bool holdPosition = false;

	private Coroutine gatherCoroutine;

	private void OnEnable()
    {
        mainCamera = Camera.main;
		Inputs.Instance.Controls.Land.LeftClick.performed += _ => MouseClick(ClickType.Left);
		Inputs.Instance.Controls.Land.LeftClick.canceled += _ => ShowWaypointIfAvailable();
		Inputs.Instance.Controls.Land.RightClick.performed += _ => MouseClick(ClickType.Right);
		Inputs.Instance.Controls.Land.RightClick.canceled += _ => ShowWaypointIfAvailable();
		Inputs.Instance.Controls.Land.W.started += _ => SwapWeapon();
		Inputs.Instance.Controls.Land.Shift.started += Shift;
		Inputs.Instance.Controls.Land.Shift.canceled += Shift;
	}

	private void SwapWeapon()
	{
		// Dont allow swapping if weapon is used
		if (attackLock) return;

		ChangeCurrentWeapon();		
	}

    private void ChangeCurrentWeapon()
    {
        CurrentWeaponVisualsEnabled(false);
        currentWeapon = currentWeapon == WeaponType.Sword ? WeaponType.Bow : WeaponType.Sword;
        CurrentWeaponVisualsEnabled();

    }

    private void CurrentWeaponVisualsEnabled(bool enable = true)
    {		
		weapons[(int)currentWeapon].gameObject.SetActive(enable);
    }

    private void Start()
	{
        Debug.Log("PlayerController START");

        playerAnimationEventController = GetComponent<PlayerAnimationEventController>();

		WayPointController = FindObjectOfType<WayPointController>();
		soundmaster = FindObjectOfType<SoundMaster>();
		
    }
	protected override void Update()
	{
		MouseButtonHeldCheck();

		base.Update();
	} 

	private void MouseButtonHeldCheck()
	{
		mouseClickTimer += Time.deltaTime;
		if (Inputs.Instance.LClick == 1f && !attackLock && mouseClickTimer > HoldMouseDuration)
		{
			// Left button is held
			MouseClick(ClickType.Left);
			mouseClickTimer = 0;
		}else if(Inputs.Instance.LClick != 1f) mouseClickTimer = 0;
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

    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            enemy.Hit(CharacterStats.HitDamage);
            playerAnimationEventController.AttackDidHit = true;
        }
    }
    private void StopInPlace()
	{
		// get position X distance in front of player
		navMeshAgent.SetDestination(transform.position+transform.forward*StopDistance);
		if (gatherCoroutine != null) playerAnimationEventController.ForcedStopGatheringEvent();
	}

	private bool GetClickInfo(ClickType type)
	{
		// Clicking UI element, ignore gameplay clicks
		if (Inputs.Instance.PointerOverUI){	return false;}

		// Get Player ActionType = Gather, Move, Attack, Powerattack
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 1000f);

        // OLD CLICKPOINT Vector3 clickPoint = new Vector3(hit.point.x, 0, hit.point.z);
        Vector3 clickPoint = hit.point;

        Vector3 aim = playerAnimationEventController.ShootPoint;

        float lineScalarValue =  (aim.y - ray.origin.y) /ray.direction.y;
		Vector3 aimPoint = ray.origin + ray.direction * lineScalarValue;

        PlayerActionType playerActionType = DeterminePlayerActionType(ray, type, ref hit);

        if (!NavMesh.SamplePosition(clickPoint, out NavMeshHit navhit, 1f, NavMesh.AllAreas))
        {
            // The clicked point is outside the NavMesh, Find the closest point on the NavMesh to the clicked point
            navhit.position = GetClosestClickPointWhenClickingOutside(clickPoint);            
        }

		// Determin what position was clicked
		Vector3 clickPosition = playerActionType == PlayerActionType.Gather ? hit.point : navhit.position;

		// Set ClickInfo i.e (LeftMouse, MoveTo, Position)
        clickInfo = new ClickInfo(type, playerActionType, clickPosition, aimPoint);
		return true;
		
    }

    private PlayerActionType DeterminePlayerActionType(Ray ray,ClickType type , ref RaycastHit hit)
    {
		// Check if "Hold position" is down (Shift) and what type of click was made. 
		if (holdPosition) return (type == ClickType.Right ? PlayerActionType.PowerAttack : PlayerActionType.Attack);

		// Check if clicked on resource
		if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f, layerMask: ResourceLayer))
		{
			// Get clicked Node if available
			if (hit.collider.TryGetComponent(out activeNode))
			{
				Debug.Log("Hit a Resource");
			}

            return PlayerActionType.Gather;
		}

        // Return Default = Move
        return PlayerActionType.Move;
    }

    private Vector3 GetClosestClickPointWhenClickingOutside(Vector3 point)
    {
		float stepSize = 0.3f;

		// Move the clicked point to be in the players plane
		Vector3 pointInPlane = new Vector3(point.x,transform.position.y,point.z);

		// Step size vector for the iteration towards the clicked point
		Vector3 step = (pointInPlane - transform.position).normalized*stepSize;

		// The Methods resulting point
		Vector3 calculatedDestination = transform.position+step;
		
		// Iterate from the players position towards the clicked point, with max steps = 200
		for (int i = 0; i < 200; i++)
		{
			if(!NavMesh.SamplePosition(calculatedDestination,out NavMeshHit navHit, 0.1f, NavMesh.AllAreas)) 
				return calculatedDestination-step;
			calculatedDestination += step;
		}		
		return calculatedDestination;
    }

    private void DoClickAction()
	{
		// Currently gathering but requesting anything else
		if(gatherCoroutine!=null && clickInfo.actionType != PlayerActionType.Gather) playerAnimationEventController.ForcedStopGatheringEvent();
        
		playerAnimationEventController.Aim = clickInfo.aim;

        switch (clickInfo.actionType)
		{
			case PlayerActionType.Attack:
				StartCoroutine(AttackAt()); 
				wayPointToShow = clickInfo;
				break;
			case PlayerActionType.PowerAttack:
				StartCoroutine(AttackAt());
				wayPointToShow = clickInfo;
				break;
			case PlayerActionType.Move:
				NavigateTo(); 
				wayPointToShow = clickInfo;
				break;
			case PlayerActionType.Gather:
				gatherCoroutine = StartCoroutine(GatherAt(activeNode));
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

	private IEnumerator GatherAt(ResourceNode node)
	{
		Vector3 pos = node.transform.position;	
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
			Debug.Log("Distance to Resounce: "+ (transform.position - pos).magnitude);
			yield return null;		
		}
		navMeshAgent.SetDestination(transform.position);

		playerState.SetState(PlayerState.Gather);
		soundmaster.PlaySFX(SoundMaster.SFX.Gather);		
	}

	private IEnumerator FaceAttackDirection()
	{
        //Face position
        Vector3 endLookDirection = (clickInfo.pos - transform.position).normalized;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(endLookDirection, Vector3.up);

        // Rotate at constant speed but with max timeout
        bool rotationComplete = false;
        while (!rotationComplete)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 10f) rotationComplete = true;
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    private IEnumerator AttackAt()
	{
		// Depending on Current Weapon Implement correct attack

		attackLock = true;
		// Workaround for going from attack to attack
		if (playerState.State == PlayerState.AttackSwordSwing || playerState.State == PlayerState.ShootArrow)
		{
			playerState.SetState(PlayerState.Idle);
			yield return null;			
		}
		
		yield return StartCoroutine(FaceAttackDirection());

		if(currentWeapon == WeaponType.Sword) playerState.SetState(PlayerState.AttackSwordSwing);
		else playerState.SetState(PlayerState.ShootArrow);

        // Wait For Attack to finish
        yield return new WaitForSeconds(CharacterStats.AttackTime / CharacterStats.AttackSpeedMultiplyer);

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
		savedAction = clickInfo;
		wayPointToShow = clickInfo;		
	}

	// --------CONTROLLER INPUTS--------------------------------------------------------
	private void ShowWaypointIfAvailable() 
	{
        // Left or Right Mouse Button Released
        if (wayPointToShow != null)
		{
			WayPointController.ShowWaypoint(wayPointToShow);
		}
		wayPointToShow = null;
	}

	private void MouseClick(ClickType type)
	{
        bool validClick = GetClickInfo(type);
        if (validClick)
        {
            if (attackLock) SaveAction();
            else DoClickAction();
        }
		// Else not valid = Over UI etc
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

    public void GatherNodeIfActive()
    {
        if (activeNode != null)
            activeNode.Harvest();
    }

    public void StopGathering()
    {
        if (gatherCoroutine != null) StopCoroutine(gatherCoroutine);
        gatherCoroutine = null;
    }

    internal void TakeHit(int damage)
    {
		Debug.Log("Player Take Hit: "+damage);
    }
}

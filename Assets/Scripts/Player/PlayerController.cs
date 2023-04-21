using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ClickInfo
{
	public ClickType type;
	public PlayerActionType actionType;
	public Vector3 pos = Vector3.zero;
	//Constructor
	public ClickInfo(ClickType t, PlayerActionType pt, Vector3 p) { type = t; actionType = pt; pos = p; }	
}
public enum ClickType {Left,Right}
public enum WeaponType {Sword,Bow}
public enum PlayerActionType {Attack, PowerAttack ,Move, Gather, Undefined}

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Collider attackCollider;
	[SerializeField] private LayerMask GroundLayers;
	[SerializeField] private LayerMask Resource;
	[SerializeField] private LayerMask TerrainLayers;
	[SerializeField] private LayerMask UI;
	[SerializeField] private TextMeshProUGUI stateText;

	[SerializeField] private GameObject[] weapons;

	[SerializeField] private GameObject shootPoint;
    private ArrowHolder arrowHolder;
	private Projectiles projectiles;

    private PlayerStateControl playerState;
	private Camera mainCamera;
	private WayPointController WayPointController;
	private NavMeshAgent navMeshAgent;
	private SoundMaster soundmaster;

	private ClickInfo clickInfo;
	private ClickInfo savedAction;
	private ClickInfo wayPointToShow;
		
	private WeaponType currentWeapon = WeaponType.Sword;

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
        arrowHolder = FindObjectOfType<ArrowHolder>();
        projectiles = FindObjectOfType<Projectiles>();
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
		if (attackLock) return;

		ChangeCurrentWeapon();		
		SwapWeaponVisuals();
		

	}

    private void ChangeCurrentWeapon()
    {
        if (currentWeapon == WeaponType.Sword) currentWeapon = WeaponType.Bow;
        else currentWeapon = WeaponType.Sword;
    }

    private void SwapWeaponVisuals()
    {
		foreach (var weapon in weapons)
		{
			weapon.gameObject.SetActive(false);
		}
		weapons[(int)currentWeapon].gameObject.SetActive(true);
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
			MouseClick(ClickType.Left);
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
        
        Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 1000f);
		
		Vector3 clickPoint = new Vector3(hit.point.x, 0, hit.point.z);

        PlayerActionType playerActionType = DeterminePlayerActionType(ray, type, ref hit);

        if (!NavMesh.SamplePosition(clickPoint, out NavMeshHit navhit, 1f, NavMesh.AllAreas))
        {
            // The clicked point is outside the NavMesh, Find the closest point on the NavMesh to the clicked point
            navhit.position = GetClosestClickPointWhenClickingOutside(clickPoint);            
        }

		// Determin what position was clicked
		Vector3 clickPosition = playerActionType == PlayerActionType.Gather ? hit.point : navhit.position;

		// Set ClickInfo i.e (LeftMouse, MoveTo, Position)
        clickInfo = new ClickInfo(type, playerActionType, clickPosition);
		return true;
		
    }

    private PlayerActionType DeterminePlayerActionType(Ray ray,ClickType type , ref RaycastHit hit)
    {
		// Check if "Hold position" is down (Shift) and what type of click was made. 
		if (holdPosition) return (type == ClickType.Right ? PlayerActionType.PowerAttack : PlayerActionType.Attack);

		// Check if clicked on resource
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f, layerMask: Resource)) return PlayerActionType.Gather;

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
		if(gatherCoroutine!=null && clickInfo.actionType != PlayerActionType.Gather) AnimationStopGatheringEvent();

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

		Debug.Log("Starting Attack");
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
	private void AnimationStopGatheringEvent()
	{
		if(gatherCoroutine != null) StopCoroutine(gatherCoroutine);
		gatherCoroutine = null;

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
		yield return new WaitForSeconds(0.05f);

		// Shoot the arrow
        Arrow newArrow = Instantiate(projectiles.arrowPrefab, arrowHolder.transform, false);
        newArrow.transform.position = shootPoint.transform.position;// transform.position;
        newArrow.transform.rotation = transform.rotation;
        newArrow.destructable = true;

		// Play Sound
		soundmaster.PlaySFX(SoundMaster.SFX.ShootArrow);
    }
	
	private IEnumerator AnimationSwordSwingEvent()
	{
		// Runs at attack moment to check what enemies were hit
		attackDidHit = false;
		attackCollider.enabled = true;
		yield return new WaitForSeconds(0.05f);
		attackCollider.enabled = false;

		// Play Sound
		if (attackDidHit) soundmaster.PlaySFX(SoundMaster.SFX.SwordHit);
		else soundmaster.PlaySFX(SoundMaster.SFX.SwingSwordMiss);

	}
}

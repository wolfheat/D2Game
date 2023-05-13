using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Playables;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class ClickInfo
{
	public IInteractable interactable;	
	public PlayerActionType actionType;
	public Vector3 pos = Vector3.zero;
	public Vector3 aim = Vector3.zero;
	public ClickInfo(PlayerActionType pt, Vector3 p, Vector3 a, IInteractable i) { actionType = pt; pos = p; aim = a; i = interactable; }	
}
public enum ClickType {Left,Right}
public enum WeaponType {Sword,Bow}
public enum PlayerActionType {Attack, PowerAttack ,Move, Interact, Gather, Stash, Undefined }


public class PlayerController : PlayerUnit
{
	[SerializeField] private Collider attackCollider;
	[SerializeField] private LayerMask GroundLayers;
	[SerializeField] private LayerMask ResourceLayer;
	[SerializeField] private LayerMask TerrainLayers;
	[SerializeField] private LayerMask Interactable;
	[SerializeField] private LayerMask UI;
	[SerializeField] private GameObject[] weapons;

    private PlayerAnimationEventController playerAnimationEventController;
	private Camera mainCamera;
	private WayPointController WayPointController;
	private SoundMaster soundmaster;

	private ClickInfo clickInfo;
	private ClickInfo savedAction;
	private ClickInfo wayPointToShow;
		
	private WeaponType currentWeapon = WeaponType.Sword;

	public IInteractable activeInteractable;
    public IInteractable ActiveNodeActiveInteractable { get { return activeInteractable; }}
    public bool IsDead { get; private set;}

    // Settings - Constants
	private const float StopDistance = 0.1f;
	private const float MinGatherDistance = 1.5f;
	private const float HoldMouseDuration = 0.1f;
	private const float RotationSpeed = 900f;


	private float mouseClickTimer = 0f;
	private bool attackLock = false;
	private bool holdPosition = false;

	private Coroutine interactCoroutine;
    private bool validClick;
    [SerializeField] private float distance;

    private void OnEnable()
    {
		Debug.Log("Player Controller Enable RUN" + GetInstanceID());
        mainCamera = Camera.main;
		Inputs.Instance.Controls.Land.LeftClick.started += LeftClick;
		Inputs.Instance.Controls.Land.LeftClick.canceled += LeftClick;
		Inputs.Instance.Controls.Land.RightClick.started += RightClick;
		Inputs.Instance.Controls.Land.RightClick.canceled += RightClick;

        Inputs.Instance.Controls.Land.W.started += SwapWeapon;
        Inputs.Instance.Controls.Land.Shift.started += Shift;
        Inputs.Instance.Controls.Land.Shift.canceled += Shift;
	}
	private void OnDisable()
    {
		Debug.Log("Player Controller Disable RUN, for instance: "+GetInstanceID());
        Inputs.Instance.Controls.Land.LeftClick.started -= LeftClick;
        Inputs.Instance.Controls.Land.LeftClick.canceled -= LeftClick;
        Inputs.Instance.Controls.Land.RightClick.started -= RightClick;
        Inputs.Instance.Controls.Land.RightClick.canceled -= RightClick;

        Inputs.Instance.Controls.Land.W.started -= SwapWeapon;
        Inputs.Instance.Controls.Land.Shift.started -= Shift;
        Inputs.Instance.Controls.Land.Shift.canceled -= Shift;

    }

	private void SwapWeapon(InputAction.CallbackContext ctx)
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
        Debug.Log("Check if playerAnimationEventController exists: " + playerAnimationEventController);

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
		if (validClick && Inputs.Instance.Controls.Land.LeftClick.inProgress && !attackLock && mouseClickTimer > HoldMouseDuration)
		{
			// Left button is held
			//Sample mouse again
			if(MouseClick())
				SaveOrExecutePlayerAction();
			mouseClickTimer = 0;
		}else if(!Inputs.Instance.Controls.Land.LeftClick.inProgress) mouseClickTimer = 0;
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
            enemy.Hit(SavingUtility.Instance.playerInventory.HitDamage);
            playerAnimationEventController.AttackDidHit = true;
        }
    }

    private void StopInPlace()
	{
		// get position X distance in front of player
		navMeshAgent.SetDestination(transform.position+transform.forward*StopDistance);
		if (interactCoroutine != null) playerAnimationEventController.ForcedStopGatheringEvent();
	}

	private bool MousePowerClick()
	{
		return MouseClick(true);
	}

	private bool MouseClick(bool rightClick = false)
	{
		// Clicking UI element, ignore gameplay clicks
		if (Inputs.Instance.PointerOverUI){
			//Debug.Log("Click On UI dismiss	");
			return false;
		}

		IInteractable interactable = null;
		PlayerActionType playerActionType = PlayerActionType.Move;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

		//If clicking anywhere on navmesh


        //Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 1000f);
        Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, 1000f, GroundLayers);
        Vector3 clickPoint = hit.point;
        if (!NavMesh.SamplePosition(clickPoint, out NavMeshHit navhit, 1f, NavMesh.AllAreas))
        {
            // The clicked point is outside the NavMesh, Find the closest point on the NavMesh to the clicked point
            clickPoint = GetClosestClickPointWhenClickingOutside(clickPoint);
        }
        Vector3 aimPoint = CalculateAimPoint(ray);


		if (holdPosition) // Attack
		{			
			playerActionType = rightClick ? PlayerActionType.PowerAttack : PlayerActionType.Attack;
		}
		else if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit interactableHit, 1000f, Interactable))
        {
            //Debug.Log("Hit layer Interactable");
            // Get clicked Node if available
            if (interactableHit.collider.TryGetComponent(out interactable))
            {
				activeInteractable = interactable;
                Debug.Log("Hit object is Interactable");
				clickPoint = interactableHit.transform.position + (transform.position-interactableHit.transform.position).normalized* MinGatherDistance;
				FindObjectOfType<WayPointController>().PlaceWaypointBlob(clickPoint);
            }
			playerActionType = PlayerActionType.Interact;                
        }
		//Debug.Log("Click: "+ playerActionType +" Interactable: "+interactable+"ClickPoint: "+clickPoint+" hitpoint:"+hit.point);
        clickInfo = new ClickInfo(playerActionType, clickPoint, aimPoint, interactable);
        return true;

    }

    private Vector3 CalculateAimPoint(Ray ray)
    {
        Vector3 aim = playerAnimationEventController.ShootPoint;
        float lineScalarValue = (aim.y - ray.origin.y) / ray.direction.y;
        return ray.origin + ray.direction * lineScalarValue;
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
		if(interactCoroutine!=null && clickInfo.actionType != PlayerActionType.Gather) playerAnimationEventController.ForcedStopGatheringEvent();
        
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
			case PlayerActionType.Interact:
				interactCoroutine = StartCoroutine(InteractAt());
				break;
			case PlayerActionType.Stash:
				interactCoroutine = StartCoroutine(OpenStash());
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

	private IEnumerator OpenStash()
	{
        yield return null;
    }
	
	private IEnumerator InteractAt()
	{
		Debug.Log("Interact At "+ clickInfo.pos);
		//If to far move closer
		if(Vector2.Distance(transform.position, clickInfo.pos) > StopDistance) 
		{
			playerState.SetState(PlayerState.MoveToInteract);
			navMeshAgent.isStopped = false;
			navMeshAgent.SetDestination(clickInfo.pos);
		}
        // Try just checking navmashagents state
        while (navMeshAgent.remainingDistance > StopDistance)//  (transform.position - clickInfo.pos).magnitude > StopDistance)
        {
            distance = Vector2.Distance(transform.position, clickInfo.pos);

            yield return null;
        }

        /*
		while (Vector2.Distance(transform.position, clickInfo.pos) > StopDistance)//  (transform.position - clickInfo.pos).magnitude > StopDistance)
        {
			distance = Vector2.Distance(transform.position, clickInfo.pos);

            yield return null;		
		}*/

		transform.rotation = Quaternion.LookRotation(activeInteractable.gameObject.transform.position - transform.position, Vector3.up);

		Debug.Log("Now Iteract, at pos: "+transform.position);
		playerState.SetState(PlayerState.Interact);
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
        yield return new WaitForSeconds(SavingUtility.Instance.playerInventory.AttackTime / SavingUtility.Instance.playerInventory.AttackSpeedMultiplyer);

		attackLock = false;
		
		yield return null;
		LoadAction();
	}

    private void LoadAction()
	{
		if (savedAction != null)
		{
			clickInfo = savedAction;            
			Debug.Log("Click Info Set: "+clickInfo.interactable);
            activeInteractable = clickInfo.interactable;
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

	private void RightClick(InputAction.CallbackContext context)
	{
        if (context.phase == InputActionPhase.Started)
        {
			if(MousePowerClick())
				SaveOrExecutePlayerAction();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            ShowWaypointIfAvailable();
        }
    }
	private void LeftClick(InputAction.CallbackContext context)
	{
        if (context.phase == InputActionPhase.Started)
        {
			validClick = MouseClick();
			if(validClick) SaveOrExecutePlayerAction();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
			ShowWaypointIfAvailable();
        }
    }
	
	private void SaveOrExecutePlayerAction()
	{
        if (attackLock) SaveAction();
        else DoClickAction();
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

    public void InteractWithIteractable()
    {
		Debug.Log("activeInteractable: " + activeInteractable);
        activeInteractable?.Interract();
    }

    public void DeathAnimationComplete()
    {
		Debug.Log("Death animation complete");
        playerState.SetState(PlayerState.Dead);
        UIController.Instance.ActivateLevelClearPanel();
    }
	
    public void StopGathering()
    {
        if (interactCoroutine != null) StopCoroutine(interactCoroutine);
        interactCoroutine = null;
    }

    internal bool TakeHit(int damage)
    {
		if (SavingUtility.Instance.playerInventory.Health <= 0) return false;
		FindObjectOfType<HitInfoText>().CreateHitInfo(transform.position, damage, InfoTextType.Damage);

        Debug.Log("Player Take Hit: "+damage);        
        if(SavingUtility.Instance.playerInventory.Health>0) SavingUtility.Instance.playerInventory.Health -= damage;

        if (SavingUtility.Instance.playerInventory.Health <= 0)
		{
			Debug.Log("Player Died");
            playerState.SetState(PlayerState.Death);
			// Do not make player take inputs
			playerAnimationEventController.DisableAgent();
			clickInfo = null;
			attackLock = true;
            IsDead = true;
        }
		return true;
    }
}

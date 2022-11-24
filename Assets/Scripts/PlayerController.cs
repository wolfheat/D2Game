using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.XR;


public class SavedAction
{
    public bool attack = false;
    public Vector3 pos = Vector3.zero;
}
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerControls playerControls;
    [SerializeField] private Collider attackCollider;
    [SerializeField] private LayerMask GroundLayers;

    private SavedAction savedAction;
    private CameraRotateController cameraRotateController;
    private Camera mainCamera;
    private Coroutine moveCoroutine;
    private Coroutine stopTweenCoroutine;
    private float attackTime = 1.22f;
    private bool attackLock = false;

	// Charactercontroller is used which makes movement with collision, can be used instead of RB
	private CharacterController characterController;

    private Vector3 startPosition = new Vector3();
    private Quaternion currentRotation;
    private const float PlayerRotateTime = 0.15f;
    private Vector3 targetDestination = new Vector3();
    private Vector3 stepDestination = new Vector3();
    private Vector3 movement = new Vector3();
    private Vector3 lastClickPoint = new Vector3();
    private Vector3 startedAttackAgainst = new Vector3();
    private Vector3 attackForward = new Vector3();


	private const float WalkSpeed = 2.0f;
	private const float SprintSpeed = 5.335f;
    private float playerTargetSpeed = SprintSpeed;
    private float playerCurrentSpeed = 0f;
    private bool holdPosition = false;
    private bool grounded = true;
    private const float GroundedRadius = 0.15f;
    private const float GroundedOffset = 0f;
    private float motionSpeed = 1f;

	private bool hasAnimator;
	private Animator animator;

    public const float StartStopTime = 0.20f;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

	private void Update()
	{
		SetAnimationSpeed();
		GroundedCheck();


		if (playerControls.Land.Shoot.triggered)
		{
			Debug.Log("Shoot: inside update function");
		}
		if (stepDestination != null)
		{
			//Move();
		}

    }

	private void OnEnable()
    {
        cameraRotateController = GetComponent<CameraRotateController>();
        cameraRotateController.SetPlayerControls(playerControls);

        playerControls.Enable();
        playerControls.Land.LeftClick.performed += MouseClick;
        playerControls.Land.RightClick.performed += MouseRightClick;
        
        //playerControls.Land.Shift.performed += Shift;
        playerControls.Land.Shift.started += Shift;
        playerControls.Land.Shift.canceled += Shift;

        mainCamera = Camera.main;
		characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("Charactercontroller not found");
        }

		hasAnimator = TryGetComponent(out animator);
	}
    
	private void OnDisable()
    {
        playerControls.Disable();
        //mouseClickAction.Disable();
    }

	private void OnTriggerEnter(Collider collider)
	{
		Enemy enemy = collider.gameObject.GetComponent<Enemy>();
        
        enemy.Hit(25);

	}

	private void MouseClick(InputAction.CallbackContext context)
    {
        

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

		if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f, layerMask: GroundLayers) && hit.collider) {
            lastClickPoint = hit.point + new Vector3(0, GroundedOffset, 0);

            // If a valid point was clicked and we are in attackLock, store it.
            if (attackLock)
            {
                savedAction = new SavedAction();
                savedAction.attack = holdPosition;
                savedAction.pos = lastClickPoint;
                return;
            }

			if (holdPosition)
			{
				if (stopTweenCoroutine != null) StopCoroutine(stopTweenCoroutine);
                
                attackLock = true;
                playerCurrentSpeed = 0;
                StartCoroutine(FaceAttackDirectionThenAttack());
                
			}
			else
            {                    
				Debug.Log("Move to position " + lastClickPoint);
                if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                if (stopTweenCoroutine != null) StopCoroutine(stopTweenCoroutine);
                moveCoroutine = StartCoroutine(PlayerMoveTowards(lastClickPoint));

            }
        }
        else
        {
		    Debug.Log("Mouse click on no collider");
        }
    }

    private IEnumerator FaceAttackDirectionThenAttack()
    {
		// Tween Speed value down to zero and send to animation
		float startSpeed = playerCurrentSpeed;

		// Animation Tween points
		Vector3 startLookDirection = transform.forward;
		Vector3 endLookDirection = (lastClickPoint - transform.position).normalized;
        Vector3 currentLookVector;
        float timer = 0f;

		while (timer < PlayerRotateTime)
		{
			// Set Rotation
			currentLookVector = Vector3.Lerp(startLookDirection, endLookDirection, timer / PlayerRotateTime);
			transform.rotation = Quaternion.LookRotation(currentLookVector, Vector3.up);
            timer += Time.deltaTime;
			yield return null;
		}

        //Rotation done now Attack

		//InstantFacePosition(lastClickPoint);

		if (hasAnimator)
		{
			startedAttackAgainst = lastClickPoint;
			StartCoroutine(AttackTimer(attackTime));
		}
        yield return null;
	}
    private IEnumerator StopTween()
    {
		// Tween Speed value down to zero and send to animation
		float startSpeed = playerCurrentSpeed;

        // Animation Tween points
		Vector3 startLookDirection = transform.forward;
		Vector3 endLookDirection = (targetDestination - transform.position).normalized;
        Vector3 currentLookVector = startLookDirection;

		while (playerCurrentSpeed > 0f)
		{
			// Set Speed for animation
			playerCurrentSpeed -= (Time.deltaTime/StartStopTime)*startSpeed;
			animator.SetFloat("Speed", playerCurrentSpeed);

            // Set Rotation
            currentLookVector = Vector3.Lerp(startLookDirection, endLookDirection, (1-playerCurrentSpeed/startSpeed));
			transform.rotation = Quaternion.LookRotation(currentLookVector, Vector3.up);

			yield return null;
		}

        // After smoot rotation and animation is done break movement and start holdposition
        // holdPosition = hold;
		playerCurrentSpeed = 0f;
	}
    
    private IEnumerator AttackTimer(float t)
    {

		animator.SetBool("Attacking", true);

		yield return new WaitForSeconds(t/2);
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
		attackCollider.enabled = false;
		yield return new WaitForSeconds(t/2);
		animator.SetBool("Attacking", false);
        attackLock = false;

        ExecuteStoredAction();
	}

    private void ExecuteStoredAction()
    {
		if (savedAction != null)
        {
            lastClickPoint = savedAction.pos;

			if (savedAction.attack)
			{
				if (stopTweenCoroutine != null) StopCoroutine(stopTweenCoroutine);

				attackLock = true;
				playerCurrentSpeed = 0;
				StartCoroutine(FaceAttackDirectionThenAttack());

			}
			else
			{
				if (moveCoroutine != null) StopCoroutine(moveCoroutine);
				if (stopTweenCoroutine != null) StopCoroutine(stopTweenCoroutine);
				moveCoroutine = StartCoroutine(PlayerMoveTowards(lastClickPoint));

			}
            savedAction = null;
		}
	}

    private void InstantFacePosition(Vector3 target)
    {
        Debug.Log("Instantly face position: "+target);
		targetDestination = target;
        transform.rotation = Quaternion.LookRotation(target-transform.position);
	}

    private IEnumerator PlayerMoveTowards(Vector3 target)
    {
        // Set targetDestination to Final Destination
        targetDestination = target;
        startPosition = transform.position;
        Vector3 playerMoveVector = (targetDestination - transform.position).normalized;
        float timeElapsed = 0f;
        float percent;
        float totalMoveTime = (targetDestination-startPosition).magnitude/playerTargetSpeed;

        Vector3 lastPosition = transform.position;
        Vector3 nextPosition = lastPosition;

        Vector3 startLookDirection = transform.forward;
        Vector3 endLookDirection = (target-transform.position).normalized;
        
		Vector3 currentLookVector = startLookDirection;
		transform.rotation = Quaternion.LookRotation(currentLookVector, Vector3.up);

        Debug.Log("PPlayer speed at start "+playerCurrentSpeed);
        float totalSpeedChangeNeeded = playerTargetSpeed - playerCurrentSpeed;
        // Speed Up tween

		while (Vector3.Distance(transform.position, target) > 0.1f && !holdPosition)
        {
			timeElapsed += Time.deltaTime;
            //Rotation
            if (timeElapsed < StartStopTime)
            {
                // Set Speed for animation
                playerCurrentSpeed += (Time.deltaTime / StartStopTime) * totalSpeedChangeNeeded;
                Debug.Log("Player speed at t: "+timeElapsed+": "+playerCurrentSpeed);
                animator.SetFloat("Speed", playerCurrentSpeed);

                // Set Rotation
                currentLookVector = Vector3.Lerp(startLookDirection, endLookDirection, timeElapsed / StartStopTime);
                transform.rotation = Quaternion.LookRotation(currentLookVector, Vector3.up);
            }
            else
            {
				transform.rotation = Quaternion.LookRotation(endLookDirection, Vector3.up);
			}

            //Movement
            movement = playerMoveVector * playerCurrentSpeed*Time.deltaTime;
            lastPosition = nextPosition;
            nextPosition = transform.position + movement;
            characterController.Move(movement);
			yield return null;
        }
		stopTweenCoroutine = StartCoroutine(StopTween());        
	}

	private void GroundedCheck()
	{
		// set sphere position, with offset
		Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
			transform.position.z);
		grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
			QueryTriggerInteraction.Ignore);

		// update animator if using character
		if (hasAnimator)
		{
			animator.SetBool("Grounded", grounded);
		}
	}
	private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetDestination, 0.1f);
        if (attackLock)
        {
            //Gizmos.DrawCube(transform.position + transform.forward+Vector3.up, Vector3.one);
            Gizmos.DrawWireSphere(transform.position + transform.forward + Vector3.up, 1.5f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(attackForward, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startedAttackAgainst, 0.1f);
    }



    private void Shift(InputAction.CallbackContext context)
	{
        if (context.phase == InputActionPhase.Started)
        {
            //Debug.Log("Shift ON");
            holdPosition = true;
			if(moveCoroutine != null)StopCoroutine(moveCoroutine);
			if(stopTweenCoroutine != null)StopCoroutine(stopTweenCoroutine);
			stopTweenCoroutine = StartCoroutine(StopTween());            
		}
        else if (context.phase == InputActionPhase.Canceled) 
        {
            //Debug.Log("Shift OFF");
            holdPosition = false;           
        }
    }
    
	private void MouseRightClick(InputAction.CallbackContext context)
    {
        Debug.Log("Right clicked point");
    }


    private void SetAnimationSpeed()
    {
		if (hasAnimator)
		{
			animator.SetFloat("Speed", playerCurrentSpeed);
			animator.SetFloat("MotionSpeed", motionSpeed);
		}
	}

    private void Move()
    {
        if (playerControls.Land.Attack.triggered)
        {
            Debug.Log("Shift held, attack");
        }
	}
}

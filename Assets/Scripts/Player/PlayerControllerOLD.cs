using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SavedActionOLD
{
    public bool attack = false;
    public Vector3 pos = Vector3.zero;
}

public class PlayerControllerOLD : MonoBehaviour
{
}
/*
    [SerializeField] private Collider attackCollider;
    [SerializeField] private LayerMask GroundLayers;
    [SerializeField] private UIController UIController;
    [SerializeField] private GameObject rotationFaceObject;

// One step takes 0.48 seconds at playspeed 1f
// FootStepTime = 0.48f/motionSpeed;
private float footStepTimer = 0;
	private const float AnimationOneStepTime = 0.48f;
	private float lastTime = 0;
	private float thisTime = 0;
	[SerializeField] private SoundMaster soundmaster;


	private Camera mainCamera;
	private SavedAction savedAction;
	private SavedAction wayPointToShow;
    private Coroutine moveCoroutine;
    private Coroutine faceCoroutine;
    private Coroutine stopTweenCoroutine;
    private float attackTime = 1.22f;
    private float mouseClickTimer = 0f;
    private const float HoldMouseDuration = 0.2f;
    private const float GroundOffset = 0f;
    private bool attackLock = false;
    private bool attackDidHit = false;

	// Charactercontroller is used which makes movement with collision, can be used instead of RB
	private CharacterController characterController;

    private float playerSpeed = 0f;

    private Vector3 startPosition = new Vector3();
    private Quaternion currentRotation;
    private const float PlayerRotateTime = 0.15f;
    private Vector3 targetDestination = new Vector3();
    private Vector3 stepDestination = new Vector3();
    private Vector3 movement = new Vector3();
    private Vector3 storedLastPosition = new Vector3();
    private Vector3 lastClickPoint = new Vector3();
    private Vector3 startedAttackAgainst = new Vector3();
    private Vector3 attackForward = new Vector3();

	private const float WalkSpeed = 2.0f;
	private const float SprintSpeed = 5.335f;
    private float moveSpeedMax = SprintSpeed;
    private float MaxSpeedUpTime = 0.4f;
    private float animationMoveSpeed = 0f;
    private float playerAcceleration = 0.04f;

    private bool holdPosition = false;
    [SerializeField] private float motionSpeed = 1.3f;

	private bool hasAnimator;
	private Animator animator;

    public const float RotationTime = 0.20f;

	private void Update()
	{
        SetAnimationSpeed();
        GetPlayerInput();

    }

    private void GetPlayerInput()
    {
		mouseClickTimer += Time.deltaTime;
        if (Inputs.Instance.LClick == 1f && mouseClickTimer > HoldMouseDuration)
        {
            // Left button is held
            MouseClick();
		}
        else if (Inputs.Instance.Shift == 1 && Inputs.Instance.X == 1)
        {
            Debug.Log("Emergenzy QUIT Coroutines");
            StopCoroutine(moveCoroutine);
            StopCoroutine(stopTweenCoroutine);
		}
    }

    private void OnEnable()
	{
		mainCamera = Camera.main;
		characterController = GetComponent<CharacterController>(); if (characterController == null)Debug.LogError("Charactercontroller not found");
        hasAnimator = TryGetComponent(out animator);
		animator.SetFloat("MotionSpeed", 1f);

		Inputs.Instance.Controls.Land.LeftClick.performed += _ => MouseClick();
		Inputs.Instance.Controls.Land.LeftClick.canceled += _ => ShowWaypointIfAvailable();
		Inputs.Instance.Controls.Land.RightClick.performed += MouseRightClick;

		Inputs.Instance.Controls.Land.Shift.started += Shift;
		Inputs.Instance.Controls.Land.Shift.canceled += Shift;
	}
    
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
        if(hit.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Debug.Log("Controllercollider hit default ");
            //Stop Player
            StopPlayer();
            
        }
        if(hit.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Debug.Log("Controllercollider bump into enemiy ");
            //Stop Player
            StopPlayer();
            StepBack();

		}
	}

    private void StopPlayer()
    {
		StopActionCoroutines();
		playerSpeed = 0f;
        Debug.Log("Motionspeed set to "+motionSpeed);
        
	}

    private void OnTriggerEnter(Collider collider)
	{
		Enemy enemy = collider.gameObject.GetComponent<Enemy>();        
        enemy.Hit(25);
        attackDidHit = true;		
	}

	private void MouseClick()
    {
		// Used to limit input recognitions when mouse is held
		mouseClickTimer = 0; 

		Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

		if (Physics.Raycast(ray.origin, ray.direction, out hit, 1000f, layerMask: GroundLayers) && hit.collider) {
            lastClickPoint = hit.point + new Vector3(0, GroundOffset, 0);
            // If a valid point was clicked and we are in attackLock, store it.
            if (attackLock)
            {
                SaveAction();
                return;  
            }
			else if (holdPosition)
			{
                //Start Attack
				attackLock = true;
				StoreWaypointToShow(lastClickPoint,true);
				faceCoroutine = StartCoroutine(FaceDirectionAttack(true));                
			}
			else
            {
                StopActionCoroutines();
				StoreWaypointToShow(lastClickPoint);
				faceCoroutine = StartCoroutine(FaceDirectionAttack(false));
                
                Debug.Log("Clicking point: "+hit.point);
				moveCoroutine = StartCoroutine(PlayerMoveTowards(lastClickPoint));
            }
        }
        else Debug.Log("Mouse click on no collider");
    }

	private void LoadAction()
	{
		if (savedAction != null)
		{
			lastClickPoint = savedAction.pos;

			if (savedAction.attack)
			{
				attackLock = true;
				faceCoroutine = StartCoroutine(FaceDirectionAttack(true));
			}
			else
			{
				StopActionCoroutines();
				StartCoroutine(FaceDirectionAttack());
				faceCoroutine = StartCoroutine(FaceDirectionAttack(false));
				moveCoroutine = StartCoroutine(PlayerMoveTowards(lastClickPoint));

			}
			savedAction = null;
		}
	}

	private void SaveAction()
    {		
		savedAction = new SavedAction();
		savedAction.attack = holdPosition;
		savedAction.pos = lastClickPoint;
		StoreWaypointToShow(lastClickPoint, holdPosition);
	}

	private IEnumerator FaceDirectionAttack(bool doAttack=false)
    {
		//Debug.Log("Face Direction");
        rotationFaceObject.transform.position = lastClickPoint;

		// Tween Speed value down to zero and send to animation
		float startSpeed = animationMoveSpeed;

		// Animation Tween points
		Vector3 startLookDirection = transform.forward;
        Vector3 endLookDirection = (lastClickPoint - transform.position).normalized;
        
        Quaternion currentRotation = Quaternion.LookRotation(transform.forward,Vector3.up);
        Quaternion targetRotation = Quaternion.LookRotation(endLookDirection,Vector3.up);

        //Debug.Log("face direction attack rotate");

        float angleVelocity = 700f;        
        while (transform.rotation != targetRotation)
		{
			// Set Rotation
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angleVelocity*Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 4f) transform.rotation = targetRotation;
			yield return null;
		}
        //Debug.Log("face direction attack rotated");

        //Rotation done now Attack
		if (doAttack)
		{
		    //Debug.Log("Face Direction: attack");
			startedAttackAgainst = lastClickPoint;
			StartCoroutine(DoAttack(attackTime));
		}
        yield return null;
		//Debug.Log("Face Direction: Completed");
	}
        
    private IEnumerator DoAttack(float t)
    {
		animator.SetBool("Attacking", true);
		yield return new WaitForSeconds(t/3);
        attackDidHit = false;
		attackCollider.enabled = true;
		yield return new WaitForSeconds(0.05f);
		attackCollider.enabled = false;
        
        if (attackDidHit) soundmaster.PlaySFX(SoundMaster.SFX.SwingSword);
        else soundmaster.PlaySFX(SoundMaster.SFX.SwingSwordMiss);

		yield return new WaitForSeconds(t*2/3);
		animator.SetBool("Attacking", false);
        attackLock = false;

        LoadAction();
	}


    private void StopActionCoroutines()
    {
		//Start new Move
		if (faceCoroutine != null) StopCoroutine(faceCoroutine);
		if (moveCoroutine != null) StopCoroutine(moveCoroutine);
		if (stopTweenCoroutine != null) StopCoroutine(stopTweenCoroutine);
	}

    private void StepBack()
    {
        Debug.Log("STEP BACK");
        transform.position = storedLastPosition;
    }
		private IEnumerator PlayerMoveTowards(Vector3 target)
    {
		playerSpeed = moveSpeedMax;
		Vector3 playerMoveVector = (target - transform.position).normalized;
        Debug.Log("Player Move Towards: "+ target + " current position: "+transform.position+ "move vector is: "+playerMoveVector);

        float distance = Vector3.Distance(transform.position, target);
        float lastDistance = distance;
		// Keep Speed up
        
        // Character controllers move to is distorted when
        // colliding with enemies leading to player missing the target

		while (distance > 0.1f)
		//while (distance > (moveSpeedMax * Time.deltaTime))
		//while (Vector3.Distance(transform.position, target) > 0.1f && !holdPosition)
        {            
            //Movement
            movement = playerMoveVector * moveSpeedMax * Time.deltaTime;
            lastDistance = distance;
            distance = Vector3.Distance(transform.position, target);

			playerSpeed = moveSpeedMax;
            storedLastPosition = transform.position;
			characterController.Move(movement);            
            yield return null;
        }

        if (distance > lastDistance)
        {
            Debug.Log("End player move to: distance is greater than last stored distance");
            Debug.Log("Last distance: "+lastDistance+" distance: "+distance);
        }
        else Debug.Log("End player move to: To close to the target");
        //Debug.Log("Player Move Towards: Done");
        transform.position = target;
        playerSpeed = 0;
	}

	private void StoreWaypointToShow(Vector3 target, bool isAttack=false)
    {
        wayPointToShow = new SavedAction();
        wayPointToShow.attack = isAttack;
        wayPointToShow.pos = target;
	}
    private void ShowWaypointIfAvailable()
    {
        if(wayPointToShow != null)
        {
			UIController.ShowWaypoint(wayPointToShow.pos, wayPointToShow.attack);
        }
    }

		

    private void Shift(InputAction.CallbackContext context)
	{
        if (context.phase == InputActionPhase.Started)
        {
            holdPosition = true;
			if(moveCoroutine != null)StopCoroutine(moveCoroutine);
			if(faceCoroutine != null)StopCoroutine(faceCoroutine);
			if(stopTweenCoroutine != null)StopCoroutine(stopTweenCoroutine);
            playerSpeed = 0;
		}
        else if (context.phase == InputActionPhase.Canceled) 
        {
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
			//animator.SetFloat("Speed", animationMoveSpeed);
			animator.SetFloat("Speed", playerSpeed);
            //Debug.Log("Set player speed to: "+playerSpeed);
		}

		footStepTimer += Time.deltaTime;

        //CLUMPSY FOOTSTEP SOLUTION - Fix so it handles sounds better for variable speeds
        if (animationMoveSpeed > 2f)
        {
            if (footStepTimer >= AnimationOneStepTime / motionSpeed)
            {
                float duration = Time.time - lastTime;
                lastTime = Time.time;
                Debug.Log("Start new Step time: " + duration);
                footStepTimer = 0;
                soundmaster.PlaySFX(SoundMaster.SFX.Footstep);
            }
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
}

*/
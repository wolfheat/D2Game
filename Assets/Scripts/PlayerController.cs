using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SavedAction
{
    public bool attack = false;
    public Vector3 pos = Vector3.zero;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Collider attackCollider;
    [SerializeField] private LayerMask GroundLayers;
    [SerializeField] private UIController UIController;

	private Camera mainCamera;
	private SavedAction savedAction;
    private Coroutine moveCoroutine;
    private Coroutine stopTweenCoroutine;
    private float attackTime = 1.22f;
    private float mouseClickTimer = 0f;
    private const float HoldMouseDuration = 0.2f;
    private const float GroundOffset = 0f;
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
    private float moveSpeedMax = SprintSpeed;
    private float MaxSpeedUpTime = 0.4f;
    private float animationMoveSpeed = 0f;
    private float playerAcceleration = 0.04f;

    private bool holdPosition = false;
    private float motionSpeed = 1f;

	private bool hasAnimator;
	private Animator animator;

    public const float RotationTime = 0.20f;

	private void Update()
	{
        SetAnimationSpeed();

		mouseClickTimer += Time.deltaTime;
        if (Inputs.Instance.LClick == 1f && mouseClickTimer > HoldMouseDuration)
        {
            // Left button is held
            MouseClick();
		}
    }

	private void OnEnable()
	{
		mainCamera = Camera.main;
		characterController = GetComponent<CharacterController>(); if (characterController == null)Debug.LogError("Charactercontroller not found");
        hasAnimator = TryGetComponent(out animator);

		Inputs.Instance.Controls.Land.LeftClick.performed += _ => MouseClick();
		Inputs.Instance.Controls.Land.RightClick.performed += MouseRightClick;

		Inputs.Instance.Controls.Land.Shift.started += Shift;
		Inputs.Instance.Controls.Land.Shift.canceled += Shift;
	}
    
	private void OnTriggerEnter(Collider collider)
	{
		Enemy enemy = collider.gameObject.GetComponent<Enemy>();        
        enemy.Hit(25);
	}

	private void MouseClick()
    {
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
				if (stopTweenCoroutine != null) StopCoroutine(stopTweenCoroutine);                
                attackLock = true;
                animationMoveSpeed = 0;

                ShowWaypoint(lastClickPoint,true);
				StartCoroutine(FaceAttackDirectionThenAttack());                
			}
			else
            {
                //Debug.Log("FROM MOUSECLICK");
                //Start new Move
                if (moveCoroutine != null) StopCoroutine(moveCoroutine);
                if (stopTweenCoroutine != null) StopCoroutine(stopTweenCoroutine);

				ShowWaypoint(lastClickPoint);
				moveCoroutine = StartCoroutine(PlayerMoveTowards(lastClickPoint));
            }
        }
        else Debug.Log("Mouse click on no collider");
    }

    private void SaveAction()
    {		
		savedAction = new SavedAction();
		savedAction.attack = holdPosition;
		savedAction.pos = lastClickPoint;
		ShowWaypoint(lastClickPoint, holdPosition);
	}

	private IEnumerator FaceAttackDirectionThenAttack()
    {
		// Tween Speed value down to zero and send to animation
		float startSpeed = animationMoveSpeed;

		// Animation Tween points
		Vector3 startLookDirection = transform.forward;
		Vector3 endLookDirection = (lastClickPoint - transform.position).normalized;
        Vector3 currentLookVector;
        float timer = 0f;

		while (timer < PlayerRotateTime)
		{
			// Set Rotation
            timer += Time.deltaTime;
			currentLookVector = Vector3.Lerp(startLookDirection, endLookDirection, timer / PlayerRotateTime);
			transform.rotation = Quaternion.LookRotation(currentLookVector, Vector3.up);
			yield return null;
		}

        //Rotation done now Attack
		if (hasAnimator)
		{
			startedAttackAgainst = lastClickPoint;
			StartCoroutine(DoAttack(attackTime));
		}
        yield return null;
	}
    private IEnumerator StopTween()
    {
		// Tween Speed value down to zero and send to animation
		float startSpeed = animationMoveSpeed;

        // Animation Tween points
		Vector3 startLookDirection = transform.forward;
		Vector3 endLookDirection = (targetDestination - transform.position).normalized;
        Vector3 currentLookVector = startLookDirection;

		while (animationMoveSpeed > 0f)
		{
			// Set Speed for animation
			animationMoveSpeed -= (Time.deltaTime/RotationTime)*startSpeed;
			animator.SetFloat("Speed", animationMoveSpeed);

            // Set Rotation
            currentLookVector = Vector3.Lerp(startLookDirection, endLookDirection, (1-animationMoveSpeed/startSpeed));
			transform.rotation = Quaternion.LookRotation(currentLookVector, Vector3.up);

			yield return null;
		}

        // After smoot rotation and animation is done break movement and start holdposition
		animationMoveSpeed = 0f;
	}
    
    private IEnumerator DoAttack(float t)
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
				animationMoveSpeed = 0;
				StartCoroutine(FaceAttackDirectionThenAttack());

			}
			else
			{
				//Debug.Log("FROM STORED");
				if (moveCoroutine != null) StopCoroutine(moveCoroutine);
				if (stopTweenCoroutine != null) StopCoroutine(stopTweenCoroutine);
                moveCoroutine = StartCoroutine(PlayerMoveTowards(lastClickPoint));
				
			}
            savedAction = null;
		}
	}

    private void InstantFacePosition(Vector3 target)
    {
        targetDestination = target;
        transform.rotation = Quaternion.LookRotation(target-transform.position);
	}

    private IEnumerator PlayerMoveTowards(Vector3 target)
    {

        float timer = 0f;
        float speedChange = moveSpeedMax-animationMoveSpeed;
        float totalMoveTime = Vector3.Distance(transform.position, target) / moveSpeedMax;
		Vector3 startLookDirection = transform.forward;
		Vector3 endLookDirection = (target - transform.position).normalized;
		Vector3 currentLookVector = startLookDirection;
		Vector3 playerMoveVector = endLookDirection;
		while (Vector3.Distance(transform.position, target) > 0.1f && !holdPosition)
        {
            timer += Time.deltaTime;
            // No actual speed tween, but animator gets tween
            if (timer < MaxSpeedUpTime)
            {
                if (animationMoveSpeed < moveSpeedMax) animationMoveSpeed += playerAcceleration;
                else
                {
                    animationMoveSpeed = moveSpeedMax;
                }
            }
            else if (timer > totalMoveTime-MaxSpeedUpTime)
            {
                if(animationMoveSpeed > 0f) animationMoveSpeed -= playerAcceleration;
                if(animationMoveSpeed < 0f) animationMoveSpeed = 0f;
			}
            //else animationMoveSpeed = moveSpeedMax;
            
			animator.SetFloat("Speed", animationMoveSpeed);

            //Rotation
			if (timer < RotationTime)
			{
				// Set Rotation
				currentLookVector = Vector3.Lerp(startLookDirection, endLookDirection, timer / RotationTime);
				transform.rotation = Quaternion.LookRotation(currentLookVector, Vector3.up);
			}
			else
			{
				transform.rotation = Quaternion.LookRotation(endLookDirection, Vector3.up);
			}


            //Movement
                movement = playerMoveVector * moveSpeedMax * Time.deltaTime;
                //nextPosition = transform.position + movement;
                characterController.Move(movement);
                yield return null;

            //stopTweenCoroutine = StartCoroutine(StopTween());
        }

        //Ending Animation
        animationMoveSpeed = 0;
		animator.SetFloat("Speed", animationMoveSpeed);


	}

    private void ShowWaypoint(Vector3 target, bool isAttack=false)
    {
        
        UIController.ShowWaypoint(target, isAttack);

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
            holdPosition = true;
			if(moveCoroutine != null)StopCoroutine(moveCoroutine);
			if(stopTweenCoroutine != null)StopCoroutine(stopTweenCoroutine);
			stopTweenCoroutine = StartCoroutine(StopTween());            
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
			animator.SetFloat("Speed", animationMoveSpeed);
			animator.SetFloat("MotionSpeed", motionSpeed);
		}
	}

}

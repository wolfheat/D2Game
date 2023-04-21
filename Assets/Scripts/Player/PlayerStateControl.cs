using UnityEngine;

public enum PlayerState { Idle, MoveTo, MoveToGather, AttackSwordSwing, ShootArrow, Chase, Death, Dead, Decay, Gather }

public class PlayerStateControl : MonoBehaviour
{
	private Animator animator;
	private UIController UIController;
	private PlayerController PlayerController;
	public PlayerState State { get; private set;}

	private void Start()
	{
		UIController = FindObjectOfType<UIController>();
		PlayerController = FindObjectOfType<PlayerController>();
		animator = GetComponent<Animator>();

	}
	public void SetState(PlayerState newState)
	{
		//Set speed of animation
		animator.speed = PlayerController.attackSpeedMultiplier;

		switch (newState)
		{
			case PlayerState.Idle:
				animator.CrossFade("Idle", 0.1f);
				break;
			case PlayerState.MoveTo:
				animator.CrossFade("Run", 0.1f);
				break;
			case PlayerState.AttackSwordSwing:
				animator.CrossFade("SwordSwing", 0.1f);
				break;
			case PlayerState.MoveToGather:
				animator.CrossFade("Run", 0.1f);
				break;
			case PlayerState.ShootArrow:
				animator.CrossFade("ShootArrow", 0.1f);
				break;
			case PlayerState.Gather:
				animator.CrossFade("Gather", 0.1f);
				break;
			case PlayerState.Death:
				break;
			case PlayerState.Dead:
				break;
			case PlayerState.Decay:
				break;
			default:
				break;
		}

		State = newState;
		UIController.SetStateText("" + State);
	}

}

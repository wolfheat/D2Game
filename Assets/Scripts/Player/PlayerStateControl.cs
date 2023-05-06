using UnityEngine;

public enum PlayerState { Idle, MoveTo, MoveToInteract, AttackSwordSwing, ShootArrow, Chase, Death, Dead, Decay, Interact }

public class PlayerStateControl : MonoBehaviour
{
	private Animator animator;
	private UIController UIController;
	private PlayerController playerController;
	public PlayerState State { get; private set;}

	private void Start()
	{
		UIController = FindObjectOfType<UIController>();
		playerController = FindObjectOfType<PlayerController>();
		animator = GetComponent<Animator>();

	}
	public void SetState(PlayerState newState)
	{
		//Set speed of animation
		animator.speed = CharacterStats.AttackSpeedMultiplyer;

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
			case PlayerState.MoveToInteract:
				animator.CrossFade("Run", 0.1f);
				break;
			case PlayerState.ShootArrow:
				animator.CrossFade("ShootArrow", 0.1f);
				break;
			case PlayerState.Interact:
				switch (playerController.activeInteractable.Type)
				{
					case ResourceType.MiningNode:
				animator.CrossFade("Mining", 0.1f);
						return;
					case ResourceType.FishingNode:
				animator.CrossFade("FishingCast", 0.1f);
						return;
					case ResourceType.ScavengingNode:
				animator.CrossFade("Gather", 0.1f);
						return;
					case ResourceType.WoodcuttingNode:
				animator.CrossFade("Gather", 0.1f);
						return;
					case ResourceType.Stash:
				animator.CrossFade("OpenStash", 0.1f);
						return;
					default:
						break;
				}
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

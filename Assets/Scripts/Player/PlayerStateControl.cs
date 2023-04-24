using UnityEngine;

public enum PlayerState { Idle, MoveTo, MoveToGather, AttackSwordSwing, ShootArrow, Chase, Death, Dead, Decay, Gather }

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
			case PlayerState.MoveToGather:
				animator.CrossFade("Run", 0.1f);
				break;
			case PlayerState.ShootArrow:
				animator.CrossFade("ShootArrow", 0.1f);
				break;
			case PlayerState.Gather:
				switch (playerController.ActiveNode.Type)
				{
					case ResourceType.Mining:
				animator.CrossFade("Mining", 0.1f);
						return;
					case ResourceType.Fishing:
				animator.CrossFade("FishingCast", 0.1f);
						return;
					case ResourceType.Scavenging:
				animator.CrossFade("Gather", 0.1f);
						return;
					case ResourceType.Woodcutting:
				animator.CrossFade("Gather", 0.1f);
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

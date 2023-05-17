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
		Debug.Log("Setting new state: "+newState);
		//Set speed of animation
		animator.speed = SavingUtility.Instance.playerInventory.AttackSpeedMultiplyer;

		switch (newState)
		{
			case PlayerState.Idle:
				animator.CrossFade("Idle", 0.1f);
				break;
			case PlayerState.MoveTo:
				animator.CrossFade("Run", 0.1f);
				break;
			case PlayerState.AttackSwordSwing:
				if(Random.Range(0,10)>6)
					animator.CrossFade("SwordSwing", 0.3f);
				else
					animator.CrossFade("SwordSwing2", 0.3f);
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
                    case ResourceType.WellResourceNode:
                animator.CrossFade("Gather", 0.1f);
                        return;
                    default:
                        break;
				}
				break;
			case PlayerState.Death:
                animator.CrossFade("Death4", 0.1f);
                break;
			case PlayerState.Dead:
				break;
			case PlayerState.Decay:
				break;
			default:
				break;
		}

		State = newState;
	}

}

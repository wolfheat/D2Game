using UnityEngine;

public enum PlayerState { Idle, MoveTo, MoveToInteract, AttackSwordSwing, ShootArrow, Chase, Death, Dead, Decay, Interact }

public class PlayerStateControl : MonoBehaviour
{
	private Animator animator;
	private UIController UIController;
	private PlayerController playerController;
	private SoundMaster soundmaster;
	public PlayerState State { get; private set;}

	private void Start()
	{
		UIController = FindObjectOfType<UIController>();
		playerController = FindObjectOfType<PlayerController>();
		animator = GetComponent<Animator>();
        soundmaster = GetComponent<SoundMaster>();

	}
	public void SetState(PlayerState newState)
	{
		Debug.Log("Setting new state: "+newState + " from "+State);
		
		if (State == newState) return;

		//Set speed of animation
		animator.speed = SavingUtility.Instance.playerInventory.AttackSpeedMultiplyer;


		State = newState;

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
						playerController.ActivateTool(ToolType.PickAxe);
                        break;
					case ResourceType.FishingNode:
				animator.CrossFade("FishingCast", 0.1f);
						playerController.ActivateTool(ToolType.FishingRod);
                        break;
					case ResourceType.ScavengingNode:
				animator.CrossFade("Gather", 0.1f);
						playerController.ActivateTool(ToolType.Cultivator);
                        break;
					case ResourceType.WoodcuttingNode:
				animator.CrossFade("Woodcutting", 0.1f);
						playerController.ActivateTool(ToolType.Axe);
						break;
					case ResourceType.Stash:
				animator.CrossFade("OpenStash", 0.1f);
                        break;
                    case ResourceType.WellResourceNode:
                animator.CrossFade("Gather", 0.1f);
                        break;
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

	}

}

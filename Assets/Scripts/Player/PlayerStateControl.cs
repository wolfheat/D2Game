using UnityEngine;

public enum PlayerState { Idle, MoveTo, MoveToInteract, AttackSwordSwing, ShootArrow, Chase, Death, Dead, Decay, Interact, Cooking }

public class PlayerStateControl : MonoBehaviour
{
	private Animator animator;
	private UIController UIController;
	private PlayerController playerController;
	private SoundMaster soundmaster;
	public PlayerState State { get; private set;}

	private const float FadeTime = 0.3f;


    private void Start()
	{
		UIController = FindObjectOfType<UIController>();
		playerController = FindObjectOfType<PlayerController>();
		animator = GetComponent<Animator>();
        soundmaster = GetComponent<SoundMaster>();

	}
	public void SetState(PlayerState newState)
	{
		
		if (State == newState) return;

		Debug.Log("Setting new state: "+newState + " from "+State);
		//Set speed of animation
		animator.speed = SavingUtility.Instance.playerInventory.AttackSpeedMultiplyer;


		State = newState;

        switch (newState)
		{
			case PlayerState.Idle:
				animator.CrossFade("Idle", FadeTime);
				break;
			case PlayerState.MoveTo:
				animator.CrossFade("Run", FadeTime);
				break;
			case PlayerState.AttackSwordSwing:
				if(Random.Range(0,10)>6)
					animator.CrossFade("SwordSwing", 0.3f);
				else
					animator.CrossFade("SwordSwing2", 0.3f);
                break;
			case PlayerState.MoveToInteract:
				animator.CrossFade("Run", FadeTime);
				break;
			case PlayerState.ShootArrow:
				animator.CrossFade("ShootArrow", FadeTime);
				break;
			case PlayerState.Cooking:
				animator.CrossFade("Cooking", FadeTime);
				break;
			case PlayerState.Interact:
				switch (playerController.activeInteractable.Type)
				{
					case ResourceType.MiningNode:
				animator.CrossFade("Mining", FadeTime);
						playerController.ActivateTool(ToolType.PickAxe);
                        break;
					case ResourceType.FishingNode:
				animator.CrossFade("FishingCast", FadeTime);
						playerController.ActivateTool(ToolType.FishingRod);
                        break;
					case ResourceType.ScavengingNode:
				animator.CrossFade("Gather", FadeTime);
						playerController.ActivateTool(ToolType.Cultivator);
                        break;
					case ResourceType.WoodcuttingNode:
				animator.CrossFade("Woodcutting", FadeTime);
						playerController.ActivateTool(ToolType.Axe);
						break;
					case ResourceType.Stash:
				animator.CrossFade("OpenStash", FadeTime);
                        break;
                    case ResourceType.CookingStation:
				animator.CrossFade("OpenCooking", FadeTime);
                        break;
                    case ResourceType.WellResourceNode:
                animator.CrossFade("Gather", FadeTime);
                        break;
                    default:
                        break;
				}
				break;
			case PlayerState.Death:
                animator.CrossFade("Death4", FadeTime);
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

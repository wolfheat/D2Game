using UnityEngine;

public enum PlayerState { Idle, MoveTo, MoveToGather, AttackSwordSwing, BowLaunch, Chase, Death, Dead, Decay, Gather }

public class PlayerStateControl : MonoBehaviour
{
	private Animator animator;
	private UIController UIController;
	public PlayerState State { get; private set;}

	private void Start()
	{
		UIController = FindObjectOfType<UIController>();
		animator = GetComponent<Animator>();
	}
	public void SetState(PlayerState newState)
	{
		//Debug.Log("Setting state: From: "+playerState+" To: "+newState);
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

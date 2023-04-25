using System.Threading;
using UnityEngine;
using UnityEngine.AI;

//	public enum EnemyState { Idle, MoveTo, MoveToGather, AttackSwordSwing, BowLaunch, Chase, Death, Dead, Decay, Gather }
public enum EnemyState { Idle, Patrol, Attack, Chase, Death, Dead, Decay }

public class EnemyStateController : MonoBehaviour
{
	private Animator animator;
	private UIController UIController;
    public NavMeshAgent Agent { get; set; }
    private float stateTimer = 0;
	private const float IdleTime = 5f;
	private const float DECAY_TIME = 5f;
	public EnemyState State { get; private set; }	

	private void Awake()
	{
		UIController = FindObjectOfType<UIController>();
		animator = GetComponent<Animator>();
	}
	private void Update()
	{
		if (State == EnemyState.Dead || State == EnemyState.Decay)
		{
			DeathDecayUpdate();
		}
	}
	private void DeathDecayUpdate()
	{
		if (State == EnemyState.Dead)
		{
			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
			{
				SetState(EnemyState.Decay);
			}
		}else if (State == EnemyState.Decay)
		{
			stateTimer += Time.deltaTime;
			if (stateTimer > DECAY_TIME)
			{
				//Debug.Log("Decayed after "+ DECAY_TIME);
				Destroy(gameObject);
			}
		}
	}

	public void SetState(EnemyState newState)
	{
		//Debug.Log("Setting Enemy state to: "+newState);
		//Debug.Log("Setting Enemy state to: "+newState);
		if(Agent.isActiveAndEnabled) Agent.isStopped = false;
		switch (newState)
		{
			case EnemyState.Idle:
                Agent.isStopped = true;
				animator.CrossFade("Idle", 0.1f);
				stateTimer = 0;
				break;
			case EnemyState.Chase:
				animator.CrossFade("Run", 0.1f);
				break;
			case EnemyState.Patrol:
				animator.CrossFade("Walk", 0.1f);
				break;
			case EnemyState.Attack:
                Agent.isStopped = true;
                animator.CrossFade("AttackGoblinShootArrow", 0.1f);
				break;
			case EnemyState.Death:
				break;
			case EnemyState.Dead:
				int deathType = Random.Range(1,4);
				animator.CrossFade("Death"+deathType, 0.1f);
				break;
			case EnemyState.Decay:
				stateTimer = 0;
				break;
			default:
				break;
		}
		State = newState;
		UIController.SetState2Text("" + State);
	}

}

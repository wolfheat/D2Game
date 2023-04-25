using UnityEngine;

public class PlayerUnit : BaseUnit
{
    protected PlayerStateControl playerState;
	protected override void Awake()
	{
        playerState = GetComponent<PlayerStateControl>();
        base.Awake();
    }

    protected virtual void Update()
    {
        PlayerReachedTarget();
    }

    private void PlayerReachedTarget()
    {
        if (playerState.State == PlayerState.MoveTo && NavMeshAtTarget())
        {
            playerState.SetState(PlayerState.Idle);
            navMeshAgent.isStopped = true;
        }
    }



}

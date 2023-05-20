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
        //Debug.Log("Check if player reached target, state: "+ playerState.State);
        if (playerState.State == PlayerState.MoveTo && (NavMeshAtTarget() || !navMeshAgent.hasPath))
        {
            Debug.Log("Player reached Target, Set to Idle (haspath: "+ navMeshAgent.hasPath + ")");
            playerState.SetState(PlayerState.Idle);
            navMeshAgent.isStopped = true;
        }
    }



}

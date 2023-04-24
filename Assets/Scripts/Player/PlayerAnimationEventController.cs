using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

class PlayerAnimationEventController : MonoBehaviour
{
	[SerializeField] Collider attackCollider;

	PlayerController playerController;
	PlayerStateControl playerState;
	NavMeshAgent navMeshAgent;
	SoundMaster soundMaster;
	ProjectilesSpawner projectiles;

    [SerializeField] private GameObject shootPoint;
    public Vector3 ShootPoint { get { return shootPoint.transform.position; }}
    public Vector3 Aim { get; set; }

    public void Start()
	{
		playerController = GetComponent<PlayerController>();
		soundMaster = FindObjectOfType<SoundMaster>();
        projectiles = FindObjectOfType<ProjectilesSpawner>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		playerState = GetComponent<PlayerStateControl>();
	}
    public void ForcedStopGatheringEvent(bool completedEntireGathering = false)
    {
        Debug.Log("Forced Stop Gathering");

		playerController.StopGathering();
		if(completedEntireGathering) playerController.GatherNodeIfActive();
            
        navMeshAgent.destination = transform.position;
        // Generate Item

        playerState.SetState(PlayerState.Idle);
		soundMaster.StopSFX();
    }

    private void AnimationStopGatheringEvent()
	{
		ForcedStopGatheringEvent(true);
	}

	private void AnimationStepEvent()
	{
    //Debug.Log("Animation Step Event");
    soundMaster.StopStepSFX();
    soundMaster.PlayStepSFX();
	}

	private IEnumerator AnimationShootArrowEvent()
	{
		yield return new WaitForSeconds(0.05f);
		Debug.Log("Projectiles: "+projectiles);	
		Debug.Log("shootpoint: "+shootPoint);
		projectiles.ShootArrow(shootPoint.transform, Aim);

		

    // Play Sound
    soundMaster.PlaySFX(SoundMaster.SFX.ShootArrow);
	}
	
	private IEnumerator AnimationSwordSwingEvent()
	{
    // Runs at attack moment to check what enemies were hit
		AttackDidHit = false;
		attackCollider.enabled = true;
		yield return new WaitForSeconds(0.05f);
		attackCollider.enabled = false;

		// Play Sound
		if (AttackDidHit) soundMaster.PlaySFX(SoundMaster.SFX.SwordHit);
		else soundMaster.PlaySFX(SoundMaster.SFX.SwingSwordMiss);

	}

    public bool AttackDidHit { get; set; }    
}

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
		navMeshAgent = GetComponent<NavMeshAgent>();
		playerState = GetComponent<PlayerStateControl>();
        Debug.Log("PlayerAnimationEventController START");

		soundMaster = FindObjectOfType<SoundMaster>();
        projectiles = FindObjectOfType<ProjectilesSpawner>();
	}
    public void ForcedStopGatheringEvent(bool completedEntireGathering = false)
    {

        Debug.Log("Stop Gathering!");

		playerController.StopGathering();
		if(completedEntireGathering) playerController.InteractWithIteractable();
            
        navMeshAgent.destination = transform.position;

        playerState.SetState(PlayerState.MoveTo);
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

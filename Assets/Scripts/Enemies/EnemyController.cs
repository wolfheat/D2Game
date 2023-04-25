using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;
using Random = UnityEngine.Random;

public class Waypoint : MonoBehaviour
{
    public Vector3 pos;
}

//public enum State {Walk, Run, Attack}

public class EnemyController : Enemy
{
    public Vector3 Aim { get; private set; }
    [SerializeField] private GameObject shootPoint;
    private ProjectilesSpawner projectiles;		

	protected override void Awake()
	{
		projectiles = FindObjectOfType<ProjectilesSpawner>();
        navMeshAgent = GetComponent<NavMeshAgent>();
		enemyState = GetComponent<EnemyStateController>();
		enemyState.Agent = navMeshAgent;
		base.Awake();
	}
	private void Start()
    {
		enemyState.SetState(EnemyState.Idle);
	}

    private void FacePlayer()
    {
		transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
        Aim = player.transform.position+Vector3.up;

    }

	// --------ANIMATION EVENTS--------------------------------------------------------
	private void EnemyShootEvent()
	{
        projectiles.ShootArrow(shootPoint.transform, Aim);
	}

	private void EnemyShootAnimationStart()
	{
		FacePlayer();
	}
	
	private void EnemyShootAnimationEnd()
	{
		FacePlayer();
	}

}

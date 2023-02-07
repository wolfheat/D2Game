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

public class EnemyPatrol : MonoBehaviour
{
    private PlayerController player;
    private LayerMask playerLayermask;
	private Animator animator;
    [SerializeField] private List<WaypointMarker> wayPoints = new List<WaypointMarker>();
    [SerializeField] private GameObject shootPoint;
    private Projectiles projectiles;
    private ArrowHolder arrowHolder;

    private const float MIN_IDLE_TIME = 2f;
    private const float MAX_IDLE_TIME = 6f;
    private const float START_CHASE = 6f;
    private const float STOP_CHASE = 8f;

	private float waitTimer = 0;
    private int activeWayPoint = 0;
    private int storedWayPoint = 0;
    private bool dead = false;
    private NavMeshAgent navMeshAgent;
    private EnemyStateController enemyState;
    private float AttackDistance = 4f;

	bool playerVisable;
	float playerDistance;
	int lastPrintedState = 0;

	private void Awake()
	{
        player = FindObjectOfType<PlayerController>();
		projectiles = FindObjectOfType<Projectiles>();
		arrowHolder = FindObjectOfType<ArrowHolder>();		
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
		enemyState = GetComponent<EnemyStateController>();
		Debug.Log("Setting agent for enemy");
		enemyState.agent = navMeshAgent;

	}
	private void Start()
    {
		playerLayermask = LayerMask.NameToLayer("Player");
		enemyState.SetState(EnemyState.Idle);
	}


	public void SetWayPoints(List<WaypointMarker> list)
	{
		//Debug.Log("Enemy Waypoints set");
		wayPoints.Clear();
		wayPoints = list;
		//Debug.Log("Enemy Waypoints is set");
	}
	private void FixedUpdate()
	{
		playerDistance = GetPlayerDistance();
		playerVisable = PlayerVisable();
	}

	private void Update()
    {
		if(!dead)CheckForChangeOfAction();
		DoStateAction();    
	}

	private void CheckForChangeOfAction()
	{
		
		if (playerDistance <= AttackDistance)
		{
			//Debug.Log("playerdistanced to attack");
			if (enemyState.State == EnemyState.Attack && !playerVisable) {
				enemyState.SetState(EnemyState.Chase); 
				Debug.Log("Attacking player that is not visable go to CHASE"); 
			}
			else if (enemyState.State != EnemyState.Attack && playerVisable) {
				enemyState.SetState(EnemyState.Attack); 
				//Debug.Log("Close enough to attack and player is visable, start ATTACK"); 
			}
		}
		else if (playerDistance < START_CHASE)
		{
			//Debug.Log("playerdistanced to chase");
			if (enemyState.State != EnemyState.Chase && playerVisable) {
				enemyState.SetState(EnemyState.Chase); 
				//Debug.Log("Close enough to chase player but not chasing ("+enemyState+"), start CHASE"); 
			}
			else if (enemyState.State == EnemyState.Attack && playerVisable) {
				enemyState.SetState(EnemyState.Chase); 
				//Debug.Log("Attacking but to far away, switch to CHASE"); 
			}
			//else if (enemyState != EnemyState.Patrol || enemyState != EnemyState.Idle) { SetState(EnemyState.Patrol); Debug.Log("???"); }
		}
		else if (playerDistance > STOP_CHASE)
		{
			//Debug.Log("playerdistanced to STOP chase");
			if (enemyState.State == EnemyState.Attack || enemyState.State == EnemyState.Chase)
			{
				//Debug.Log("End Attack or Chase, player to far away");
				enemyState.SetState(EnemyState.Patrol);
				GetStoredPatrolWayPoint();
			}
		}
	}

	private void DoStateAction()
    {
		
		if (enemyState.State == EnemyState.Chase)
		{
			navMeshAgent.SetDestination(player.transform.position);
		}else if (enemyState.State == EnemyState.Idle)
		{
			//Debug.Log("Doing IDLE: "+waitTimer);
			waitTimer += Time.deltaTime;
			if (waitTimer > MAX_IDLE_TIME)
			{
				Debug.Log("before setting state to patrol");
				enemyState.SetState(EnemyState.Patrol);
				GetNextPatrolWayPoint();
				waitTimer = 0;
			}
		}
		else if (enemyState.State == EnemyState.Attack)
		{
			//FacePlayer();
		}
		else if (enemyState.State == EnemyState.Patrol)
		{
			if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
			{
				//Debug.Log("Setting state to Idle from Patrol, remaining distance is "+navMeshAgent.remainingDistance + " stopdistance is "+ navMeshAgent.stoppingDistance);
				
				enemyState.SetState(EnemyState.Idle);
			}
		}		
	}

    private bool PlayerVisable()
    {
        RaycastHit hit;

        // Ray towards player
        bool hitObject = Physics.Raycast(transform.position+Vector3.up, (player.transform.position - transform.position).normalized, out hit, AttackDistance);
        Debug.DrawRay(transform.position + Vector3.up, (player.transform.position - transform.position).normalized * AttackDistance , Color.yellow);

		if (hitObject)
        {
			if(hit.collider.gameObject.layer != lastPrintedState)
			{
				lastPrintedState = hit.collider.gameObject.layer;
			}
            if (hit.collider.gameObject.layer != playerLayermask.value)
			{
				Debug.Log("hitting layer " + hit.collider.gameObject.layer);
			}else return true;
        }
        else
        {
            //Debug.Log("Hit something else than player");
        }
        return false;
    }

    public void Killed()
    {
		dead = true;
		enemyState.SetState(EnemyState.Dead);
		navMeshAgent.enabled = false;
        //Remove Waypoints
        foreach(WaypointMarker wp in wayPoints)
        {
            Destroy(wp.gameObject);
        }
        wayPoints.Clear();

    }
    
    private float GetPlayerDistance()
    {
        return (player.transform.position - transform.position).magnitude;
    }

    private void FacePlayer()
    {
		transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);		
	}

	// --------ANIMATION EVENTS--------------------------------------------------------
	private void EnemyShootEvent()
	{
		Arrow newArrow = Instantiate(projectiles.arrowPrefab, arrowHolder.transform, false);
		newArrow.transform.position = shootPoint.transform.position;// transform.position;
		newArrow.transform.rotation = transform.rotation;
		newArrow.destructable = true;
	}

	private void EnemyShootAnimationStart()
	{
		FacePlayer();
	}
	
	private void EnemyShootAnimationEnd()
	{
		FacePlayer();
	}

	private void GetStoredPatrolWayPoint()
    {
		navMeshAgent.SetDestination(wayPoints[storedWayPoint].transform.position);
	}
    private void GetNextPatrolWayPoint()
    {
		activeWayPoint++;
		if (activeWayPoint >= wayPoints.Count) activeWayPoint = 0;
        //Debug.Log("ActiveWaypint: "+activeWayPoint);
		// Set next wayPoint, go to idle now
		navMeshAgent.SetDestination(wayPoints[activeWayPoint].transform.position);
		storedWayPoint = activeWayPoint;
		Debug.Log("Getting next patrol Point "+ wayPoints[activeWayPoint].transform.position+" at distance "
			+(transform.position- wayPoints[activeWayPoint].transform.position).magnitude);
	}

}

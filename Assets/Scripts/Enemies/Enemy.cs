using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : BaseUnit
{
    public List<WaypointMarker> WayPoints { get; set; }
    protected EnemyStateController enemyState;
    protected PlayerController player;
    private LayerMask playerLayermask;
    private Animator animator;


    private int health = 100;
    private float waitTimer = 0;

    private bool dead = false;
    private float AttackDistance = 4f;

    bool playerVisable;
    float playerDistance;
    int lastPrintedState = 0;

    private const float MIN_IDLE_TIME = 2f;
    private const float MAX_IDLE_TIME = 6f;
    private const float START_CHASE = 6f;
    private const float STOP_CHASE = 8f;

    private int activeWayPoint = 0;
    private int storedWayPoint = 0;


    protected override void Awake()
    {
        enemyState = GetComponent<EnemyStateController>();
        playerLayermask = LayerMask.NameToLayer("Player");
        player = FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
        base.Awake();
    }


    private void FixedUpdate()
    {
        playerDistance = GetPlayerDistance();
        playerVisable = PlayerVisable();
    }

    private void Update()
    {
        if (!dead) CheckForChangeOfAction();
        DoStateAction();
    }

    private float GetPlayerDistance()
    {
        return (player.transform.position - transform.position).magnitude;
    }

    private bool PlayerVisable()
    {
        RaycastHit hit;

        // Ray towards player
        bool hitObject = Physics.Raycast(transform.position + Vector3.up, (player.transform.position - transform.position).normalized, out hit, AttackDistance);
        Debug.DrawRay(transform.position + Vector3.up, (player.transform.position - transform.position).normalized * AttackDistance, Color.yellow);

        if (hitObject)
        {
            if (hit.collider.gameObject.layer != lastPrintedState)
            {
                lastPrintedState = hit.collider.gameObject.layer;
            }
            if (hit.collider.gameObject.layer != playerLayermask.value)
            {
                //Debug.Log("hitting layer " + hit.collider.gameObject.layer);
            }
            else return true;
        }
        else
        {
            //Debug.Log("Hit something else than player");
        }
        return false;
    }

    protected void DoStateAction()
    {

        if (enemyState.State == EnemyState.Chase)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        else if (enemyState.State == EnemyState.Idle)
        {
            //Debug.Log("Doing IDLE: "+waitTimer);
            waitTimer += Time.deltaTime;
            if (waitTimer > MAX_IDLE_TIME)
            {
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

    private void CheckForChangeOfAction()
    {

        if (playerDistance <= AttackDistance)
        {
            //Debug.Log("playerdistanced to attack");
            if (enemyState.State == EnemyState.Attack && !playerVisable)
            {
                enemyState.SetState(EnemyState.Chase);
            }
            else if (enemyState.State != EnemyState.Attack && playerVisable)
            {
                enemyState.SetState(EnemyState.Attack);
                //Debug.Log("Close enough to attack and player is visable, start ATTACK"); 
            }
        }
        else if (playerDistance < START_CHASE)
        {
            //Debug.Log("playerdistanced to chase");
            if (enemyState.State != EnemyState.Chase && playerVisable)
            {
                enemyState.SetState(EnemyState.Chase);
                //Debug.Log("Close enough to chase player but not chasing ("+enemyState+"), start CHASE"); 
            }
            else if (enemyState.State == EnemyState.Attack && playerVisable)
            {
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

    private void GetStoredPatrolWayPoint()
    {
        navMeshAgent.SetDestination(WayPoints[storedWayPoint].transform.position);
    }
    private void GetNextPatrolWayPoint()
    {
        if(WayPoints.Count == 0)
        {
            Debug.Log("No waypoints");
            return;
        }
        activeWayPoint++;
        if (activeWayPoint >= WayPoints.Count) activeWayPoint = 0;
        //Debug.Log("ActiveWaypint: "+activeWayPoint);
        // Set next wayPoint, go to idle now
        navMeshAgent.SetDestination(WayPoints[activeWayPoint].transform.position);
        storedWayPoint = activeWayPoint;
        //Debug.Log("Getting next patrol Point " + WayPoints[activeWayPoint].transform.position + " at distance " + (transform.position - WayPoints[activeWayPoint].transform.position).magnitude);
    }


    public void Hit(int dmg)
    {
        //Debug.Log("Enemy hit by "+dmg+" damage");
        health -= dmg;

        if (health <= 0) Die();
    }

    private void Die()
    {
        dead = true;
        enemyState.SetState(EnemyState.Dead);
        navMeshAgent.enabled = false;
        //Remove Waypoints
        foreach (WaypointMarker wp in WayPoints)
        {
            Destroy(wp.gameObject);
        }
        Debug.Log("Clearing Waypoints at die");
        WayPoints.Clear();

        DisableColliders();     
	}

    private void DisableNavMesh()
    {
		// disable navmeshagent
		NavMeshAgent navMesh = FindObjectOfType<NavMeshAgent>();
		navMesh.enabled = false;
	}

	private void DisableColliders()
    {
		Collider[] colliders = GetComponents<Collider>();
		foreach (Collider collider in colliders)
		{
			collider.enabled = false;
			//Debug.Log("Enemy collider removed");
		}

	}
}

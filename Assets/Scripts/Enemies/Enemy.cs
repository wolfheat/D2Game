using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject miniPrefab;
    //private EnemyPatrol enemyPatrol;
    public EnemyPatrol enemyPatrol { get; set; }
    private int health = 100;

	private void Awake()
    {
        enemyPatrol = GetComponent<EnemyPatrol>();
    }

    public void Hit(int dmg)
    {
        //Debug.Log("Enemy hit by "+dmg+" damage");
        health -= dmg;

        if (health <= 0) Die();
    }

    private void Die()
    {
        //Instantiate(miniPrefab,transform.position,Quaternion.identity);
        //Debug.Log("Enemy Dies");
        //Destroy(transform.gameObject);
        enemyPatrol.Killed();
        //unable collider
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

using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject miniPrefab;
    private int health = 100;

	private void Start()
    {
        
    }

	private void Update()
    {
        
    }
    
	public void Hit(int dmg)
    {
        Debug.Log("Enemy hit by "+dmg+" damage");
        health -= dmg;

        if (health <= 0) Die();
    }

    private void Die()
    {
        Instantiate(miniPrefab,transform.position,Quaternion.identity);
        Debug.Log("Enemy Dies");
        Destroy(transform.gameObject);
    }
}

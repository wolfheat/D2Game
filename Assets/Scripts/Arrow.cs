using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Arrow : MonoBehaviour
{
    private float velocity = 40f;
    private float life = 3f;
    public int DMG { get; set; } = 50;
    public bool destructable = false;
    Rigidbody rb;


	private void Awake()
    {
        rb = GetComponent<Rigidbody>();
	}

    public void RandomizeDirection()
    {
		float lambda = 0f;
		//float lambda = 3f;
		Quaternion randomRotation = transform.rotation * Quaternion.Euler(Random.Range(-lambda, lambda), Random.Range(-lambda, lambda), 0);
		transform.rotation = randomRotation;
        rb.transform.rotation = randomRotation;
        rb.AddForce(transform.forward * velocity, ForceMode.VelocityChange);       
        
    }



	private void OnCollisionEnter(Collision collider)
    {
        if(collider.transform.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Hit(DMG);
            transform.parent = enemy.transform;
        }
        else if(collider.transform.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.TakeHit(DMG);
            transform.parent = player.transform;
        }        
        StopArrow();
    }

    private void StopArrow()
    {
        velocity = 0;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.useGravity = false;
        Collider col = gameObject.GetComponent<Collider>();
        col.enabled = false;
    }


    private void Update()
    {
        if (!destructable) return;
        //transform.position += transform.forward * velocity * Time.deltaTime;
        life -= Time.deltaTime;
        if (life <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void Destroy()
    {
        if(destructable) Die();
    }

    internal void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
}

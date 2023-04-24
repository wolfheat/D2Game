using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Arrow : MonoBehaviour
{
    private float velocity = 40f;
    private float life = 3f;
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
        //Debug.Log("Arrow hit trigger enter");
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
}

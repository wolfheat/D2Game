using UnityEngine;
using UnityEngine.AI;

public class BaseUnit : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;


    protected bool NavMeshAtTarget()
	{
		return navMeshAgent.remainingDistance < 0.1f;
    }
	
    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Debug.Log("Awake Unit navmesh");
    }

    public void SetToPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void EnableNavMesh(bool enable)
    {
        if (navMeshAgent) navMeshAgent.enabled = enable;
        else Debug.LogError("No Nav Mesh available");
    }
    protected virtual void OnTriggerEnter(Collider collider){}

}

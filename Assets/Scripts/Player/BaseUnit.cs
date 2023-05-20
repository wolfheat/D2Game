using UnityEngine;
using UnityEngine.AI;

public class BaseUnit : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;


    protected bool NavMeshAtTarget()
	{
        //if (navMeshAgent.remainingDistance > 0.1f) Debug.Log("NavMesh Remaining Distance: "+ navMeshAgent.remainingDistance);
        if(!navMeshAgent.enabled) return true;
		return navMeshAgent.remainingDistance < 0.1f;
    }
	
    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetToPosition(Vector3 pos)
    {
        Debug.Log("Setting Unit to positon: "+pos);
        if(navMeshAgent != null) navMeshAgent.Warp(pos);
        else transform.position = pos;
    }

    public void EnableNavMesh(bool enable)
    {
        if (navMeshAgent) navMeshAgent.enabled = enable;
        else Debug.LogError("No Nav Mesh available");
    }
    protected virtual void OnTriggerEnter(Collider collider){}

}

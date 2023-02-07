using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    [SerializeField] private Transform moveToPosition;
    private NavMeshAgent navMeshAgent;
	[SerializeField] private Animator animator;


	private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();    
    }

	private void Update()
    {
        navMeshAgent.destination = moveToPosition.position;
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        if(navMeshAgent.velocity.magnitude < 0.02f)
        {
            animator.SetBool("Idle", true);
        }
        else
        {
            animator.SetBool("Idle", false);
        }
    }

}

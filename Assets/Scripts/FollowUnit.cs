using UnityEngine;

public class FollowUnit : MonoBehaviour
{
    [SerializeField] GameObject unitToFollow;
    [SerializeField] float heightOffset = 0; 
	
	private void Start()
    {
        unitToFollow = FindObjectOfType<PlayerController>().gameObject;
    }
    
	private void Update()
    {
        transform.position = unitToFollow.transform.position + Vector3.up*heightOffset;
    }

}

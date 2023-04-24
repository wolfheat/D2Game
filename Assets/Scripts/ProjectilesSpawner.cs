using UnityEngine;

public class ProjectilesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject arrowHolder;
    public Arrow arrowPrefab;

    public void ShootArrow(Transform shootPoint, Vector3 aim)
    {
        // Shoot the arrow
        Arrow newArrow = Instantiate(arrowPrefab, arrowHolder.transform, false);
        newArrow.gameObject.layer = shootPoint.gameObject.layer;
        newArrow.transform.position = shootPoint.transform.position;// transform.position;
        newArrow.transform.rotation = Quaternion.LookRotation(aim-shootPoint.position,Vector3.up);
        newArrow.destructable = true;
        newArrow.RandomizeDirection();
    }
    
}

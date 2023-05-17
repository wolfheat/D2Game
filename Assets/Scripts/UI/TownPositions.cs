using UnityEngine;

public class TownPositions : MonoBehaviour
{
    [SerializeField] GameObject[] positions;
	
	public Vector3 GetClosestPosition(Vector3 pos)
    {
		Vector3 closest = Vector3.zero;
		float distance = 1000f;

		for (int i = 0; i < positions.Length; i++)
		{
			float newDistance = Vector3.Distance(pos, positions[i].transform.position);

            if (newDistance < distance){
				distance = newDistance;
				closest = positions[i].transform.position;
			}
		}

		if(closest == Vector3.zero) return pos;
		else return closest;
    }

}

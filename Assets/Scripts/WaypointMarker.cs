using UnityEngine;

public class WaypointMarker : MonoBehaviour
{
	public void ToggleVisability(bool visable)
	{
		transform.gameObject.GetComponent<MeshRenderer>().enabled = visable;
	}
}

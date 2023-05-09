using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WayPointController : MonoBehaviour
{
    
    [SerializeField] private List<GameObject> wayPoints;
    [SerializeField] private GameObject blobPrefab;
    private GameObject blob;
    private GameObject attackWayPoint;
    private ParticleSystem attackWayPointPS;
	private GameObject powerAttackWayPoint;
	private ParticleSystem powerAttackWayPointPS;
    private GameObject wayPoint;
    private ParticleSystem wayPointPS;
    private int activeWayPointID=0;
    private bool waypointVisable;
    
    private void Awake()
    {        
        Inputs.Instance.Controls.Land.WayPointType.performed += ChangeWayPoint;
        Toggle(FindObjectOfType<UIController>().toggle);
    }
    
    private void OnDestroy()
    {        
        Inputs.Instance.Controls.Land.WayPointType.performed -= ChangeWayPoint;
        //Toggle(FindObjectOfType<UIController>().toggle);
    }

    public void Toggle(Toggle toggle)
    {
		waypointVisable = toggle.isOn;
		UpdateWaypointMarkers();
	}
	public void PlaceWaypointBlob(Vector3 p)
	{
        //Debug.Log("Placing Blob");
        if(blob == null) blob = Instantiate(blobPrefab,p,Quaternion.identity);
        else blob.transform.position = p;
	}
    
	public void UpdateWaypointMarkers()
	{
        // Used for Enemy waypoints
        WaypointMarker[] wayPointMarkers = FindObjectsOfType<WaypointMarker>();
        foreach (WaypointMarker marker in wayPointMarkers)
        {
            marker.ToggleVisability(waypointVisable);
        }
	}

	private void OnEnable()
    {
        wayPoint = wayPoints[0];
        wayPointPS = wayPoints[0].GetComponent<ParticleSystem>();
        attackWayPoint = wayPoints[1];
        attackWayPointPS = wayPoints[1].GetComponent<ParticleSystem>();
        powerAttackWayPoint = wayPoints[2];
		powerAttackWayPointPS = wayPoints[2].GetComponent<ParticleSystem>();

	}
    private void ChangeWayPoint(InputAction.CallbackContext context)
    {
        Debug.Log("Change TYPE");
        activeWayPointID++;
        if(activeWayPointID>=wayPoints.Count) activeWayPointID = 0;
        wayPoint = wayPoints[activeWayPointID];
		wayPointPS = wayPoints[activeWayPointID].GetComponent<ParticleSystem>();
	}


	public void ShowWaypoint(ClickInfo savedAction)
    {
		switch (savedAction.actionType)
        {
            case PlayerActionType.Attack:
                attackWayPoint.transform.position = savedAction.pos;
                attackWayPointPS.Play();
                break;
            case PlayerActionType.PowerAttack:
                Debug.Log("Doing power attack marker");
				powerAttackWayPoint.transform.position = savedAction.pos;
                powerAttackWayPointPS.Play();
                break;
            case PlayerActionType.Move:
                wayPoint.transform.position = savedAction.pos;
				wayPointPS.Play();
                break;
            case PlayerActionType.Gather:
				wayPoint.transform.position = savedAction.pos;
				wayPointPS.Play();
				break;
            default:
                break;
        }
	}
}

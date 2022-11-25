using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    
    [SerializeField] private List<GameObject> wayPoints;
    private GameObject attackWayPoint;
    private ParticleSystem attackWayPointPS;
    private GameObject wayPoint;
    private ParticleSystem wayPointPS;
    private int activeWayPointID=0;
    private void OnEnable()
    {
        wayPoint = wayPoints[0];
        wayPointPS = wayPoints[0].GetComponent<ParticleSystem>();
        attackWayPoint = wayPoints[1];
        attackWayPointPS = wayPoints[1].GetComponent<ParticleSystem>();
	}
    private void Start()
    {
		Inputs.Instance.Controls.Land.WayPointType.performed += _ => ChangeWayPoint();
        
    }
    private void ChangeWayPoint()
    {
        Debug.Log("Change TYPE");
        activeWayPointID++;
        if(activeWayPointID>=wayPoints.Count) activeWayPointID = 0;
        wayPoint = wayPoints[activeWayPointID];
		wayPointPS = wayPoints[activeWayPointID].GetComponent<ParticleSystem>();
	}

	public void ShowWaypoint(Vector3 target, bool isAttack=false)
    {
        if (isAttack)
        {
            attackWayPoint.transform.position = target;
            attackWayPointPS.Play();
        }
        else
        {
            wayPoint.transform.position = target;
            wayPointPS.Play();
        }
	}
}

using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] WaypointMarker waypointPrefab;
    [SerializeField] GameObject waypointHolder;
    [SerializeField] GameObject enemyHolder;
	private WayPointController wayPointController;

    private const int MinWayPoints = 2;
    private const int MaxWayPoints = 3;

    private const float MinSpawnX = 1;
    private const float MaxSpawnZ = 1;
    private const float MaxSpawnX = 8;
    private const float MinSpawnZ = 19;
    
    private Vector2Int specialSpawnPoint = new Vector2Int(2,12);
           

	private void Start()
    {
        wayPointController = FindObjectOfType<WayPointController>();
		Inputs.Instance.Controls.Land.Space.performed += _ => SpawnNewEnemyRandomWaypoints();
        
	}

    private void SpawnNewEnemyRandomWaypoints()
    {
        int waypoints = Random.Range(MinWayPoints,MaxWayPoints+1);
        //Debug.Log("Creating "+waypoints+" Waypoints for the Enemy");
		List<WaypointMarker> waypointList = new List<WaypointMarker>();
		
        Enemy newEnemy = Instantiate(enemyPrefab, enemyHolder.transform);
		newEnemy.transform.position = new Vector3(Random.Range(MinSpawnX, MaxSpawnX), 0f, Random.Range(MinSpawnZ, MinSpawnZ));

		for (int i = 0; i < waypoints; i++)
        {
			WaypointMarker wp = Instantiate(waypointPrefab, waypointHolder.transform);
			wp.transform.position = i==0?newEnemy.transform.position:new Vector3(Random.Range(MinSpawnX, MaxSpawnX), 0f, Random.Range(MinSpawnZ, MinSpawnZ));

            waypointList.Add(wp);
		}
		newEnemy.WayPoints = waypointList;
		wayPointController.UpdateWaypointMarkers();
        Debug.Log("Spawning Enemy at: "+newEnemy.transform.position);
	}

    private void SpawnNewEnemy()
    {

		Vector3 spawnPosition = new Vector3(Random.Range(MinSpawnX,MaxSpawnX), 0f, Random.Range(MinSpawnZ, MinSpawnZ));   
        Vector3 firstWaypointPosition = new Vector3(Random.Range(MinSpawnX,MaxSpawnX), 0f, Random.Range(MinSpawnZ, MinSpawnZ));   


        Debug.Log("Spawning Enemy by SpawnNewEnemy.");
        if (Inputs.Instance.Shift == 1)
        {
            Debug.Log("With Shift pressed");
            spawnPosition = new Vector3 (specialSpawnPoint.x*LevelCreator.Tilesize,0, specialSpawnPoint.y * LevelCreator.Tilesize);
        }
        else
        {
            Debug.Log("With Shift NOT pressed");

        }

        //Debug.Log("Spawns New enemy at: "+spawnPosition);
        //Debug.Log("Second waypoint at: "+firstWaypointPosition);
        Enemy newEnemy = Instantiate(enemyPrefab, enemyHolder.transform);
		newEnemy.transform.position = spawnPosition;
        //Debug.Log("Enemy Created: ");

		WaypointMarker wp1 = Instantiate(waypointPrefab, enemyHolder.transform);
        wp1.transform.position = spawnPosition;
		WaypointMarker wp2 = Instantiate(waypointPrefab, enemyHolder.transform);
        wp2.transform.position = firstWaypointPosition;
        //Debug.Log("Waypoints Created: ");
        
        List<WaypointMarker> waypointList = new List<WaypointMarker>() { wp1, wp2};
        newEnemy.WayPoints = waypointList;

        wayPointController.UpdateWaypointMarkers();
	}
}

using System;
using UnityEngine;
using UnityEngine.AI;

public class SetPlayerStartPosition : MonoBehaviour
{
    [SerializeField] PlayerSettingsSO playerSettingsData;
    NavMeshAgent agent;

	public void SetPlayerDataPostionFromFile(Vector3 pos)
    {
        Debug.Log("Loading File Position InTo Data Position");  
        //playerSettingsData.SetTownPosition(pos);
    }
	public void StorePlayerPosition()
    {
        if (agent == null) GetAgent();
        Debug.Log("Request current player position to be stored in data position: "+agent.transform.position);
        //playerSettingsData.SetTownPosition(agent.transform.position);
        FindObjectOfType<SavingUtility>().SetPlayerTownPosition();
    }

    private void GetAgent()
    {
        agent = FindObjectOfType<PlayerController>().GetComponent<NavMeshAgent>();
    }

    public void PlacePlayerOnStoredPosition()
    {
        Debug.Log("Physically warping player to stored data position");
        if (agent == null) GetAgent();

        agent.Warp(playerSettingsData.TownPosition);
    }
}

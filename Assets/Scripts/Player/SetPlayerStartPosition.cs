using UnityEngine;
using UnityEngine.AI;

public class SetPlayerStartPosition : MonoBehaviour
{
    [SerializeField] PlayerSettingsSO playerSettingsData;
    NavMeshAgent agent;

	public void StorePlayerPosition()
    {
        playerSettingsData.SetTownPosition(agent.transform.position);
    }
	public void PlacePlayerOnStoredPosition()
    {
        Debug.Log("SetPlayerStartPosition Start");

        agent = FindObjectOfType<PlayerController>().GetComponent<NavMeshAgent>();

        agent.Warp(playerSettingsData.TownPosition);
    }
}

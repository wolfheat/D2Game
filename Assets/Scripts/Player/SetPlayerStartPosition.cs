using UnityEngine;
using UnityEngine.AI;

public class SetPlayerStartPosition : MonoBehaviour
{
    [SerializeField] PlayerSettingsSO playerSettingsData;
    NavMeshAgent agent;


	private void Start()
    {
        Debug.Log("SetPlayerStartPosition Start");

        agent = GetComponent<NavMeshAgent>();
        Debug.Log("Setting Player position to Townposition: " + playerSettingsData.TownPosition);
        //GetComponent<Rigidbody>().position = playerSettingsData.TownPosition;
        //agent.transform.position = playerSettingsData.TownPosition;        
        agent.Warp(playerSettingsData.TownPosition);
    }
}

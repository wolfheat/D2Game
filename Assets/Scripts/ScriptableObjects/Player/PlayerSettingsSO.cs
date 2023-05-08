using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettingsData_", menuName = "PlayerSetting")]
public class PlayerSettingsSO : ScriptableObject
{
    public Vector3 TownPosition;

    public enum TownSpots {TownEntrance, PortalEntrance}

    [SerializeField] public Vector3[] TownSpotsPositions;

    public void SetTownPosition(Vector3 pos)
    {
        TownPosition = pos;
    }
    
    public void UpdateTownPositions(Vector3[] positions)
    {
        TownSpotsPositions = positions;
    }

    internal void EditorSet()
    {
        TownPosition = new Vector3(TownPosition.x,0,TownPosition.z);
    }

    internal void SetToClosest(Vector3 pos)
    {
        Debug.Log("Setting To closest");
        float distance = 1000f;
        int bestIndex = 0;
        for (int i = 0; i < TownSpotsPositions.Length; i++)
        {
            float newDistance = Vector3.Distance(pos, TownSpotsPositions[i]);
            if (newDistance < distance)
            {
                bestIndex = i;
                distance = newDistance;
            }
        }
        TownPosition = TownSpotsPositions[bestIndex];
    }
}

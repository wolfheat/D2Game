using System.Linq;
using UnityEngine;

public class TownPositionsController : MonoBehaviour
{
    [SerializeField] PlayerSettingsSO playerSettingsSO;

    [SerializeField] GameObject[] TownPositionObjects;

    public void Populate()
    {
        Vector3[] positions = TownPositionObjects.Select(go => go.transform.position).ToArray();
        playerSettingsSO.UpdateTownPositions(positions);        
    }
    public void ChangeToClosestPoint(Vector3 pos)
    {
        playerSettingsSO.SetToClosest(pos);
    }
}

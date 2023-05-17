using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class TownPositionsController : MonoBehaviour
{

    [SerializeReference][SerializeField] PlayerSettingsSO playerSettingsSO;

    [SerializeField] GameObject[] TownPositionObjects;

    public void Populate()
    {
        Vector3[] positions = TownPositionObjects.Select(go => go.transform.position).ToArray();
        ChangeToClosestPoint(FindObjectOfType<PlayerController>().transform.position);
        playerSettingsSO.UpdateTownPositions(positions);
        playerSettingsSO.EditorSet();
        UpdateScriptableObject();

    }

    private void UpdateScriptableObject()
    {
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        //Debug.Log("Scriptable Object created and saved: ");
    }

    public void ChangeToClosestPoint(Vector3 pos)
    {
        Debug.Log("Changing Stored Point");
        playerSettingsSO.SetToClosest(pos);
        UpdateScriptableObject();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("On Aplication quit set player position");
        /*if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TownScene")
        {
            ChangeToClosestPoint(FindObjectOfType<PlayerController>().transform.position);            
        }
        else Debug.Log("Not In Town, Player position not stored");
        */
    }
}



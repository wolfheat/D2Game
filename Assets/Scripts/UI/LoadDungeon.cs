using TMPro;
using UnityEngine;

public class LoadDungeon : MonoBehaviour
{
    [SerializeField] GameObject loadDungeonPanel;

    public void Start()
    {
        //Inputs.Instance.Controls.Land.ESC.started += _ => HideMenu(loadDungeonPanel.activeSelf);
        HideMenu(true);
    }
    public void HideMenu(bool action = true)
    {
        loadDungeonPanel.SetActive(!action);
    }

    public void StartDungeon()
    {
        Debug.Log("StartDungeon");
    }


}

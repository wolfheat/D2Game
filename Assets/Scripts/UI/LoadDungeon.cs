using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadDungeon : MonoBehaviour
{
    [SerializeField] GameObject loadDungeonPanel;
    TownPositionsController townPositionsController;

    public void Start()
    {
        townPositionsController = FindObjectOfType<TownPositionsController>();
        //Inputs.Instance.Controls.Land.ESC.started += _ => HideMenu(loadDungeonPanel.activeSelf);
        HideMenu(true);
    }
    public void HideMenu(bool action = true)
    {
        Debug.Log("ShowMenu From: "+GetInstanceID());
        loadDungeonPanel.SetActive(!action);
    }

    public void StartDungeon()
    {
        Debug.Log("StartDungeon");
        HideMenu(true);

        townPositionsController.ChangeToClosestPoint(FindObjectOfType<PlayerController>().transform.position);

        SceneManager.LoadScene("MainScene");
    }

}

using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoadDungeon : MonoBehaviour, IOpenCloseMenu
{
    [SerializeField] GameObject panel;
    TownPositionsController townPositionsController;

    public bool PanelEnabled => panel.activeSelf;
    public void ShowPanel() => panel.SetActive(true);
    public void ClosePanel() => panel.SetActive(false);

    public void Start()
    {
        townPositionsController = FindObjectOfType<TownPositionsController>();
        ClosePanel();
    }
    public void ShowMenu(InputAction.CallbackContext coontext)
    {
        ShowPanel();
    }
    
    public void StartDungeon()
    {
        Debug.Log("StartDungeon");
        ClosePanel();

        FindObjectOfType<SavingUtility>()?.SetPlayerTownPosition();
        //townPositionsController.ChangeToClosestPoint(FindObjectOfType<PlayerController>().transform.position);

        SavingUtility.Instance.SaveToFile();

        SceneManager.LoadScene("DungeonSceneA", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

}

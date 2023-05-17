using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoadDungeon : MonoBehaviour, IOpenCloseMenu
{
    [SerializeField] GameObject panel;
    TownPositionsController townPositionsController;

    public bool PanelEnabled => panel.activeSelf;
    public void OpenMenu() => panel.SetActive(true);
    public void CloseMenu() => panel.SetActive(false);

    public void Start()
    {
        townPositionsController = FindObjectOfType<TownPositionsController>();
        CloseMenu();
    }
    public void ShowMenu(InputAction.CallbackContext coontext)
    {
        OpenMenu();
    }
    
    public void StartDungeon()
    {
        Debug.Log("StartDungeon");
        CloseMenu();

        FindObjectOfType<SavingUtility>()?.SetPlayerTownPosition();
        //townPositionsController.ChangeToClosestPoint(FindObjectOfType<PlayerController>().transform.position);

        SavingUtility.Instance.SaveToFile();

        SceneManager.LoadScene("DungeonSceneA", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

}

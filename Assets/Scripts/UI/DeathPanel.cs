using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] GameObject panel;
    LevelCreator levelCreator;

    public bool PanelEnabled => panel.activeSelf;
    public void ShowPanel() => panel.SetActive(true);
    public void ClosePanel() => panel.SetActive(false);

	public void OkPressed()
    {
        Debug.Log("Return To Town, when dead");
        
        FindObjectOfType<InventoryUI>().StoreItemDataArray();
        SavingUtility.Instance.SaveToFile();

        ClosePanel();
        SceneManager.LoadScene("TownScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController?.SetToPosition(FindObjectOfType<SavingUtility>().GetPlayerTownPosition());
        playerController?.Revive();

    }    
}

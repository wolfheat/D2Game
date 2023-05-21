using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClear : MonoBehaviour, IOpenCloseMenu
{
    [SerializeField] GameObject panel;
    LevelCreator levelCreator;

    public bool PanelEnabled => panel.activeSelf;
    public void ShowPanel() => panel.SetActive(true);
    public void ClosePanel() => panel.SetActive(false);

	public void NewPressed()
    {
        Debug.Log("New Level");
        levelCreator = FindObjectOfType<LevelCreator>();
        if (levelCreator != null)
        {
            levelCreator.CreateNewLevel();
            if(PanelEnabled) ClosePanel();
        }
    }
    
	public void OkPressed()
    {
        Debug.Log("Return To Town");
        Debug.Log("Store Collected Items in Inventory");
        FindObjectOfType<InventoryUI>().StoreItemDataArray();
        SavingUtility.Instance.SaveToFile();

        ClosePanel();
        SceneManager.LoadScene("TownScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        FindObjectOfType<PlayerController>()?.SetToPosition(FindObjectOfType<SavingUtility>().GetPlayerTownPosition());
    }    
}

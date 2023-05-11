using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClear : MonoBehaviour, IOpenCloseMenu
{
    [SerializeField] GameObject panel;
    LevelCreator levelCreator;

    public bool PanelEnabled => panel.activeSelf;
    public void OpenMenu() => panel.SetActive(true);
    public void CloseMenu() => panel.SetActive(false);


    public void ShowPanel()
    {
        if (panel.activeSelf) return;
        Debug.Log("Show Panel");
        OpenMenu();
    }
    
	public void NewPressed()
    {
        Debug.Log("New Level");
        levelCreator = FindObjectOfType<LevelCreator>();
        if (levelCreator != null)
        {
            levelCreator.CreateNewLevel();
            if(PanelEnabled) CloseMenu();
        }
    }
    
	public void OkPressed()
    {
        Debug.Log("Return To Town");
        Debug.Log("Store Collected Items in Inventory");
        FindObjectOfType<InventoryUI>().StoreItemDataArray();
        SavingUtility.Instance.SaveToFile();

        ClosePanel();
        SceneManager.LoadScene("TownScene");
    }
    
	public void ClosePanel()
    {
        Debug.Log("Return To Town, Close pressed");
        panel.SetActive(false);
    }    
}

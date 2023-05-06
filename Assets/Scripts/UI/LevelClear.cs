using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelClear : MonoBehaviour
{
    [SerializeField] GameObject panel;
    LevelCreator levelCreator;

	public void ShowPanel()
    {
        if (panel.activeSelf) return;
        Debug.Log("Show Panel");
        panel.SetActive(true);
    }
    
	public void NewPressed()
    {
        Debug.Log("New Level");
        levelCreator = FindObjectOfType<LevelCreator>();
        if (levelCreator != null)
        {
            levelCreator.CreateNewLevel();
            ClosePanel();
        }

    }
    
	public void OkPressed()
    {
        Debug.Log("Return To Town");

        FindObjectOfType<InventoryUI>().StoreItemDataArray();


        ClosePanel();
        SceneManager.LoadScene("TownScene");
    }
    
	public void ClosePanel()
    {
        Debug.Log("Return To Town, Close pressed");
        panel.SetActive(false);
    }

}

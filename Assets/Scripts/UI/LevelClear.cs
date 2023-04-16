using UnityEngine;

public class LevelClear : MonoBehaviour
{
    [SerializeField] GameObject panel;
    LevelCreator levelCreator;

	public void ActivatePanel()
    {
        if (panel.activeSelf) return;
        Debug.Log("Activate Panel");
        panel.SetActive(true);
    }
    
	public void OkPressed()
    {
        Debug.Log("Return To Town, OK pressed");
        levelCreator = FindObjectOfType<LevelCreator>();
        if (levelCreator != null)
        {
            levelCreator.CreateNewLevel();
            ClosePanel();
        }

    }
    
	public void ClosePanel()
    {
        Debug.Log("Return To Town, Close pressed");
        panel.SetActive(false);
    }

}

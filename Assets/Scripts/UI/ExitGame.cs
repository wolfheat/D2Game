using UnityEngine;

public class ExitGame : MonoBehaviour
{
    [SerializeField] GameObject quitPanel;

    public void Start()
    {
        Inputs.Instance.Controls.Land.ESC.started += _ => HideMenu(quitPanel.activeSelf);
        HideMenu(true);
    }
    public void HideMenu(bool action = true)
    {
        quitPanel.SetActive(!action);
    }
    
	public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}

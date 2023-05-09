using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IOpenCloseMenu
{
    public bool PanelEnabled { get;}
    public void OpenMenu();
    public void CloseMenu();
}

public class ExitGame : MonoBehaviour, IOpenCloseMenu
{
    [SerializeField] GameObject panel;
    [SerializeField] PreReleaseInfoController preReleaseInfoController;
    [SerializeField] TextMeshProUGUI text;

    public bool PanelEnabled => panel.activeSelf;
    public void OpenMenu() => panel.SetActive(true);
    public void CloseMenu() => panel.SetActive(false);
    

    public void Start()
    {
        text.text = preReleaseInfoController.infoText.text;
        CloseMenu();
    }
    
	public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}

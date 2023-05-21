using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IOpenCloseMenu
{
    public bool PanelEnabled { get;}
    public void ShowPanel();
    public void ClosePanel();
}

public class ExitGame : MonoBehaviour, IOpenCloseMenu
{
    [SerializeField] GameObject panel;
    [SerializeField] PreReleaseInfoController preReleaseInfoController;
    [SerializeField] TextMeshProUGUI text;

    public bool PanelEnabled => panel.activeSelf;
    public void ShowPanel() => panel.SetActive(true);
    public void ClosePanel() => panel.SetActive(false);
    

    public void Start()
    {
        text.text = preReleaseInfoController.infoText.text;
        ClosePanel();
    }
    
	public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}

using TMPro;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    [SerializeField] GameObject quitPanel;
    [SerializeField] PreReleaseInfoController preReleaseInfoController;
    [SerializeField] TextMeshProUGUI text;

    public void Start()
    {
        text.text = preReleaseInfoController.infoText.text;
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

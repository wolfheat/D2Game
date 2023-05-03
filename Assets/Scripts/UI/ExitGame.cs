using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExitGame : MonoBehaviour
{
    [SerializeField] GameObject quitPanel;
    [SerializeField] PreReleaseInfoController preReleaseInfoController;
    [SerializeField] TextMeshProUGUI text;

    public void Start()
    {
        text.text = preReleaseInfoController.infoText.text;
        HideMenu(true);
    }
    public void OnEnable()
    {
        Inputs.Instance.Controls.Land.ESC.started += ESCPressed;
    }
    public void OnDisable()
    {
        Inputs.Instance.Controls.Land.ESC.started -= ESCPressed;
    }
    public void ESCPressed(InputAction.CallbackContext context)
    {
        HideMenu(quitPanel.activeSelf);
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

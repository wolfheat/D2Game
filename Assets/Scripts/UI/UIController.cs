using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private TextMeshProUGUI state2Text;
    [SerializeField] public Toggle toggle;

    //Panels
    [SerializeField] public LevelClear levelClear;


    public void ActivateLevelClearPanel()
    {
        levelClear.ActivatePanel();
    }
    private void Awake()
    {
        Debug.Log("UIController awake");
        Debug.Log("Toggle: " + toggle);
    }
    private void Start()
    {
        Debug.Log("UIController start");
    }
    public void DebugTest()
    {
        Debug.Log("Button Works");
    }
    
    public void SetInfoText(string text)
    {
        infoText.text = text;
    }
    
	public void SetStateText(string text)
    {
        stateText.text = text;
    }
    
	public void SetState2Text(string text)
    {
        state2Text.text = text;
    }

}

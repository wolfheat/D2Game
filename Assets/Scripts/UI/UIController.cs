using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; set; }

    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private TextMeshProUGUI state2Text;
    [SerializeField] public Toggle toggle;

    //Panels
    [SerializeField] public LevelClear levelClear;

    private void Awake()
    {
        //Singelton
        if (Instance != null)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);       
        Inputs.Instance.Controls.Land.BackSpace.performed += _ => ActivateLevelClearPanel();
    }

    public void ActivateLevelClearPanel()
    {
        levelClear.ShowPanel();
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

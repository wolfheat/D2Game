using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; set; }

    [SerializeField] private InventoryUI inventoryUI;
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
            Instance.DestroySelf();

        }
        Instance = this;
        DontDestroyOnLoad(this);       
        Inputs.Instance.Controls.Land.BackSpace.performed += ActivateLevelClearPanel;
    }
    
    public void DestroySelf()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        Inputs.Instance.Controls.Land.BackSpace.performed -= ActivateLevelClearPanel;
        Destroy(gameObject);            
    }

    public void ActivateLevelClearPanel()
    {
        levelClear.ShowPanel();
    }
    public void ActivateLevelClearPanel(InputAction.CallbackContext context)
    {
        ActivateLevelClearPanel();
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

    internal bool AddItemToInventory(ItemData itemData)
    {
        return inventoryUI.AddItem(itemData);
    }
}

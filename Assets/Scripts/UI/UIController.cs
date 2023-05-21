using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; set; }

    [SerializeField] private GameObject canvas;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private ExitGame exitGame;

    //Panels
    [SerializeField] public LevelClear levelClear;
    
    private void Awake()
    {
        //Singelton
        if (Instance != null)
        {
           Destroy(this);
            return;
        }
        Instance = this;
        Inputs.Instance.Controls.Land.BackSpace.performed += ActivateLevelClearPanel;
        Inputs.Instance.Controls.Land.ESC.performed += HideOpenMenu;
        Inputs.Instance.Controls.Land.Consol.performed += ToggleConsol;
        //canvas.SetActive(false);
    }
    public void OnDestroy()
    {
        Inputs.Instance.Controls.Land.BackSpace.performed -= ActivateLevelClearPanel;
        Inputs.Instance.Controls.Land.ESC.performed -= HideOpenMenu;
        Inputs.Instance.Controls.Land.Consol.performed -= ToggleConsol;
    }    
    
    public void ActivateCanvas()
    {
        canvas.SetActive(true);
    }

    public void ActivateLevelClearPanel()
    {
        levelClear.ShowPanel();
    }

    public void ActivateLevelClearPanel(InputAction.CallbackContext context)
    {
        ActivateLevelClearPanel();
    } 
    
    public bool IsMenuOpen()
    {
        //Get all active closables
        IOpenCloseMenu[] closables = GetComponentsInChildren<IOpenCloseMenu>();

        for (int i = closables.Length-1; i >= 0 ; i--)
        {
            if (closables[i].PanelEnabled)
            {
                return true;
            }
        }
        return false;
    }

    public void ToggleConsol(InputAction.CallbackContext context)
    {
        InGameConsol.Instance.TogglePanel();
    }
    public void HideOpenMenu(InputAction.CallbackContext context)
    {
        //Get all active closables
        IOpenCloseMenu[] closables = GetComponentsInChildren<IOpenCloseMenu>();

        for (int i = closables.Length-1; i >= 0 ; i--)
        {
            if (closables[i].PanelEnabled)
            {
                closables[i].ClosePanel();
                Debug.Log("Closed Menu");
                return;
            }
        }

        // Reach here if no closables open then open exit
        exitGame.ShowPanel();
    }
        
    internal bool AddItemToInventory(ItemData itemData)
    {
        return inventoryUI.AddItem(itemData);
    }
}

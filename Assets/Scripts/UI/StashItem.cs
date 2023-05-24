using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StashItem : UIItem, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI textField;
    
    public void SetAmount(int amt)
    {
        textField.text = amt.ToString();
        if (amt == 1) textField.alpha = 0.2f;
        else textField.alpha = 1f;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Inputs.Instance.Controls.Land.Shift.inProgress)
            {
                Debug.Log("Left Click (Shift) Stash Item");
                SavingUtility.Instance.MoveItemToInventory(Data.ID);
                FindObjectOfType<InventoryUI>().ReloadFromPlayerInventory();
                FindObjectOfType<StashUI>().UpdateStashItems();
            }
            else
                Debug.Log("Left Click Stash Item");
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(Inputs.Instance.Controls.Land.Shift.inProgress)
                Debug.Log("Right Click (Shift) Stash Item");
            else
            {
                Debug.Log("Right Click Stash Item");
                SavingUtility.Instance.MoveAsManyItemsToInventoryAsPossible(Data.ID);
                FindObjectOfType<InventoryUI>().ReloadFromPlayerInventory();
                FindObjectOfType<StashUI>().UpdateStashItems();
            }
        }
    }

    public void RemoveRayCastTarget()
    {        
        image.raycastTarget = false;
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public InventoryItem HeldItem { get; private set; }
    public bool HasItem { get; private set; } = false;
    public int Index { get; set; } = 0;

    public void PlaceItem(InventoryItem newItem)
    {
        HeldItem = newItem;
        HeldItem.SetParent(this);
        HasItem = true;        
    }

    internal void SwapItemWith(InventoryItem inventoryItem)
    {
        Debug.Log("Swapping Item: "+HeldItem?.Data.Itemname + " with "+inventoryItem.Data.Itemname);
        Slot otherSlot = inventoryItem.ParentSlot;
        if (!HasItem)
        {
            Debug.Log("Just place Item, this slot was empty");
            PlaceItem(inventoryItem);
            otherSlot.ClearSlot();
            return;
        }
        InventoryItem currentlyHeld = HeldItem;
        PlaceItem(inventoryItem);
        otherSlot.PlaceItem(currentlyHeld);
    }

    public void ClearSlotEntirely()
    {
        if (HeldItem != null) Destroy(HeldItem.gameObject);
        HeldItem = null;        
        HasItem = false;
    }
    
    public void ClearSlot(bool destroy=false)
    {
        HeldItem = null;        
        HasItem = false;
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public InventoryItem HeldItem { get; private set; }
    public bool HasItem { get; private set; } = false;

    public InventoryItem PlaceItem(InventoryItem newItem)
    {
        InventoryItem current = HeldItem;
        HeldItem = newItem;
        HeldItem.SetParent(this);
        HasItem = true;
        
        return current;
    }

    internal void SwapItemWith(InventoryItem inventoryItem)
    {
        Debug.Log("Swapping Item: "+HeldItem?.Data.Itemname + " with "+inventoryItem.Data.Itemname);
        Slot otherSlot = inventoryItem.ParentSlot;
        if (!HasItem)
        {
            PlaceItem(inventoryItem);
            otherSlot.ClearSlot();
            return;
        }
        InventoryItem currentlyHeld = HeldItem;
        PlaceItem(inventoryItem);
        otherSlot.PlaceItem(currentlyHeld);
    }

    public void ClearSlot()
    {
        HeldItem = null;
        HasItem = false;
    }
}

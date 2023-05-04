using System;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private InventoryItem heldItem;
    public bool HasItem { get; private set; } = false;

    public InventoryItem PlaceItem(InventoryItem newItem)
    {
        InventoryItem current = heldItem;
        heldItem = newItem;
        heldItem.SetParent(this);
        HasItem = true;
            
        return current;
    }

    internal void SwapItemWith(InventoryItem inventoryItem)
    {
        Debug.Log("Swapping Item: "+heldItem?.Data.Itemname + " with "+inventoryItem.Data.Itemname);
        Slot otherSlot = inventoryItem.ParentSlot;
        if (!HasItem)
        {
            PlaceItem(inventoryItem);
            otherSlot.ClearSlot();
            return;
        }
        InventoryItem currentlyHeld = heldItem;
        PlaceItem(inventoryItem);
        otherSlot.PlaceItem(currentlyHeld);
    }

    public void ClearSlot()
    {
        heldItem = null;
        HasItem = false;
    }
}

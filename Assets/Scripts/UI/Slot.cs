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
        HasItem = true;
            
        return current;
    }
}

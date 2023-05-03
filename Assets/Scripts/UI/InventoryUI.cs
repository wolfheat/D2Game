using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] InventoryItem inventoryItemPrefab;
    [SerializeField] ItemData[] testData;

    [SerializeField] List<Slot> slots;
    
    private const int slotAmount = 20;

    private void Start()
    {
        Inputs.Instance.Controls.Land.G.performed += AddTestData;
    }

    public void UpdateSlots(List<ItemData> items)
    {
        for (int i = 0; i < slots.Count; i++)
        {

        }
    }

    public void AddTestData(InputAction.CallbackContext context)
    {
        int type = Random.Range(0, testData.Length);
        AddItem(testData[type]);
    }
    public void AddItem(ItemData data)
    {
        foreach (var slot in slots)
        {
            if (!slot.HasItem)
            {
                Debug.Log("Adding Item: "+data.name);
                AddItemAt(slot, data);
                return;
            }
        }
        Debug.Log("No Empty Slot To place item in");
    }

    private void AddItemAt(Slot slot, ItemData data)
    {
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, slot.transform);
        inventoryItem.DefineItem(data);
        slot.PlaceItem(inventoryItem);
    }
}

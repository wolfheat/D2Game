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
        LoadItemDataArray();
    }
    
    private void OnDestroy()
    {
        Inputs.Instance.Controls.Land.G.performed -= AddTestData;
    }

    public void StoreItemDataArray()
    {
        Debug.Log("Storing Items In DataSave");
        ItemData[] itemData = new ItemData[slots.Count];
        for (int i = 0; i < slots.Count; i++)
            itemData[i] = slots[i].HasItem?slots[i].HeldItem.Data:null;
        CharacterStats.Items = itemData;
    }
    
    public void LoadItemDataArray()
    {
        Debug.Log("Loading Items From DataSave");
        ItemData[] itemData = CharacterStats.Items;
        if (itemData == null) return;
        for (int i = 0; i < itemData.Length; i++)
            if (itemData[i] != null) AddItemAt(slots[i], itemData[i]);
            else slots[i].ClearSlot();
    }

    public void AddTestData(InputAction.CallbackContext context)
    {
        int type = Random.Range(0, testData.Length);
        AddItem(testData[type]);
    }
    public bool AddItem(ItemData data)
    {
        foreach (var slot in slots)
        {
            if (!slot.HasItem)
            {
                Debug.Log("Adding Item: "+data.name);
                AddItemAt(slot, data);
                return true;
            }
        }
        Debug.Log("No Empty Slot To place item in");
        return false;
    }

    private void AddItemAt(Slot slot, ItemData data)
    {
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, slot.transform);
        inventoryItem.DefineItem(data,slot);
        slot.PlaceItem(inventoryItem);
    }
    public void UpdateItems()
    {

    }

}

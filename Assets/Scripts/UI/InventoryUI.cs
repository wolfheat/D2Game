using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
    SavingUtility savingUtility;

    [SerializeField] InventoryItem inventoryItemPrefab;
    [SerializeField] ItemData[] testData;
    [SerializeField] List<Slot> slots;
    
    private const int slotAmount = 20;
    

    private void Start()
    {
        savingUtility = FindObjectOfType<SavingUtility>();
        Inputs.Instance.Controls.Land.G.performed += AddTestData;
    }
    
    private void OnDestroy()
    {
        Inputs.Instance.Controls.Land.G.performed -= AddTestData;
    }

    public void StoreItemDataArray()
    {
        Debug.Log("StoreItemData");
        int[] itemIDs = new int[slots.Count];
        for (int i = 0; i < slots.Count; i++)
            itemIDs[i] = slots[i].HasItem?slots[i].HeldItem.Data.ID:-1;
        //savingUtility.playerData.Items = itemData;
        //Debug.Log("savingUtility.playerInventory.Items = "+ savingUtility.playerInventory.Items);
        savingUtility.playerInventory.Items = itemIDs;
    }
    
    public void ReloadFromPlayerInventory()
    {
        ItemData[] itemDatas = ItemLibrary.Instance.ItemsAsData(savingUtility.playerInventory.Items);
        LoadItemDataArray(itemDatas);
    }

    public void LoadItemDataArray(ItemData[] itemData)
    {
        Debug.Log("Load Items into Inventory UI");
        //Debug.Log("Itemdata: "+itemData);
        if (itemData == null) return;
        ClearAllSlots();
        for (int i = 0; i < itemData.Length; i++)
        {
            if (itemData[i] != null) AddItemAt(slots[i], itemData[i]);
        }
    }

    private void ClearAllSlots()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].ClearSlotEntirely();
            slots[i].Index = i;
        }
    }

    public void AddTestData(InputAction.CallbackContext context)
    {
        // Check what scene? Depending on town or not save this to SavingUtility.Instance.playerInventory?
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
        HitInfoText.Instance.CreateHitInfo(FindObjectOfType<PlayerController>().transform.position,"Inventory Full!", InfoTextType.Info);
        return false;
    }

    private void AddItemAt(Slot slot, ItemData data)
    {
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, slot.transform);
        inventoryItem.DefineItem(data,slot);
        slot.PlaceItem(inventoryItem);

        StoreItemsIfInTown();
        
    }

    private void StoreItemsIfInTown()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "TownScene")
        {
            //Debug.Log("In Town, Store Inventory when adding to it");
            StoreItemDataArray();
        }else Debug.Log("Not In Town, Inventory not stored when adding this item");
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;

public class StashUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject itemParent;
    [SerializeField] StashItem stashItemPrefab;
    InventoryUI inventoryUI;
    ItemLibrary library;

    private List<StashItem> stashItems = new List<StashItem>();

    private void Start()
    {
        library = FindObjectOfType<ItemLibrary>();
        inventoryUI = FindObjectOfType<InventoryUI>();
    }


    public void ShowPanel()
    {
        if (panel.activeSelf) return;
        Debug.Log("Show Panel");
        panel.SetActive(true);
        UpdateInventoryItems();
    }

    public void ClosePanel()
    {
        Debug.Log("Return To Town, Close pressed");
        panel.SetActive(false);
    }

    public void MoveAllButton()
    {
        Debug.Log("Click On Move All button");
        CharacterStats.AddItemsToStash();
        UpdateStashItems();
        UpdateInventoryItems();
    }

    private void UpdateInventoryItems()
    {
        ClearStash();
        foreach (int ID in CharacterStats.Stash.Keys) 
        {
            StashItem newStashItem = Instantiate(stashItemPrefab, itemParent.transform);
            ItemData data = library.GetItemByID(ID);
            newStashItem.DefineItem(data);
            Debug.Log("Adding "+data.Itemname+" amount: "+ CharacterStats.Stash[ID]);
            newStashItem.SetAmount(CharacterStats.Stash[ID]);
            stashItems.Add(newStashItem);
        }
    }

    private void ClearStash()
    {
        for (int i = stashItems.Count-1; i >= 0; i--)
        {
            Destroy(stashItems[i].gameObject);
        }
        stashItems.Clear();
    }

    private void UpdateStashItems()
    {
        inventoryUI.LoadItemDataArray();
    }
}

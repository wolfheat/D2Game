using System;
using System.Collections.Generic;
using UnityEngine;

public class StashUI : MonoBehaviour, IOpenCloseMenu
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject itemParent;
    [SerializeField] StashItem stashItemPrefab;
    InventoryUI inventoryUI;
    ItemLibrary library;

    private List<StashItem> stashItems = new List<StashItem>();

    public bool PanelEnabled => panel.activeSelf;
    public void OpenMenu(){panel.SetActive(true); UpdateStashItems();}
    public void CloseMenu() => panel.SetActive(false);

    private void Start()
    {
        library = FindObjectOfType<ItemLibrary>();
        inventoryUI = FindObjectOfType<InventoryUI>();
        CloseMenu();
    }

    public void MoveAllButton()
    {
        Debug.Log("Click On Move All button");
        SavingUtility.Instance.MoveAllItemsToStash();
        UpdateStashItems();
        UpdateInventoryItems();
    }

    public void UpdateStashItems()
    {
        Debug.Log("Updating Stash Items. Currently clearing it and remaking it change later.");
        ClearStash();
        Debug.Log("Stash Itemcount: "+ SavingUtility.Instance.playerInventory.Stash.Count);
        if (SavingUtility.Instance.playerInventory.Stash.Count == 0) return;
        foreach (int ID in SavingUtility.Instance.playerInventory.Stash.Keys) 
        {
            StashItem newStashItem = Instantiate(stashItemPrefab, itemParent.transform);
            ItemData data = library.GetItemByID(ID);
            newStashItem.DefineItem(data);
            //Debug.Log("Adding "+data.Itemname+" amount: "+ SavingUtility.Instance.playerInventory.Stash[ID]);
            newStashItem.SetAmount(SavingUtility.Instance.playerInventory.Stash[ID]);
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

    public void UpdateInventoryItems()
    {
        inventoryUI.ReloadFromPlayerInventory();
    }
}

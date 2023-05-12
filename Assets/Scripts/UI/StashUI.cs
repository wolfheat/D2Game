using System;
using System.Collections.Generic;
using System.Linq;
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
        Debug.Log("Updating Stash Items.");
        ClearStash();

        int ItemTypesInStash = SavingUtility.Instance.playerInventory.Stash.Count;
        int ItemTypesInUI = stashItems.Count;
        if (ItemTypesInUI > ItemTypesInStash) DeleteAmountFromStash(ItemTypesInUI-ItemTypesInStash);
        int index = 0;
        if (SavingUtility.Instance.playerInventory.Stash.Count == 0) return;
        foreach (int ID in SavingUtility.Instance.playerInventory.Stash.Keys) 
        {
            ItemData data = library.GetItemByID(ID);

            StashItem newStashItem;

            if (index > stashItems.Count - 1)
            {
                newStashItem = Instantiate(stashItemPrefab, itemParent.transform);
                stashItems.Add(newStashItem);
            }
            else
                newStashItem = stashItems[index];

            newStashItem.DefineItem(data);            
            newStashItem.SetAmount(SavingUtility.Instance.playerInventory.Stash[ID]);
            index++;
        }
    }

    private void DeleteAmountFromStash(int v)
    {
        for (int i = stashItems.Count-1; i > stashItems.Count - 1 - v; i++)
        {
            Destroy(stashItems[i].gameObject);
            stashItems.RemoveAt(i);
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

using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using static UnityEditor.Progress;

public class SavingUtility : MonoBehaviour
{

    private const string LocalSaveLovation = "/player-data.json";

    [SerializeField] public PlayerInventory playerInventory = new PlayerInventory();

    public static SavingUtility Instance { get; set; }  

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;    
        }
        StartCoroutine(LoadFromFile());


    }

    private void OnApplicationQuit()
    {
        SaveToFile();
    }

    public void SaveToFile()
    {
        IDataService dataService = new JsonDataService();
        if (dataService.SaveData(LocalSaveLovation, playerInventory, false))
            Debug.Log("Saved in: "+LocalSaveLovation);
        else
            Debug.LogError("Could not save file.");
    }
    public IEnumerator LoadFromFile()
    {
        // Hold the load for 1 second so Library has time to load
        yield return new WaitForSeconds(0.4f);


        IDataService dataService = new JsonDataService();
        try
        {
            playerInventory = dataService.LoadData<PlayerInventory>(LocalSaveLovation, false);
            Debug.Log("Loading items from file: "); 
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e.Message);
        }
        finally
        {
            ItemData[] itemDatas = FindObjectOfType<ItemLibrary>().ItemsAsData(playerInventory.Items);
            FindObjectOfType<InventoryUI>().LoadItemDataArray(itemDatas);

            //FindObjectOfType<StashUI>()
        }
    }

    public void UpdatePlayerInventoryItems(ItemData[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            playerInventory.Items[i] = items[i].ID;
        }
    }

    internal void MoveItemToStash(int slotIndex)
    {
        if(playerInventory.Items[slotIndex] > 0)
        {
            AddOneToStash(playerInventory.Items[slotIndex]);
            playerInventory.Items[slotIndex] =-1;
        }
    }
    internal bool MoveItemToInventory(int ID)
    {
        if (!playerInventory.Stash.ContainsKey(ID)) return false;
        if (playerInventory.Stash[ID] > 0) // Got at least one item in stash
        {
            if (AddOneToInventory(ID))
            {
                
                RemoveOneFromStash(ID);
                return true;
            }
        }
        return false;
    }

    internal bool AddOneToInventory(int ID)
    {
        for(int i = 0; i < playerInventory.Items.Length; i++)
        {
            if (playerInventory.Items[i] <=0)
            {
                playerInventory.Items[i] = ID;
                return true;
            }   
        }
        return false;
    }
    
    internal bool RemoveFromInventory(int index)
    {
        if (playerInventory.Items[index]<1) return false;
        playerInventory.Items[index] = -1;
        return true;        
    }
    internal void MoveAllItemsOfThisTypeToStash(int ID)
    {        
        // Copy Items to Stash
        for (int i = 0; i < playerInventory.Items.Length; i++)
        {
            if (playerInventory.Items[i] == ID)
            {
                AddOneToStash(ID);
                playerInventory.Items[i] = -1;
            }
        }
    }
    internal void MoveAllItemsToStash()
    {
        // Copy Items to Stash
        for (int i = 0; i < playerInventory.Items.Length; i++)
        {
            int ID = playerInventory.Items[i];
            if (ID < 1) continue;
            AddOneToStash(ID);
            
            // remove the Items from inventory
            playerInventory.Items[i] = -1;
        }
    }

    private void RemoveOneFromStash(int ID)
    {
        Debug.Log("Remove on from stash");
        if (!playerInventory.Stash.ContainsKey(ID)) return;
        if (playerInventory.Stash[ID] == 1)
        {
            playerInventory.Stash.Remove(ID);
            return;
        }
        playerInventory.Stash[ID]--;        
    }
    private void AddOneToStash(int ID)
    {
        if (!playerInventory.Stash.ContainsKey(ID)) playerInventory.Stash.Add(ID, 0);

        playerInventory.Stash[ID]++;
    }
    public void AddXP(int xp)
    {
        playerInventory.XP += xp;        
    }

    internal void MoveAsManyItemsToInventoryAsPossible(int ID)
    {
        if (playerInventory.Stash[ID] <= 0) return;

        for (int i = 0; i < playerInventory.Items.Length; i++)
        {
            if (playerInventory.Items[i] <= 0)
            {
                playerInventory.Stash[ID]--;
                playerInventory.Items[i] = ID;
                if (playerInventory.Stash[ID] <= 0)
                {
                    playerInventory.Stash.Remove(ID);
                    return;
                }
            }
        }
    }
}

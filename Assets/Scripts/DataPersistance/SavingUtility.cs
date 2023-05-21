using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
        Debug.Log("Shift held: "+ Inputs.Instance.Controls.Land.Shift.IsPressed());
        Debug.Log("Ctrl held: "+ Inputs.Instance.Controls.Land.CTRL.IsPressed());
#if UNITY_EDITOR
        Debug.Log("Exiting in Unity Editor = Do not Save to file");
#else
        SaveToFile();
#endif
    }

    public void SaveToFile()
    {
        //Debug.Log("Saving to file: position is: "+playerInventory.TownPos);
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
            Debug.Log(" - Loading items from file! - "); 
        }
        catch (Exception e)
        {
            Debug.LogError("Exception: " + e.Message);
        }
        finally
        {
            Debug.Log("Items loaded from file: FINALLY"); 
            ItemData[] itemDatas = FindObjectOfType<ItemLibrary>().ItemsAsData(playerInventory.Items);  
            FindObjectOfType<InventoryUI>().LoadItemDataArray(itemDatas);
            // Check if in town if the townposition is to be set
            if(SceneManager.GetActiveScene().name == "TownScene")
                FindObjectOfType<PlayerController>()?.SetToPosition(FindObjectOfType<SavingUtility>().GetPlayerTownPosition());
            else Debug.Log("Not Town Scene");
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

    public Vector3 GetPlayerTownPosition()
    {
        Vector3 pos = new Vector3(playerInventory.TownPos[0], playerInventory.TownPos[1], playerInventory.TownPos[2]);
        Debug.Log("Reading player position from file location: "+pos);
        return pos;
    }

    public void SetPlayerTownPosition()
    {
        Vector3 fixedPosition = FindObjectOfType<TownPositions>().GetClosestPosition(FindObjectOfType<PlayerController>().transform.position);
        SetPlayerTownPositionFixed(fixedPosition);
        //SetPlayerTownPositionFixed(FindObjectOfType<PlayerController>().transform.position);
    }
    public void SetPlayerTownPositionFixed(Vector3 pos)
    {
        Debug.Log("Store new player town position in file location "+pos);
        playerInventory.TownPos = new float[] {pos.x,pos.y,pos.z };
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

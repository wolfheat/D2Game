using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

public class AssetReferenceItemData : AssetReferenceT<ItemData> {
    public AssetReferenceItemData(string guid) : base(guid) { }
}


public class ItemLibrary : MonoBehaviour
{
    [SerializeField] Dictionary<int,ItemData> libraryItemList = new Dictionary<int,ItemData>();
    [SerializeField] List<AssetReferenceItemData> libraryReferenceItemList = new List<AssetReferenceItemData>();

    [SerializeField] AssetLabelReference assetLabelReference;

    public static ItemLibrary Instance { get; set; }

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
        //Debug.Log("Load all items into library");

        LoadAllItems();
    }

    private void LoadAllItems()
    {
        InGameConsol.Instance.AddInfo("Loading addressables items into the Library.");

        Addressables.LoadAssetsAsync<ItemData>(assetLabelReference, (itemData) =>
        {
            libraryItemList.Add(itemData.ID,itemData);
            InGameConsol.Instance.AddInfo("Loaded item to Library: " + itemData.Itemname);
            //Debug.Log("Loaded item to Library: "+itemData.Itemname);
        });
    }

    public ItemData GetRandomItem()
    {
        if(libraryItemList.Count < 1) return null;

        List<int> keys = libraryItemList.Keys.ToList();
        int randomID = keys[Random.Range(0, keys.Count)];
        return libraryItemList[randomID];
    }
    
    public ItemData GetItemByID(int id)
    {
        return libraryItemList[id];
    }

    internal ItemData[] ItemsAsData(int[] items)
    {
        ItemData[] itemDatas = new ItemData[items.Length];
        for (int i = 0; i < items.Length; i++)
        {
            //Debug.Log("Retrieving From Library ID: " + items[i]);
            if (items[i] > 0) itemDatas[i] = libraryItemList[items[i]];
            else itemDatas[i] = null;
        }
        return itemDatas;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetReferenceItemData : AssetReferenceT<ItemData> {
    public AssetReferenceItemData(string guid) : base(guid) { }
}


public class ItemLibrary : MonoBehaviour
{
    [SerializeField] Dictionary<int,ItemData> libraryItemList = new Dictionary<int,ItemData>();
    [SerializeField] List<AssetReferenceItemData> libraryReferenceItemList = new List<AssetReferenceItemData>();

    [SerializeField] AssetLabelReference assetLabelReference;

	private void Start()
    {
        Debug.Log("Load all items into library");

        LoadAllItems();
    }

    private void LoadAllItems()
    {
        Addressables.LoadAssetsAsync<ItemData>(assetLabelReference, (itemData) =>
        {
            Debug.Log("Loaded:" + itemData.Itemname);
            libraryItemList.Add(itemData.ID,itemData);
        });                        
    }

    public ItemData GetItemByID(int id)
    {
        return libraryItemList[id];
    }
}

using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] ItemData itemDataPrefab;
    [SerializeField] ItemData[] itemDataPrefabs;
    [SerializeField] PickupItem pickupItemPrefab;
    [SerializeField] Transform itemHolder;

	private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        Inputs.Instance.Controls.Land.F.performed += _ => GenerateItem(itemDataPrefabs[Random.Range(0,itemDataPrefabs.Length)]);
    }

    public void GenerateItem(ItemData itemData)
    {
        Debug.Log("Spawned ITEM: "+itemData.Itemname);
        
        Vector3 pos = playerController.transform.position+playerController.transform.forward*3f+Vector3.up*0.2f;
        PickupItem newItem = Instantiate(pickupItemPrefab,pos,Quaternion.identity);
        newItem.Init(itemData);
        

    }
    
    public void GenerateItemAt(ItemData itemData, Vector3 pos)
    {
        Debug.Log("Spawned ITEM: "+itemData.Itemname);

        PickupItem newItem = Instantiate(pickupItemPrefab,pos,Quaternion.identity,itemHolder);
        newItem.Init(itemData);
        

    }

}

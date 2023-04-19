using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] ItemData itemDataPrefab;
    [SerializeField] ItemData[] itemDataPrefabs;
    [SerializeField] PickupItem pickupItemPrefab;

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

}

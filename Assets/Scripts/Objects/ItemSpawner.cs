using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

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
        Inputs.Instance.Controls.Land.F.performed += SpawnTestItem;
    }
    
	private void OnDestroy()
    {
        Inputs.Instance.Controls.Land.F.performed -= SpawnTestItem;
    }

    public void SpawnTestItem(InputAction.CallbackContext context)
    {
        ItemData randomData = FindObjectOfType<ItemLibrary>().GetRandomItem();
        // Currently using list for items, use itemlibrary instead?
        if(randomData != null) GenerateItem(randomData);
        //GenerateItem(itemDataPrefabs[Random.Range(0, itemDataPrefabs.Length)]);
    }
    public void GenerateItem(ItemData itemData)
    {
        // Get position in front of player
        //Vector3 pos = playerController.transform.position+playerController.transform.forward*3f+Vector3.up*0.2f;
        Vector3 pos = playerController.transform.position+playerController.transform.forward*0.5f+Vector3.up*0.2f;
        Vector3 dir = playerController.transform.forward;

        GenerateItemAt(itemData, pos, dir);
    }
    
    public void GenerateItemAt(ItemData itemData, Vector3 pos, Vector3 dir)
    {
        Debug.Log("Spawned ITEM: "+itemData.Itemname);

        PickupItem newItem = Instantiate(pickupItemPrefab,pos,Quaternion.identity,itemHolder);
        newItem.Init(itemData, dir);
        

    }

}

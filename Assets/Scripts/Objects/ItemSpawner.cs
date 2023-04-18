using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] ItemData itemDataPrefab;
    [SerializeField] PickupItem pickupItemPrefab;

	private void Start()
    {
        Inputs.Instance.Controls.Land.F.performed += _ => GenerateItem(itemDataPrefab);
    }

    public void GenerateItem(ItemData itemData)
    {
        
        Vector3 pos = playerController.transform.position+playerController.transform.forward*3f;
        PickupItem newItem = Instantiate(pickupItemPrefab,pos,Quaternion.identity);
        newItem.Init(itemData);
        
        Debug.Log("");

        

    }

}

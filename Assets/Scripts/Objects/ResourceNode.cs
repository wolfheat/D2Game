using System;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    ItemSpawner itemSpawner;
    ItemData itemData;
    internal void Start()
    {
        itemSpawner = FindObjectOfType<ItemSpawner>();
    }

    internal void LoadWithItem(ItemData itemDataIn)
    {
        itemData = itemDataIn;
    }

    internal void Harvest()
    {
        if (itemData != null)
        {
            itemSpawner.GenerateItemAt(itemData, transform.position);
        }        
        RemoveAnimation();
    }

    private void RemoveAnimation()
    {
        // Play Animation
        // Create Particle effects
        // Play Sound
        // Destroy disable gameobject
        Destroy(gameObject);
    }
}

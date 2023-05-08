using System;
using UnityEngine;


public enum ResourceType{MiningNode, FishingNode, WoodcuttingNode, ScavengingNode, Stash}
public abstract class ResourceNode : MonoBehaviour, IInteractable
{
    ItemSpawner itemSpawner;
    ItemData itemData;
    protected ResourceType type;
    public ResourceType Type{ get {return type;} protected set{} }

    internal virtual void Start()
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
        Destroy(base.gameObject);
    }
    public void Interract()
    {
        Debug.Log("Interact with: "+type);
        Harvest();
    }

}

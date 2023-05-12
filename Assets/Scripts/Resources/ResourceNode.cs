using System;
using UnityEngine;


public enum ResourceType{MiningNode, FishingNode, WoodcuttingNode, ScavengingNode, WellResourceNode, CookingResourceNode, Stash }
public abstract class ResourceNode : MonoBehaviour, IInteractable
{
    ItemSpawner itemSpawner;
    ItemData itemData;
    protected ResourceType type;
    protected bool destroyable = true;
    protected bool spawnItem = true;
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
            if(spawnItem) itemSpawner.GenerateItemAt(itemData, transform.position);
            else UIController.Instance.AddItemToInventory(itemData);
        }
        else Debug.LogWarning("Itemdata not set for this interactable");        

        if (destroyable) RemoveAnimation();
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

using UnityEngine;

public class CookingResourceNode : ResourceNode
{
    [SerializeField] ItemData loadWithItemData;
    internal override void Start()
    { 
        destroyable = false;    
        type = ResourceType.CookingResourceNode;
        spawnItem = false;
        base.Start();
        if (loadWithItemData != null )
            LoadWithItem(loadWithItemData);
    }
}

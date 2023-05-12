using UnityEngine;

public class WellResourceNode : ResourceNode
{
    [SerializeField] ItemData loadWithItemData;
    internal override void Start()
    { 
        destroyable = false;    
        type = ResourceType.WellResourceNode;
        spawnItem = false;
        base.Start();
        if (loadWithItemData != null )
            LoadWithItem(loadWithItemData);
    }
}

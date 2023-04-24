using UnityEngine;

public class WoodcuttingResourceNode : ResourceNode
{
    internal override void Start()
    {
        type = ResourceType.Woodcutting;
        base.Start();
    }

}

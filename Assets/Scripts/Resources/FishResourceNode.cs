using UnityEngine;

public class FishResourceNode : ResourceNode
{
    internal override void Start()
    {
        type = ResourceType.FishingNode;
        base.Start();
    }

}

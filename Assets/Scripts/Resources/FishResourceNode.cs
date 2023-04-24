using UnityEngine;

public class FishResourceNode : ResourceNode
{
    internal override void Start()
    {
        type = ResourceType.Fishing;
        base.Start();
    }

}

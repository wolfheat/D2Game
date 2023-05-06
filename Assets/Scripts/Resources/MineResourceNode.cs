using UnityEngine;

public class MineResourceNode : ResourceNode
{
    internal override void Start()
    {
        type = ResourceType.MiningNode;
        base.Start();
    }
}

using UnityEngine;

public class ScavengeResourceNode : ResourceNode
{
    internal override void Start()
    {
        type = ResourceType.Scavenging;
        base.Start();
    }

}

using UnityEngine;

public interface IInteractable
{
    ResourceType Type { get; }
    GameObject gameObject { get; }

    void Interract();
}

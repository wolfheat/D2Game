using UnityEngine;

public interface IInteractable
{
    ResourceType Type { get; }

    void Interract();
}

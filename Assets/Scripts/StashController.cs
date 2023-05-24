using System;
using UnityEngine;

public class StashController : MonoBehaviour, IInteractable
{
    public ResourceType Type => ResourceType.Stash;

    public void Interract()
    {
        Debug.Log("Interact with Stash: " + Type);
    }

}
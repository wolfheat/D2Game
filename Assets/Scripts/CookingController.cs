using UnityEngine;

public class CookingController : MonoBehaviour, IInteractable
{
    public ResourceType Type => ResourceType.CookingStation;

    public void Interract()
    {
        Debug.Log("Interact with Cooking resource node: " + Type);
    }

}

using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PortalController : MonoBehaviour
{
    UIController uIController;

	private void Awake()
    {
            uIController = FindObjectOfType<UIController>();
    }

	private void OnTriggerEnter(Collider other)
	{
		if (uIController != null)
		{
			PlayerController playerController = other.GetComponent<PlayerController>();
			if(playerController != null)
			{
				uIController.ActivateLevelClearPanel();
			}
		}
	}
}

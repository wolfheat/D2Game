using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    [SerializeField] Light spotlight;
	private void Start()
    {
        UseSpotLight(false);
    }

	public void UseSpotLight(bool use)
    {
        spotlight.enabled = use;
    }

}

using UnityEngine;
using UnityEngine.UI;

public class SetImageThreshold : MonoBehaviour
{
	private void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;    
    }

}

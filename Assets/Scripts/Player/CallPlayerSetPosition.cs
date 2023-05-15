using UnityEngine;

public class CallPlayerSetPosition : MonoBehaviour
{
	private void Start()
    {
        FindObjectOfType<SetPlayerStartPosition>().PlacePlayerOnStoredPosition();
    }

}

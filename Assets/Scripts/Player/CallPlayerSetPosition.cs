using UnityEngine;

public class CallPlayerSetPosition : MonoBehaviour
{
	private void Start()
    {
        Debug.Log("CallPlayerSetPosition - START");
        Init(); 
    }

    public void Init()
    {
        Debug.Log("CallPlayerSetPosition");
        FindObjectOfType<PlayerController>()?.SetToPosition(FindObjectOfType<SavingUtility>().GetPlayerTownPosition());
        FindObjectOfType<UIController>()?.ActivateCanvas();

    }

}

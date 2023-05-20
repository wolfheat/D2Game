using System.Collections;
using UnityEngine;

public class CallPlayerSetPosition : MonoBehaviour
{
	private void Start()
    {
        Debug.Log("CallPlayerSetPosition - START");
        StartCoroutine(Init()); 
    }

    public IEnumerator Init()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("CallPlayerSetPosition");
        FindObjectOfType<PlayerController>()?.SetToPosition(FindObjectOfType<SavingUtility>().GetPlayerTownPosition());
        FindObjectOfType<UIController>()?.ActivateCanvas();

    }

}

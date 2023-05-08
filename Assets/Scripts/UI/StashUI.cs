using UnityEngine;

public class StashUI : MonoBehaviour
{
    [SerializeField] GameObject panel;

    public void ShowPanel()
    {
        if (panel.activeSelf) return;
        Debug.Log("Show Panel");
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        Debug.Log("Return To Town, Close pressed");
        panel.SetActive(false);
    }

}

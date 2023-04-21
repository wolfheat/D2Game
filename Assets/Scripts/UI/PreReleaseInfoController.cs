using UnityEngine;

public class PreReleaseInfoController : MonoBehaviour
{
    [SerializeField] GameObject aboutButton;
    [SerializeField] GameObject aboutPanel;
	private void Start()
    {
        ShowMenu(false);
    }

	public void ShowMenu(bool action)
    {
        aboutButton.SetActive(!action);
        aboutPanel.SetActive(action);
    }

}

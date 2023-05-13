using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ForceSave : MonoBehaviour
{
	private void Start()
    {
        Inputs.Instance.Controls.Land.S.started += ForcingSave;
    }

    private void ForcingSave(InputAction.CallbackContext context)
    {
        if (Inputs.Instance.Controls.Land.Shift.IsPressed())
        {
#if UNITY_EDITOR
            Debug.Log("IN EDITOR - Force Save to File");
            GetComponent<SavingUtility>().SaveToFile();
            Vector3 pos = FindObjectOfType<PlayerController>().transform.position;
            HitInfoText.Instance.CreateHitInfo(pos, "Game Saved To File", InfoTextType.Info);
#endif
        }
    }

	private void OnDestroy()
    {
        Inputs.Instance.Controls.Land.S.started -= ForcingSave;
    }

}

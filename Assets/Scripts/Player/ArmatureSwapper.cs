using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArmatureSwapper : MonoBehaviour
{
    [SerializeField] Armature[] armatures;
    [SerializeField] Animator animator;
    private int activeAvatar = 0;
    [SerializeField] GameObject ToolsAndWeaponsHolder;
    [SerializeField] GameObject BowHolder;
    [SerializeField] GameObject sword;
    [SerializeField] GameObject bow;


	private void Start()
    {
        Inputs.Instance.Controls.Land.T.performed += SetAvatar;
    }

    private void SetAvatar(InputAction.CallbackContext context)
    {
        Debug.Log("Change Avatar");
        LoadNext();
    }

    private void LoadNext()
    {
        UnloadCurrent();
        activeAvatar = (activeAvatar + 1) % armatures.Length;
        LoadCurrent();
    }

    private void LoadCurrent()
    {
        armatures[activeAvatar].gameObject.SetActive(true);
        ToolsAndWeaponsHolder.transform.SetParent(armatures[activeAvatar].swordPosition.transform,false);
        BowHolder.transform.SetParent(armatures[activeAvatar].bowPosition.transform,false);

        animator.avatar = armatures[activeAvatar].avatar;

    }
    private void UnloadCurrent()
    {
        armatures[activeAvatar].gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    private float timeToGetBored = 3f;
    private int idleAnimations = 4;
    private float boredTimer = 0f;
    int animationToRun = 0;
    bool isBored = false;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Idle Behaviour Start.");
        ResetState();
	}

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isBored)
        {
            boredTimer += Time.deltaTime;
            if(boredTimer > timeToGetBored)
            {
                // Wait for previous animation to complete
                if (stateInfo.normalizedTime % 1 < 0.02f)
                {
                    boredTimer = 0f;
                    animationToRun = Random.Range(1, idleAnimations);
                    //Debug.Log("New Behaviour: " + animationToRun);
                    isBored = true;
                }
            }
        }
        else if(stateInfo.normalizedTime % 1 > 0.98f)
        {
            ResetState();
            isBored=false;
        }
        //Debug.Log("Time: " + stateInfo.normalizedTime % 1 + "s");
        animator.SetFloat("IdleBlend", animationToRun, 0.15f, Time.deltaTime);
    }

    private void ResetState()
    {
        //Debug.Log("Idle Behaviour Reset.");
        animationToRun = 0;
	}

}

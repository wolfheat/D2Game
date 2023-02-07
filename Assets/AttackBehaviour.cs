using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    private float attackDelay = 3f;
    private float attackTimer = 0f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackTimer+= Time.deltaTime;
        if (attackTimer > attackDelay)
        {
            animator.SetBool("Attacking", true);
        }
        else if (stateInfo.normalizedTime % 1 > 0.98f)
		{
			animator.SetBool("Attacking", false);
            attackTimer = 0f;
		}
	}

}

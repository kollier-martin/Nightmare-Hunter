using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackPlayer : StateMachineBehaviour
{
    private int rand;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rand = Random.Range(0, 2);

        if (rand == 1)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRandomScript : StateMachineBehaviour
{
    #region Variable
    //private int numOfStates = 1;
    private int Range = 20;
    private float minNormTime = 0f;
    private float MaxNormTime = 2f;

    private float randomNormalTime;

    //해쉬 값을 가지고 있는 게 연산 시 스트링 연산이 아니라 int 연산이 가능해 속도적 이득이 있다.
    readonly int hashRandomIdle = Animator.StringToHash("RandomIdle");



    #endregion
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //트랜지션에 필요한 시간 랜덤으로
        randomNormalTime = Random.Range(minNormTime, MaxNormTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //현재 서브스테이트에 접속하지 않았거나 현재 필요한 상태가 없는 상태라면
        //베이스 레이어(animator.IsInTransition(0))이고
        //현재 상태와 같은 게 있다면(animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
        //현재 idle state machine가 아니라는 듰
        if (animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
        {
            animator.SetInteger(hashRandomIdle, -1);
        }
        //미리 상태가 미리 계산한 시간이 지났다면
        //stateInfo,nomalizedTime 상태에 진입한 이후 시간
        if(stateInfo.normalizedTime > randomNormalTime && !animator.IsInTransition(0))
        {
            animator.SetInteger(hashRandomIdle, Random.Range(0, Range)); 
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

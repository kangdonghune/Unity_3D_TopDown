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

    //�ؽ� ���� ������ �ִ� �� ���� �� ��Ʈ�� ������ �ƴ϶� int ������ ������ �ӵ��� �̵��� �ִ�.
    readonly int hashRandomIdle = Animator.StringToHash("RandomIdle");



    #endregion
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Ʈ�����ǿ� �ʿ��� �ð� ��������
        randomNormalTime = Random.Range(minNormTime, MaxNormTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //���� ���꽺����Ʈ�� �������� �ʾҰų� ���� �ʿ��� ���°� ���� ���¶��
        //���̽� ���̾�(animator.IsInTransition(0))�̰�
        //���� ���¿� ���� �� �ִٸ�(animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
        //���� idle state machine�� �ƴ϶�� ��
        if (animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
        {
            animator.SetInteger(hashRandomIdle, -1);
        }
        //�̸� ���°� �̸� ����� �ð��� �����ٸ�
        //stateInfo,nomalizedTime ���¿� ������ ���� �ð�
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

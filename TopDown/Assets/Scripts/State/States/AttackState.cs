using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
{
    private Animator _animator;
    private AttackStateController _attackStateController;
    private IAttackable _attackable;

    private int hashAttackTrigger = Animator.StringToHash("AttackTrigger");
    protected int hashAttackIndex = Animator.StringToHash("AttackIndex");

    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _attackStateController = context.GetComponent<AttackStateController>();
        _attackable = context.GetComponent<IAttackable>();
    }

    public override void Enter()
    {
        if (_attackable == null)
        {
            Debug.LogError($"{context.gameObject.name} Attack, but does'n have IAttackable!!");
            stateMachine.ChangeState<IdleState>();
            return;
        }

        if (_attackable.CurrentAttackBehavior == null)
        {
            stateMachine.ChangeState<IdleState>();
        }
        //트리거를 키기 전에 컨트롤러 이벤트핸들러에 함수 추가
        _attackStateController.enterAttackStateHandler += OnEnterAttackState;
        _attackStateController.exitAttackStateHandler += OnExitAttackState;

        _animator.SetInteger(hashAttackIndex, _attackable.CurrentAttackBehavior.animationIndex);
        //실제 애니메이터에서 state로 넘어가는 건 트리거를 켜주는 순간
        _animator.SetTrigger(hashAttackTrigger);
    }
    
    public override void Update(float deltaTime)
    {
    }
    public override void Exit()
    {
        //퇴장 시 추가 했던 함수 제거(+는 중첩이 되지만 -는 중첩이 안되니 안전상 걸어두는편이 좋다.)
        _attackStateController.enterAttackStateHandler -= OnEnterAttackState;
        _attackStateController.exitAttackStateHandler -= OnExitAttackState;
    }

    public void OnEnterAttackState()
    {
        stateMachine.ChangeState<AttackState>();
    }

    public void OnExitAttackState()
    {
        stateMachine.ChangeState<IdleState>();
    }

}

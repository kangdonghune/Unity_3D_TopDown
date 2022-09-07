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
        context.transform.LookAt(context.Target.position);

        if (_attackable == null)
        {
            Debug.LogError($"{context.gameObject.name} Attack, but does'n have IAttackable!!");
            stateMachine.ChangeState<IdleState>();
            return;
        }

        if (_attackable.CurrentAttackBehavior == null)
        {
            stateMachine.ChangeState<IdleState>();
            return;
        }
        //Ʈ���Ÿ� Ű�� ���� ��Ʈ�ѷ� �̺�Ʈ�ڵ鷯�� �Լ� �߰�
        _attackStateController.enterAttackStateHandler += OnEnterAttackState;
        _attackStateController.exitAttackStateHandler += OnExitAttackState;

        _animator.SetInteger(hashAttackIndex, _attackable.CurrentAttackBehavior.animationIndex);
        //���� �ִϸ����Ϳ��� state�� �Ѿ�� �� Ʈ���Ÿ� ���ִ� ����
        _animator.SetTrigger(hashAttackTrigger);
    }
    
    public override void Update(float deltaTime)
    {

    }
    public override void Exit()
    {
        //���� �� �߰� �ߴ� �Լ� ����(+�� ��ø�� ������ -�� ��ø�� �ȵǴ� ������ �ɾ�δ����� ����.)
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

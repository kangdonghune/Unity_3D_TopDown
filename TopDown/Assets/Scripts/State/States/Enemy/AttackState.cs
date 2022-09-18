using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
{
    private Animator _animator;
    private AttackStateController _attackStateController;
    private IAttackable _attackable;
    private Vector3 _lookDir;

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
        if (context.Target != null)
            _lookDir = (context.Target.position - context.transform.position).normalized;
        else
            _lookDir = context.transform.forward;
        if (_attackable == null)
        {
            Debug.LogError($"{context.gameObject.name} Attack, but does'n have IAttackable!!");
            stateMachine.ChangeState<IdleState>();
            return;
        }
        context.CheckAttackBehavior();
        if (_attackable.CurrentAttackBehavior == null)
        {
            stateMachine.ChangeState<IdleState>();
            return;
        }
        //Ʈ���Ÿ� Ű�� ���� ��Ʈ�ѷ� �̺�Ʈ�ڵ鷯�� �Լ� �߰�
        _attackStateController.enterAttackStateHandler += OnEnterAttackState;
        _attackStateController.exitAttackStateHandler += OnExitAttackState;

        _animator.SetInteger(hashAttackIndex, _attackable.CurrentAttackBehavior.AnimationIndex);
        //���� �ִϸ����Ϳ��� state�� �Ѿ�� �� Ʈ���Ÿ� ���ִ� ����
        _animator.SetTrigger(hashAttackTrigger);
    }
    
    public override void Update(float deltaTime)
    {
        context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.LookRotation(_lookDir), 0.2f);
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

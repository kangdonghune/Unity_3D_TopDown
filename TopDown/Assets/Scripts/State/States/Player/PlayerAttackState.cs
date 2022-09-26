using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttackState : State<PlayerController>
{
    private Animator _animator;
    private NavMeshAgent _navAgent;
    private CharacterController _characterController;
    private AttackStateController _attackStateController;
    private IAttackable _attackable;
    private Vector3 _lookDir;

    private int hashAttack = Animator.StringToHash("Attack");

    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _navAgent = context.GetComponent<NavMeshAgent>();
        _characterController = context.GetComponent<CharacterController>();
        _attackStateController = context.GetComponent<AttackStateController>();
        _attackable = context.GetComponent<IAttackable>();
    }

    public override void Enter()
    {
        context.CheckAttackBehavior();
        if (_attackable.CurrentAttackBehavior == null)
        {
            stateMachine.ChangeState<PlayerIdleState>();
            return;
        }
        //Ʈ���Ÿ� Ű�� ���� ��Ʈ�ѷ� �̺�Ʈ�ڵ鷯�� �Լ� �߰�
        _attackStateController.enterAttackStateHandler += OnEnterAttackState;
        _attackStateController.exitAttackStateHandler += OnExitAttackState;
        //���� �ִϸ����Ϳ��� state�� �Ѿ�� �� Ʈ���Ÿ� ���ִ� ����
        _animator.SetBool(hashAttack, true);
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
        stateMachine.ChangeState<PlayerAttackState>();
    }

    public void OnExitAttackState()
    {
        stateMachine.ChangeState<PlayerIdleState>();
    }
}

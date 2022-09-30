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
    private int hashAttackIndex = Animator.StringToHash("AttackIndex");

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
        if(context.CurrentAttackBehavior.type != Define.AttackType.Skill_NoneTarget)
        {
            context.transform.rotation = Quaternion.LookRotation((context.Target.position - context.transform.position).normalized);
        }
        //Ʈ���Ÿ� Ű�� ���� ��Ʈ�ѷ� �̺�Ʈ�ڵ鷯�� �Լ� �߰�
        _attackStateController.enterAttackStateHandler += OnEnterAttackState;
        _attackStateController.exitAttackStateHandler += OnExitAttackState;
        //���� �ִϸ����Ϳ��� state�� �Ѿ�� �� Ʈ���Ÿ� ���ִ� ����
        _animator.SetBool(hashAttack, true);
        _animator.SetInteger(hashAttackIndex, context.CurrentAttackBehavior.AnimationIndex);
        context.isMove = false;
    }

    public override void Update(float deltaTime)
    {
        if(context.isMove)
        {
            _animator.SetBool(hashAttack, false);
            stateMachine.ChangeState<PlayerMoveState>();
        }
        if(context.Target != null && context.Target.GetComponent<IDamageable>()?.IsAlive == false)
        {
            stateMachine.ChangeState<PlayerIdleState>();
        }
        
    }
    public override void Exit()
    {
        //���� �� �߰� �ߴ� �Լ� ����(+�� ��ø�� ������ -�� ��ø�� �ȵǴ� ������ �ɾ�δ����� ����.)
        _attackStateController.enterAttackStateHandler -= OnEnterAttackState;
        _attackStateController.exitAttackStateHandler -= OnExitAttackState;
        _animator.SetBool(hashAttack, false);
    }

    public void OnEnterAttackState()
    {
    }

    public void OnExitAttackState()
    {
        _animator.SetBool(hashAttack, false);
        context.CurrentAttackBehavior.Ready = false;
    }
}

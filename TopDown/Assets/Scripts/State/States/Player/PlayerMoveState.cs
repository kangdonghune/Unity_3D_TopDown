using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveState : State<PlayerController>
{
    private Animator _animator;
    private NavMeshAgent _navAgent;
    private CharacterController _characterController;

    protected int hashMove = Animator.StringToHash("Move");
    protected int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _navAgent = context.GetComponent<NavMeshAgent>();
        _characterController = context.GetComponent<CharacterController>();
    }

    public override void Enter()
    {
        context.state = Define.PlayerState.Move;
        _animator.SetBool(hashMove, true);
        _animator.SetFloat(hashMoveSpeed, context.Stats.GetModifiedValue(Define.UnitAttribute.MoveSpeed));
    }

    public override void Update(float deltaTime)
    {
        if (context.CurrentAttackBehavior.type == Define.AttackType.Skill_NoneTarget && context.CurrentAttackBehavior.Ready)
        {
            stateMachine.ChangeState<PlayerAttackState>();
            return;
        }

        _animator.SetFloat(hashMoveSpeed, context.Stats.GetModifiedValue(Define.UnitAttribute.MoveSpeed) * 10);

        if (context.Target != null)
        {
            _navAgent.SetDestination(context.Target.position); //������Ʈ���� Ÿ���� �����Ǿ� ������ �ش� Ÿ������ ������ ����
        }
        if (_navAgent.remainingDistance > _navAgent.stoppingDistance)
        {
            //�̵��� ĳ������Ʈ�ѷ��� �̵�. �׺�޽� ����ӵ��� �÷��̾� �ӵ� * ��ŸŸ������ �̵�
            //�����ٵ� ���⿡ �߷´�� �׺��޽��� y������ ���� ����
            //�׺� �ӵ��� ĳ������Ʈ�ѷ��� �ӵ��� ������
            //����: �׺� �ӵ����� ĳ���� �ӵ��� ���� �� ��߳��� ���� �߻�
            //�׺� �ӵ��� 1000���� �ΰ� ĳ���� �ӵ��� 1�� ����Ʈ������ �ؼ� ������ ����
            _characterController.Move(_navAgent.desiredVelocity * context.Stats.GetModifiedValue(Define.UnitAttribute.MoveSpeed) * deltaTime);
            context.transform.position = new Vector3(context.transform.position.x, _navAgent.nextPosition.y, context.transform.position.z);
            _navAgent.velocity = _characterController.velocity;
        }
        else
        {
            context.transform.position = _navAgent.nextPosition;
            _navAgent.velocity = Vector3.zero;
            context.isMove = false;
            _animator.SetBool(hashMove, false);
            if (context.Target != null)
            {
                if (context.Target.GetComponent<IInteractable>() != null)
                {
                    IInteractable interactable = context.Target.GetComponent<IInteractable>();
                    interactable.Interact(context.gameObject);
                    stateMachine.ChangeState<PlayerIdleState>();
                    return;
                }
                else
                {
                    stateMachine.ChangeState<PlayerAttackState>();
                    return;
                }
            }
            stateMachine.ChangeState<PlayerIdleState>();
        }
    }
    public override void Exit()
    {
        _animator.SetBool(hashMove, false);
        //���̻� ��ã�� ���ϵ��� ����
        _navAgent.velocity = Vector3.zero;
        _navAgent.ResetPath();
    }
}

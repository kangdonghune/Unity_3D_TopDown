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
            _navAgent.SetDestination(context.Target.position); //업데이트마다 타겟이 지정되어 있으면 해당 타겟으로 목적지 지정
        }
        if (_navAgent.remainingDistance > _navAgent.stoppingDistance)
        {
            //이동은 캐릭터컨트롤러로 이동. 네비메쉬 희망속도에 플레이어 속도 * 델타타임으로 이동
            //리짓바디가 없기에 중력대신 네베메쉬의 y값으로 높이 고정
            //네비 속도는 캐릭터컨트롤러의 속도로 마무리
            //주의: 네비 속도보다 캐릭터 속도가 빠를 때 어긋나는 문제 발생
            //네비 속도를 1000으로 두고 캐릭터 속도를 1을 디펄트값으로 해서 가속을 조정
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
        //더이상 길찾기 안하도록 해제
        _navAgent.velocity = Vector3.zero;
        _navAgent.ResetPath();
    }
}

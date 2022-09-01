using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToWayPointState : State<BearController>
{
    private Animator _animator;
    private CharacterController _controller;
    private NavMeshAgent _agent;

    protected int hashMove = Animator.StringToHash("Move");
    protected int hashMoveSpeed = Animator.StringToHash("MoveSpeed");


    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _controller = context.GetComponent<CharacterController>();
        _agent = context.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        if(context.targetWayPoint == null)
            context.FindNextWayPoint();

        if(context.targetWayPoint)
        {
            _agent.SetDestination(context.targetWayPoint.position);
            _animator.SetBool(hashMove, true);
        }
    }

    public override void Update(float deltaTime)
    {
        Transform enemy = context.SearchEnemy();
        if (enemy)
        {
            if (context.IsAvailableAttack)
            {
                stateMachine.ChangeState<AttackState>();
            }
            else
            {
                stateMachine.ChangeState<MoveState>();
            }
        }
        else
        {
            //경로가 더 없거나 남은 거리가 정지 거리 이하일 경우
            if(!_agent.pathPending && (_agent.remainingDistance <= _agent.stoppingDistance))
            { 
                Transform nextDest = context.FindNextWayPoint();
                if(nextDest)
                {
                    _agent.SetDestination(nextDest.position);
                }
                stateMachine.ChangeState<IdleState>();
            }
            else
            {
                _controller.Move(_agent.velocity * Time.deltaTime * context.MoveSpeed);
                context.transform.position = _agent.nextPosition;
                _animator.SetFloat(hashMoveSpeed, _agent.velocity.magnitude / _agent.speed, .1f, Time.deltaTime);
            }
        }
    }

    public override void Exit()
    {
        _animator.SetBool(hashMove, false);
        _agent.ResetPath();
    }
}

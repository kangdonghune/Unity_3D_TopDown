using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : State<BearController>
{
    private Animator _animator;
    private NavMeshAgent _agent;
    private CharacterController _controller;

    protected int hashMove = Animator.StringToHash("Move");
    protected int hashMoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _agent = context.GetComponent<NavMeshAgent>();
        _controller = context.GetComponent<CharacterController>();
    }

    public override void Enter()
    {
        _agent.SetDestination(context.Target.position);
        _animator.SetBool(hashMove, true);
    }

    public override void Update(float deltaTime)
    {
        Transform enemy = context.SearchEnemy();
        if (enemy)
        {
            _agent.SetDestination(context.Target.position);
            if (_agent.remainingDistance > _agent.stoppingDistance)
            {
                _controller.Move(_agent.velocity * Time.deltaTime * context.MoveSpeed);
                context.transform.position = _agent.nextPosition;
                _animator.SetFloat(hashMoveSpeed, _agent.velocity.magnitude / _agent.speed, .1f, Time.deltaTime);
                return;
            }
        }
        stateMachine.ChangeState<IdleState>();

    }
    public override void Exit()
    {
        _animator.SetBool(hashMove, false);
        //더이상 길찾기 안하도록 해제
        _agent.velocity = Vector3.zero;
        _agent.ResetPath();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : State<BearController>
{
    private Animator _animator;
    private NavMeshAgent _agent;
    private CharacterController _controller;

    protected int hasMove = Animator.StringToHash("Move");
    protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");

    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _agent = context.GetComponent<NavMeshAgent>();
        _controller = context.GetComponent<CharacterController>();
    }

    public override void Enter()
    {
        _agent.SetDestination(context.target.position);
        _animator.SetBool(hasMove, true);
    }

    public override void Update(float deltaTime)
    {
        Transform enemy = context.SearchEnemy();
        if (enemy)
        {
            _agent.SetDestination(context.target.position);
            if (_agent.remainingDistance > _agent.stoppingDistance)
            {
                _controller.Move(_agent.velocity * deltaTime);
                //context.transform.position = new Vector3(context.transform.position.x, _agent.nextPosition.y, context.transform.position.z);
                _animator.SetFloat(hasMoveSpeed, _agent.velocity.magnitude / _agent.speed, 1f, deltaTime);
                return;
            }
        }

        stateMachine.ChangeState<IdleState>();

    }
    public override void Exit()
    {
        _animator.SetBool(hasMove, false);
        _animator.SetFloat(hasMoveSpeed, 0f);

        //더이상 길찾기 안하도록 해제
        _agent.ResetPath();
    }

}

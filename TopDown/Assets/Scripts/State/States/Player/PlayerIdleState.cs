using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerIdleState : State<PlayerController>
{
    private Animator _animator;
    private CharacterController _controller;
    private NavMeshAgent _agent;

    protected int hasMove = Animator.StringToHash("Move");
    protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");


    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _controller = context.GetComponent<CharacterController>();
        _agent = context.GetComponent<NavMeshAgent>();


    }

    public override void Enter()
    {
        _animator.SetBool(hasMove, false);
        _animator.SetFloat(hasMoveSpeed, 0f);
    }

    public override void Update(float deltaTime)
    {
        if (context.isMove == true)
            stateMachine.ChangeState<PlayerMoveState>();
    }

    public override void Exit()
    {
    }
}

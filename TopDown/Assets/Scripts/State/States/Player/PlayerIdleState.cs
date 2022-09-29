using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerIdleState : State<PlayerController>
{
    private Animator _animator;
    private CharacterController _controller;
    private NavMeshAgent _navAgent;

    protected int hasMove = Animator.StringToHash("Move");
    protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");


    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _controller = context.GetComponent<CharacterController>();
        _navAgent = context.GetComponent<NavMeshAgent>();


    }

    public override void Enter()
    {
        _animator.SetBool(hasMove, false);
        _animator.SetFloat(hasMoveSpeed, 0f);
    }

    public override void Update(float deltaTime)
    {
        if(context.CurrentAttackBehavior.type == Define.AttackType.Skill_NoneTarget && context.CurrentAttackBehavior.Ready)
            stateMachine.ChangeState<PlayerAttackState>();
        if (context.isMove == true)
            stateMachine.ChangeState<PlayerMoveState>();
    }

    public override void Exit()
    {
    }
}

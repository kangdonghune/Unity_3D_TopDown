using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerIdleState : State<PlayerController>
{
    private Animator _animator;
    private CharacterController _controller;
    private NavMeshAgent _navAgent;

    protected int hashIdle = Animator.StringToHash("Idle");
    protected int hashMove = Animator.StringToHash("Move");
    protected int hashMoveSpeed = Animator.StringToHash("MoveSpeed");


    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _controller = context.GetComponent<CharacterController>();
        _navAgent = context.GetComponent<NavMeshAgent>();


    }

    public override void Enter()
    {
        context.state = Define.PlayerState.Idle;
        _animator.SetBool(hashIdle, true);
        _animator.SetBool(hashMove, false);
        _animator.SetFloat(hashMoveSpeed, 0f);
    }

    public override void Update(float deltaTime)
    {
        if(context.CurrentAttackBehavior.type == Define.AttackType.Skill_NoneTarget && context.CurrentAttackBehavior.Ready)
        {
            stateMachine.ChangeState<PlayerAttackState>();
            return;
        }

        if (context.Target != null && context.isMove == false && context.Target.GetComponent<IInteractable>() == null)
        {
            stateMachine.ChangeState<PlayerAttackState>();
            return;
        }

        if (context.isMove == true)
        {
            stateMachine.ChangeState<PlayerMoveState>();
            return;
        }
    }

    public override void Exit()
    {
        _animator.SetBool(hashIdle, false);
    }
}

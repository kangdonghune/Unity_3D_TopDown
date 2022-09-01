using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<BearController>
{
    bool isPatrol = true;
    private float minIdleTime = 0.0f;
    private float maxIdleTime = 2.0f;
    private float idleTime = 0.0f;


    private Animator _animator;
    private CharacterController _controller;

    protected int hasMove = Animator.StringToHash("Move");
    protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");


    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _controller = context.GetComponent<CharacterController>();

    }

    public override void Enter()
    {
        _animator.SetBool(hasMove, false);
        _animator.SetFloat(hasMoveSpeed, 0f);
        _controller.Move(Vector3.zero);

        if (isPatrol)
        {
            idleTime = Random.Range(minIdleTime, maxIdleTime);
        }
    }

    public override void Update(float deltaTime)
    {
        Transform enemy = context.SearchEnemy();
        if(enemy)
        {
            if(context.IsAvailableAttack)
            {
                stateMachine.ChangeState<AttackState>();
            }
            else
            {
                stateMachine.ChangeState<MoveState>();
            }
        }

        else if (isPatrol && stateMachine.ElapsedTimeInState > idleTime)
        {
            stateMachine.ChangeState<MoveToWayPointState>();
        }
    }

    public override void Exit()
    {
    }
}

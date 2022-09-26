using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State<EnemyController>
{
    private Animator _animator;
    private CharacterController _controller;
    private NavMeshAgent _agent;
    private WayPoint _wayPoint;

    protected int hasMove = Animator.StringToHash("Move");
    protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");


    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _controller = context.GetComponent<CharacterController>();
        _agent = context.GetComponent<NavMeshAgent>();
        if(context.data.isPatrol)
            _wayPoint = context.gameObject.GetOrAddComponent<WayPoint>();
    }

    public override void Enter()
    {
        _animator.SetBool(hasMove, false);
        _animator.SetFloat(hasMoveSpeed, 0f);
        _controller.Move(Vector3.zero);

        if (context.data.isPatrol)
        {
            _wayPoint.idleTime = Random.Range(_wayPoint.MinIdleTime, _wayPoint.MaxIdleTime);
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

        else if (context.data.isPatrol && stateMachine.ElapsedTimeInState > _wayPoint.idleTime)
        {
            stateMachine.ChangeState<MoveToWayPointState>();
        }
    }

    public override void Exit()
    {
    }
}

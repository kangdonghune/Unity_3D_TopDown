using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonsterController
{
    private StateMachine<EnemyController> _stateMachine;
    public StateMachine<EnemyController> StateMachine { get { return _stateMachine; } }

    protected override void Init()
    {
        base.Init();
        WorldObjectType = Define.WorldObject.Monster;
        _stateMachine = new StateMachine<EnemyController>(this, new MoveToWayPointState());
        _stateMachine.AddState(new MoveState());
        _stateMachine.AddState(new AttackState());
        _stateMachine.AddState(new IdleState());
    }

    void Update()
    {
        _stateMachine.Update(Time.deltaTime);
    }

 

}

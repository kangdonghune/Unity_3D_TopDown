using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearController : FovMonsterController
{
    private StateMachine<BearController> _stateMachine;
    public StateMachine<BearController> StateMachine { get { return _stateMachine; } }

    protected override void Init()
    {
        base.Init();
        WorldObjectType = Define.WorldObject.Monster;
        _stateMachine = new StateMachine<BearController>(this, new MoveToWayPointState());
        _stateMachine.AddState(new IdleState());
        _stateMachine.AddState(new MoveState());
        _stateMachine.AddState(new AttackState());
    }

    void Update()
    {
        _stateMachine.Update(Time.deltaTime);
    }

 

}

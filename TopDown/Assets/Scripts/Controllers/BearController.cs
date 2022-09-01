using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearController : FovMonsterController
{
    private StateMachine<BearController> _stateMachine;
    public StateMachine<BearController> StateMachine { get { return _stateMachine; } }

    public Transform[] wayPoints;
    [HideInInspector]
    public Transform targetWayPoint = null;
    private int wayPointIndex = 0;

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

    public Transform FindNextWayPoint()
    {
        targetWayPoint = null;
        if(wayPoints.Length > 0)
        {
            targetWayPoint = wayPoints[wayPointIndex];
        }

        wayPointIndex = (wayPointIndex + 1) % wayPoints.Length;

        return targetWayPoint;

    }

}

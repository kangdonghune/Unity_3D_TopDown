using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : EnemyController
{
    #region Variable

    #endregion

    
    protected override void Init()
    {
        base.Init();
        _stateMachine = new StateMachine<EnemyController>(this, new IdleState());
        _stateMachine.AddState(new MoveState());
        _stateMachine.AddState(new AttackState());
        _stateMachine.AddState(new DeadState());
        if (Data.isPatrol)
        {
            SettingWayPoint();
            _stateMachine.AddState(new MoveToWayPointState());
        }
    }

    protected override void Update()
    {
        base.Update();
        _stateMachine.Update(Time.deltaTime);
    }


    #region GizMos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

 
    #endregion
}

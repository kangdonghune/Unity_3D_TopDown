using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : EnemyController, IPatrolable
{
    #region Variable

    #endregion


    protected override void Init()
    {
        base.Init();
        SettingWayPoint();
        _stateMachine = new StateMachine<EnemyController>(this, new IdleState());
        _stateMachine.AddState(new MoveToWayPointState());
        _stateMachine.AddState(new MoveState());
        _stateMachine.AddState(new AttackState());
        _stateMachine.AddState(new DeadState());
    }

    private void Update()
    {
        CheckAttackBehavior();
        _stateMachine.Update(Time.deltaTime);
    }

    #region interface
    public bool isPatrol { get; set; } = true;

    public void SettingWayPoint()
    {
        transform.gameObject.GetOrAddComponent<WayPoint>();//만약 waypoint컴퍼넌트 없으면 추가
    }
    #endregion

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

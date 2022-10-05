using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FovMonsterController : EnemyController
{
    #region Variable
    public override Transform Target { get { return _fov.NearestTarget; } }
    private FieldOfView _fov;
    private StatsObject _stats;
    #endregion

    internal override Transform SearchEnemy()
    {
        return Target;
    }


    protected override void Init()
    {
        base.Init();
        _fov = transform.gameObject.GetOrAddComponent<FieldOfView>();
        _stateMachine = new StateMachine<EnemyController>(this, new IdleState());
        _stateMachine.AddState(new MoveState());
        _stateMachine.AddState(new AttackState());
        _stateMachine.AddState(new DeadState());
        if (Data.isPatrol)
        {
            SettingWayPoint();
            _stateMachine.AddState(new MoveToWayPointState());
        }
        _stats = gameObject.GetComponent<EnemyController>().Stats;
    }

    protected override void Update()
    {
        base.Update();
        _stateMachine.Update(Time.deltaTime);
        if (_stats.HP < _stats.GetModifiedValue(Define.UnitAttribute.HP)) //피격 당한 상태라면
            _fov.ViewAngle = 360f;
    }
}

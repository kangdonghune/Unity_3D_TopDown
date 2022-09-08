using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FovMonsterController : EnemyController
{
    #region Variable
    public override Transform Target { get { return _fov.NearestTarget; } }
    private FieldOfView _fov;
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
    }

    private void Update()
    {
        CheckAttackBehavior();
        _stateMachine.Update(Time.deltaTime);
    }
}

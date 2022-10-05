using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyState : State<EnemyController>
{
    private StatsObject _stats;
    private Animator _animator;
    public int HashAwakeTrigger = Animator.StringToHash("AwakeTrigger");
    public override void Init()
    {
        _stats = context.GetComponent<EnemyController>().Stats;
        _animator = context.GetComponent<Animator>();
    }

    public override void Enter()
    {
      
    }

    public override void Update(float deltaTime)
    {
        if (_stats.HP < _stats.GetModifiedValue(Define.UnitAttribute.HP))
        {
            _animator.SetTrigger(HashAwakeTrigger);
            stateMachine.ChangeState<IdleState>();
        }


    }

    public override void Exit()
    {
    }
}

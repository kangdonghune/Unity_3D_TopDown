using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<EnemyController>
{
    private Animator _animator;

    public int isAliveHash = Animator.StringToHash("IsAlive");

    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
    }

    public override void Enter()
    {
        _animator.SetBool(isAliveHash, false);
    }

    public override void Update(float deltaTime)
    {
        if(stateMachine.ElapsedTimeInState > 3.0f)
        {
            Managers.Game.Despawn(context.gameObject);
        }
    }
    public override void Exit()
    {
    }
}

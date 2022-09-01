using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<BearController>
{
    private Animator _animator;

    protected int hashAttack = Animator.StringToHash("Attack");

    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
    }

    public override void Enter()
    {
        _animator.SetTrigger(hashAttack);
    }
    
    public override void Update(float deltaTime)
    {
    }
    public override void Exit()
    {
    }
  

}

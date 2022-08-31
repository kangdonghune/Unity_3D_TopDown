using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    #region Variable
    protected StateMachine<BearController> stateMachine;
    #endregion

    void Init()
    {
        stateMachine = new StateMachine<BearController>(this, new IdleState());
        stateMachine.AddState(new MoveState());
        stateMachine.AddState(new AttackState());
    }
    void Start()
    {
        Init();
    }

    void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }

    internal Transform SearchEnemy()
    {
        return transform;
    }
}

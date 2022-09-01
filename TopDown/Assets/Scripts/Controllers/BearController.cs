using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    #region Variable
    protected StateMachine<BearController> stateMachine;
    public LayerMask targetMask;
    private int _targetMask = 1 << (int)Define.Layer.Player;
    public Transform target;
    public float viewRadius;
    public float attackRange;

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

    public bool IsAvailableAttack
    {
        get
        {
            if (!target)
            {
                return false;
            }
            float distance = Vector3.Distance(transform.position, target.position);
            return (distance <= attackRange);
        }
    }

    internal Transform SearchEnemy()
    {
        target = null;

        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, _targetMask);
        if (targetInViewRadius.Length > 0)
        {
            target = targetInViewRadius[0].transform;
        }
        return target;
    }
}

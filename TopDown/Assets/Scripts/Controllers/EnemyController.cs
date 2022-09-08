using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : BaseController, IAttackable, IDamageable
{
    #region Variable

    public float MoveSpeed { get; protected set; } = 5f;
    public float viewRadius = 5f;
    public float attackRange => CurrentAttackBehavior?.range ?? 2f;


    public int maxHP = 100;
    public int hp;
    public virtual Transform Target { get; protected set; }
    protected StateMachine<EnemyController> _stateMachine;
    public StateMachine<EnemyController> StateMachine { get { return _stateMachine; } }
    protected Animator ani;
    protected NavMeshAgent agent;
    public Transform projectileTransform;
    public Transform hitTransform;
    protected int hashAttackTrigger = Animator.StringToHash("AttackTrigger");
    #endregion


    #region Virtual

    public virtual bool IsAvailableAttack
    {
        get
        {
            if (Target == null)
            {
                return false;
            }
            float distance = Vector3.Distance(transform.position, Target.position);
            return (distance < attackRange);
        }
    }

    internal virtual Transform SearchEnemy()
    {
        Target = null;

        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        if (targetInViewRadius.Length > 0)
        {
            Target = targetInViewRadius[0].transform;
        }
        return Target;
    }

    #endregion
    protected override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        targetMask = 1 << (int)Define.Layer.Player;
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        agent.updatePosition = false; //�̵��� ��Ʈ�ѷ��� ����
        agent.updateRotation = true; // ȸ���� �׺� �ϵ���
        InitAttackBehavior();
    }

    protected void InitAttackBehavior()
    {
        foreach (AttackBehavior behavior in attackBehaviors)
        {
            if (CurrentAttackBehavior == null)
                CurrentAttackBehavior = behavior;
            behavior.targetMask = targetMask;
        }
        
    }

    protected void CheckAttackBehavior()
    {
        if (CurrentAttackBehavior == null || !CurrentAttackBehavior.isAvailable)
            CurrentAttackBehavior = null;

        //���� behavior�� ��ȸ�ϸ� isAvailable�� ���� �� �� �켱���� ���� ���� ���� ���� ������ behavior�� ���� 
        foreach (AttackBehavior behavior in attackBehaviors)
        {
            if(behavior.isAvailable)
            {
                if (CurrentAttackBehavior == null || CurrentAttackBehavior.priority < behavior.priority)
                    CurrentAttackBehavior = behavior;
            }
        }
    }

    #region Interface

    public AttackBehavior CurrentAttackBehavior { get; private set; }

    public void OnExecuteAttack(int attackIndex)
    {
        if(CurrentAttackBehavior != null && Target != null)
        {
            CurrentAttackBehavior.ExecuteAttack(Target.gameObject, projectileTransform);
        }
    }

    public bool IsAlive => hp > 0;

    public void TakeDamage(int damage, GameObject hitEffectPrefabs)
    {
        if (!IsAlive)
            return;
        hp -= damage;

        if(hitEffectPrefabs)
        {
            //TODO-���� �����鿡 ��Ʈ����Ʈ �߰� �� �ش� �ڵ�� ����
            //Managers.Resource.Instantiate(hitEffectPrefabs-prefabname, hitTransform);
            Instantiate(hitEffectPrefabs, hitTransform);
        }

        if(IsAlive)
        {
            ani.SetTrigger(hashAttackTrigger);
        }
        else
        {
            StateMachine.ChangeState<DeadState>();
        }
    }
    #endregion


}

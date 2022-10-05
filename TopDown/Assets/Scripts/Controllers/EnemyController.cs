using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : BaseController, IAttackable, IDamageable, IPatrolable
{
    #region Variable

    public float MoveSpeed { get; protected set; } = 5f;
    public float viewRadius = 5f;
    public float attackRange => CurrentAttackBehavior?.Range ?? 2f;

    public virtual Transform Target { get; protected set; }
    protected StateMachine<EnemyController> _stateMachine;
    public StateMachine<EnemyController> StateMachine { get { return _stateMachine; } }
    protected Animator ani;
    protected NavMeshAgent agent;
    public Transform projectileTransform;
    public Transform hitTransform;
    public MonsterDatabase database;
    [HideInInspector]
    public MonsterObject Data { get; set; }
    private UI_UnitDefault _defalutUI;

    public Collider attackCollider;
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
        //가장 가까운 플레이어를 타겟으로
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        if (targetInViewRadius.Length > 0)
        {
            Target = targetInViewRadius[0].transform;
        }
        return Target;
    }

    #endregion

    protected override void AwakeInit()
    {
        UI_UnitDefault uI_origin = Resources.Load<UI_UnitDefault>("Prefab/UI/Unit/UI_UnitDefault");
        _defalutUI = Instantiate<UI_UnitDefault>(uI_origin, gameObject.transform);
    }

    protected override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        targetMask = 1 << (int)Define.Layer.Player;
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        agent.updatePosition = true; //이동은 컨트롤러가 시행
        agent.updateRotation = true; // 회전은 네비가 하도록
        InitAttackBehavior();
        foreach(MonsterObject monsterObject in database.MonsterObjects)
        {
            if (monsterObject.data.name == gameObject.tag)
            {
                StatsObject original = Resources.Load<StatsObject>($"Prefab/Data/Monster/Stat/{gameObject.tag}Stat");
                Data = Instantiate<MonsterObject>(monsterObject);
                Data.stats = Instantiate<StatsObject>(original);
                Data.stats.InitializeAttribute(Data.stats);
                Stats = Data.stats;
                break;
            }
        }
    }

    protected virtual void Update()
    {
        CheckAttackBehavior();
        _defalutUI.Value = Data.stats.HealthPercentage;
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

    public void CheckAttackBehavior()
    {
        if (CurrentAttackBehavior == null || !CurrentAttackBehavior.isAvailable)
            CurrentAttackBehavior = null;

        //어택 behavior를 순회하며 isAvailable이 참인 것 중 우선도가 가장 높은 것은 현재 실행할 behavior로 지정 
        foreach (AttackBehavior behavior in attackBehaviors)
        {
            if(behavior.isAvailable)
            {
                if (CurrentAttackBehavior == null || CurrentAttackBehavior.Priority < behavior.Priority)
                    CurrentAttackBehavior = behavior;
            }
        }
    }

    #region Interface

    public AttackBehavior CurrentAttackBehavior { get; private set; }

    public void OnExecuteAttack(GameObject targetObj)
    {
        if(CurrentAttackBehavior != null && Target != null)
        {
            CurrentAttackBehavior.ExecuteAttack(Target.gameObject, projectileTransform);
  
        }
    }

    public void OnAttackStart()
    {
        if (CurrentAttackBehavior != null)
        {
            CurrentAttackBehavior.AttackStart();
        }
    }

    public void OnAttackUpdate()
    {
        if (CurrentAttackBehavior != null)
        {
            CurrentAttackBehavior.AttackUpdate();
        }
    }

    public void OnAttackEnd()
    {
        if (CurrentAttackBehavior != null)
        {
            CurrentAttackBehavior.AttackEnd();
        }
    }

    public bool IsAlive => Data.stats.HP > 0;

 
    public void TakeDamage(int damage, GameObject hitEffectPrefabs, GameObject attacker)
    {
        if (!IsAlive)
            return;

        if (hitEffectPrefabs)
        {
            Managers.Effect.Instantiate(hitEffectPrefabs, hitTransform.position, Quaternion.identity);
        }

        Data.stats.AddHP(-damage);
        _defalutUI.CreateDamageText(damage);
        _defalutUI.Value = Data.stats.HealthPercentage;

        if (IsAlive == false)
        {
            attacker.GetComponent<BaseController>().Stats.AddExp(Data.stats.exp);
            StateMachine.ChangeState<DeadState>();
        }


    }

    public void SettingWayPoint()
    {
        transform.gameObject.GetOrAddComponent<WayPoint>();//만약 waypoint컴퍼넌트 없으면 추가
    }

    public void AddBuff(ItemBuff buff, float duration)
    {
        StartCoroutine(CoAddBuff(buff, duration));
    }


    #endregion


}

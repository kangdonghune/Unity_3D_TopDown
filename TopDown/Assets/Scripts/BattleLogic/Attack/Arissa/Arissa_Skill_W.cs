using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arissa_Skill_W : AttackBehavior
{
    //이미 공격을 받은 대상이면 중복 피해 방지
    private SortedSet<GameObject> _attacked = new SortedSet<GameObject>();

    public override void AttackStart()
    {
        gameObject.GetComponent<PlayerController>().attackCollider.enabled = true;
    }

    public override void AttackUpdate()
    {
        _attacked.Clear();//특정 주기로 맞은 애들 또 맞게 할 때는 맞은 애들 목록 초기화.
    }

    public override void AttackEnd()
    {
        calcCoolTime = 0f;
        Ready = false;
        _attacked.Clear();
        gameObject.GetComponent<PlayerController>().StateMachine.ChangeState<PlayerIdleState>();
        gameObject.GetComponent<PlayerController>().attackCollider.enabled = false;
    }

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        if (target != null)
        {
            if (_attacked.Contains(target))
                return;
            _attacked.Add(target);
            StatsObject attackStat = gameObject.GetComponent<BaseController>().Stats;
            StatsObject targetStat = target.GetComponent<BaseController>().Stats;
            int calcDamage = (int)(attackStat.GetModifiedValue(Define.UnitAttribute.Attack) - targetStat.GetModifiedValue(Define.UnitAttribute.Defence));
            calcDamage = calcDamage > 0 ? calcDamage : 0;
            int Damage = Value + calcDamage;
            target.GetComponent<IDamageable>()?.TakeDamage(Damage, effectPrefab, gameObject);
        }

    }

    protected override void Init()
    {
        AnimationIndex = (int)Define.PlayerAttackIndex.W;
        Priority = (int)Define.AttackPrioty.Firts;
        Ready = false;
        type = Define.AttackType.Skill_NoneTarget;
        Key = KeyCode.W;
        Value = 90;
        Range = 2f;
        Active = true;
        coolTime = 4f;
        calcCoolTime = 4f;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
    }
}

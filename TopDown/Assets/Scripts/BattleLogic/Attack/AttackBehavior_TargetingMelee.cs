using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior_TargetingMelee : AttackBehavior
{


    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        StatsObject attackStat = gameObject.GetComponent<BaseController>().Stats;
        StatsObject targetStat = target.GetComponent<BaseController>().Stats;
        int calcDamage = (int)(attackStat.GetModifiedValue(Define.UnitAttribute.Attack) - targetStat.GetModifiedValue(Define.UnitAttribute.Defence));
        calcDamage = calcDamage > 0 ? calcDamage : 0;
        int Damage = BaseDamage + calcDamage;

        target.GetComponent<IDamageable>()?.TakeDamage(Damage, effectPrefab, gameObject);
        calcCoolTime = 0f;
    }

    protected override void Init()
    {
        AnimationIndex = (int)Define.MonsterAttackPattern.Attack1;
        Priority = (int)Define.AttackPrioty.Fifth;
        BaseDamage = 5;
        Range = 2.5f;
        coolTime = 2f;
        calcCoolTime = coolTime;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
    }
}

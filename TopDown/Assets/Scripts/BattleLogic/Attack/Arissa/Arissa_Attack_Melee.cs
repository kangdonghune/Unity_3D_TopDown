using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arissa_Attack_Melee : AttackBehavior
{
    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        if (target != null)
        {
            StatsObject attackStat = gameObject.GetComponent<BaseController>().Stats;
            StatsObject targetStat = target.GetComponent<BaseController>().Stats;
            int calcDamage = (int)(attackStat.GetModifiedValue(Define.UnitAttribute.Attack) - targetStat.GetModifiedValue(Define.UnitAttribute.Defence));
            calcDamage = calcDamage > 0 ? calcDamage : 0;
            int Damage = BaseDamage + calcDamage;
            target.GetComponent<IDamageable>()?.TakeDamage(Damage, effectPrefab, gameObject);
        }

        calcCoolTime = 0f;
    }

    protected override void Init()
    {
        AnimationIndex = (int)Define.PlayerAttackIndex.Default;
        Priority = (int)Define.AttackPrioty.Fifth;
        Ready = true;
        BaseDamage = 10;
        Range = 2f;
        coolTime = 0f;
        calcCoolTime = 0f;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
    }
}

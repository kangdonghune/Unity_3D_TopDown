using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arissa_Skill_R : AttackBehavior
{
    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        if (target != null)
        {
            //StatsObject attackStat = gameObject.GetComponent<BaseController>().Stats;
            //StatsObject targetStat = target.GetComponent<BaseController>().Stats;
            //int calcDamage = (int)(attackStat.GetModifiedValue(Define.UnitAttribute.Attack) - targetStat.GetModifiedValue(Define.UnitAttribute.Defence));
            //calcDamage = calcDamage > 0 ? calcDamage : 0;
            //int Damage = BaseDamage + calcDamage;
            //target.GetComponent<IDamageable>()?.TakeDamage(Damage, effectPrefab, gameObject);
        }

        calcCoolTime = 0f;
    }

    protected override void Init()
    {
        AnimationIndex = (int)Define.PlayerAttackIndex.R;
        Priority = (int)Define.AttackPrioty.Firts;
        Ready = false;
        type = Define.AttackType.Skill_NoneTarget;
        Key = KeyCode.R;
        BaseDamage = 50;
        Range = 2f;
        coolTime = 0f;
        calcCoolTime = 0f;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAttackBehavior2 : AttackBehavior
{
    public override void AttackEnd()
    {
    }

    public override void AttackStart()
    {

    }

    public override void AttackUpdate()
    {
    }

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        if (target != null)
        {
            StatsObject attackStat = gameObject.GetComponent<BaseController>().Stats;
            StatsObject targetStat = target.GetComponent<BaseController>().Stats;
            int calcDamage = (int)(1.5f * attackStat.GetModifiedValue(Define.UnitAttribute.Attack) - targetStat.GetModifiedValue(Define.UnitAttribute.Defence));
            calcDamage = calcDamage > 0 ? calcDamage : 0;
            int Damage = Value + calcDamage;
            if (soundPrefab != null)
                Managers.Sound.Play(soundPrefab);
            target.GetComponent<IDamageable>()?.TakeDamage(Damage, effectPrefab, gameObject);
        }

        calcCoolTime = 0f;
    }

    protected override void Init()
    {
        AnimationIndex = (int)Define.MonsterAttackPattern.Attack2;
        Priority = (int)Define.AttackPrioty.Forth;
        Value = 10;
        Range = 2f;
        coolTime = 2f;
        calcCoolTime = 0f;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
    }
}

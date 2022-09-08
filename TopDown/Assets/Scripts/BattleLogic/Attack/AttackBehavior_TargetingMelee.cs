using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior_TargetingMelee : AttackBehavior
{


    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        target.GetComponent<IDamageable>()?.TakeDamage(Damage, effectPrefab);
        calcCoolTime = 0f;
    }

    protected override void Init()
    {
        AnimationIndex = (int)Define.MonsterAttackPattern.Attack1;
        Priority = (int)Define.AttackPrioty.Fifth;
        Damage = 5;
        Range = 2.5f;
        coolTime = 2f;
        calcCoolTime = coolTime;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
    }
}

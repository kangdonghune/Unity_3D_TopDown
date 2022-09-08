using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior_TargetingMelee : AttackBehavior
{


    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        target.GetComponent<IDamageable>()?.TakeDamage(damage, effectPrefab);
        calcCoolTime = 0f;
    }

    protected override void Init()
    {
        animationIndex = (int)Define.MonsterAttackPattern.Attack1;
        priority = (int)Define.AttackPrioty.Fifth;
        damage = 5;
        range = 2.5f;
        coolTime = 2f;
        calcCoolTime = 0f;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
    }
}

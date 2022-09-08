using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior_Melee : AttackBehavior
{
    public ManualCollision attackCollision;

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        Collider[] colliders = attackCollision.CheckOverlapBox(targetMask);

        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage, effectPrefab);
        }
        calcCoolTime = 0f;
    }

    protected override void Init()
    {
        animationIndex = (int)Define.MonsterAttackPattern.Attack1;
        priority = (int)Define.AttackPrioty.Fifth;
        damage = 5;
        range = 2f;
        coolTime = 1f;
        calcCoolTime = 0f;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior_TargetingMelee : AttackBehavior
{

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        target.GetComponent<IDamageable>()?.TakeDamage(damage, effectPrefab);
    }
}

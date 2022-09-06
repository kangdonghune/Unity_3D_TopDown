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
    }
}

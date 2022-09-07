 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior_Projectile : AttackBehavior
{
   

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        if (target == null)
            return;

        Vector3 projectilePosion = startPoint?.position ?? transform.position;
        if(effectPrefab)
        {
            GameObject projectileGo = GameObject.Instantiate<GameObject>(effectPrefab, projectilePosion, Quaternion.identity);
            projectileGo.transform.forward = transform.forward;

            Projectile projectile = projectileGo.GetComponent<Projectile>();
            if(projectile)
            {
                projectile.owner = this.gameObject;
                projectile.target = target;
                projectile.attackBehavior = this;
            }
        }

        calcCoolTime = 0.0f;
    }
}

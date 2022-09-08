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
            Vector3 dest = target.transform.position;
            dest.y += 1.5f;
            transform.LookAt(dest);
            projectileGo.transform.LookAt(dest);

          

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

    protected override void Init()
    {
        animationIndex = (int)Define.MonsterAttackPattern.Projectile;
        priority = (int)Define.AttackPrioty.Firts;
        damage = 10;
        range = 5f;
        coolTime = 3f;
        calcCoolTime = 0f;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
}
}

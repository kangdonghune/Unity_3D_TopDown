 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior_Projectile : AttackBehavior
{

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        if (target == null)
            return;

        Vector3 projectilePosition = startPoint?.position ?? transform.position;
        if(effectPrefab)
        {
            GameObject projectileGo = Managers.Resource.Instantiate(effectPrefab, projectilePosition, Quaternion.identity);
            projectileGo.transform.forward = transform.forward;
     
            Projectile projectile = projectileGo.GetComponent<Projectile>();
            if(projectile)
            {
                projectile.owner = this.gameObject;
                projectile.target = target;
                projectile.attackBehavior = this;
                if (projectile.collided)
                    projectile.Init();
            }
        }

        calcCoolTime = 0.0f;
    }

    protected override void Init()
    {
        AnimationIndex = (int)Define.MonsterAttackPattern.Projectile;
        Priority = (int)Define.AttackPrioty.Firts;
        Damage = 10;
        Range = 5f;
        coolTime = 10f;
        calcCoolTime = 0f;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
}
}

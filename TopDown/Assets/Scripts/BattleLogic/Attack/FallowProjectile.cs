using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallowProjectile : Projectile
{
    public float destroyDelay = 3f;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(CoDestroyProjectile(destroyDelay));

    }

    protected override void FixedUpdate()
    {
        if(target)
        {
            Vector3 dest = target.transform.position;
            dest.y += 1.5f;
            transform.LookAt(dest);
        }

        base.FixedUpdate();
    }
}

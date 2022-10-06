using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearBuffBehavior : AttackBehavior
{


    public override void AttackStart()
    {
        calcCoolTime = 0f;
        Ready = false;
        gameObject.GetComponent<EnemyController>().AddBuff(new ItemBuff(Define.UnitAttribute.AttackSpeed, 1f), 5f);
        if (soundPrefab != null)
            Managers.Sound.Play(soundPrefab);
    }

    public override void AttackUpdate()
    {

    }
    public override void AttackEnd()
    {
     }

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {

    }

    protected override void Init()
    {
        AnimationIndex = (int)Define.MonsterAttackPattern.Buff;
        Priority = (int)Define.AttackPrioty.Firts;
        Ready = false;
        Value = 10;
        Range = 2f;
        coolTime = 10000f;
        calcCoolTime = 10000f;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
    }
}

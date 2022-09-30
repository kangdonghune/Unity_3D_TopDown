using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arissa_Skill_R : AttackBehavior
{

    public override void AttackStart()
    {
        //gameObject.GetComponent<PlayerController>().attackCollider.enabled = true;
    }

    public override void AttackUpdate()
    {
    }

    public override void AttackEnd()
    {
        calcCoolTime = 0f;
        Ready = false;
        gameObject.GetComponent<PlayerController>().StateMachine.ChangeState<PlayerIdleState>();
        //gameObject.GetComponent<PlayerController>().attackCollider.enabled = false;
    }

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        if (target != null)
        {
            StatsObject attackStat = gameObject.GetComponent<BaseController>().Stats;
            StatsObject targetStat = target.GetComponent<BaseController>().Stats;
            int calcDamage = (int)(attackStat.GetModifiedValue(Define.UnitAttribute.Attack) - targetStat.GetModifiedValue(Define.UnitAttribute.Defence));
            calcDamage = calcDamage > 0 ? calcDamage : 0;
            int Damage = Value + calcDamage;
            target.GetComponent<IDamageable>()?.TakeDamage(Damage, effectPrefab, gameObject);
        }


    }

    protected override void Init()
    {
        AnimationIndex = (int)Define.PlayerAttackIndex.R;
        Priority = (int)Define.AttackPrioty.Firts;
        Ready = false;
        type = Define.AttackType.Skill_Target;
        Key = KeyCode.R;
        Value = 50;
        Range = 2f;
        coolTime = 10f;
        calcCoolTime = 10f;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
    }
}

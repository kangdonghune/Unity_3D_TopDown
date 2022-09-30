using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arissa_Skill_E : AttackBehavior
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
        gameObject.GetComponent<PlayerController>().AddBuff(new ItemBuff(Define.UnitAttribute.AttackSpeed, 1f), 5f);
        //gameObject.GetComponent<PlayerController>().attackCollider.enabled = false;
    }

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        
    }

    protected override void Init()
    {
        AnimationIndex = (int)Define.PlayerAttackIndex.E;
        Priority = (int)Define.AttackPrioty.Firts;
        Ready = false;
        type = Define.AttackType.Skill_NoneTarget;
        Key = KeyCode.E;
        Value = 50;
        Range = 2f;
        Active = false;
        coolTime = 8f;
        calcCoolTime = 8f;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
    }
}

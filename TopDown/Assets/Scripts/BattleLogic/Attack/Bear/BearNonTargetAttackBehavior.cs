using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearNonTargetAttackBehavior : AttackBehavior
{
    //�̹� ������ ���� ����̸� �ߺ� ���� ����
    private SortedSet<GameObject> _attacked = new SortedSet<GameObject>();

    public override void AttackStart()
    {
        gameObject.GetComponent<EnemyController>().attackCollider.enabled = true;
    }

    public override void AttackUpdate()
    {
        _attacked.Clear();//Ư�� �ֱ�� ���� �ֵ� �� �°� �� ���� ���� �ֵ� ��� �ʱ�ȭ.
    }

    public override void AttackEnd()
    {
        calcCoolTime = 0f;
        gameObject.GetComponent<EnemyController>().attackCollider.enabled = false;
        _attacked.Clear();
    }

    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        if (target != null)
        {
            if (_attacked.Contains(target))
                return;
            _attacked.Add(target);
            StatsObject attackStat = gameObject.GetComponent<BaseController>().Stats;
            StatsObject targetStat = target.GetComponent<BaseController>().Stats;
            int calcDamage = (int)(2 * attackStat.GetModifiedValue(Define.UnitAttribute.Attack) - targetStat.GetModifiedValue(Define.UnitAttribute.Defence));
            calcDamage = calcDamage > 0 ? calcDamage : 0;
            int Damage = Value + calcDamage;
            if (soundPrefab != null)
                Managers.Sound.Play(soundPrefab);
            target.GetComponent<IDamageable>()?.TakeDamage(Damage, effectPrefab, gameObject);
        }

    }

    protected override void Init()
    {
        AnimationIndex = (int)Define.MonsterAttackPattern.NonTarget;
        Priority = (int)Define.AttackPrioty.Firts;
        Ready = false;
        Value = 80;
        Range = 2f;
        coolTime = 8f;
        calcCoolTime = 8f;
        targetMask = gameObject.GetComponent<BaseController>().targetMask;
    }
}

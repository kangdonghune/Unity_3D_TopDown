using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;
    public int targetMask;
    [SerializeField]
    public List<AttackBehavior> attackBehaviors = new List<AttackBehavior>();

    [SerializeField]
    public List<AttackBehavior> SkillBehaviors = new List<AttackBehavior>();

    //Stat
    [HideInInspector]
    public StatsObject Stats { get; protected set; }

    public void Awake()
    {
        AttackBehaviorSet();
        AwakeInit();
    }

    public void Start()
    {
        Init();
    }

    protected abstract void Init();
    protected abstract void AwakeInit();


    protected virtual void AttackBehaviorSet()
    {   //���� �� ���ӿ�����Ʈ�� �ִ� ��� ���� ������ �ڵ����� ����Ʈ�� �߰�
        AttackBehavior[] behaviors = GetComponents<AttackBehavior>();
        if (behaviors == null)
            return;
        foreach (AttackBehavior behavior in behaviors)
        {
            attackBehaviors.Add(behavior); //�⺻������ ��ų�̵� ��Ÿ�� ���غ���� �ְ�
            if (behavior.type == Define.AttackType.Skill_Target || behavior.type == Define.AttackType.Skill_NoneTarget)
            {
                SkillBehaviors.Add(behavior); //��ų�� ������ ��ų ����� �߰��� ������.
            }
        }
           
    }

    public void ReserveDestroy(float time)
    {
        StartCoroutine(CoDestroy(time));
    }

    public IEnumerator CoDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Managers.Resource.Destroy(gameObject);
    }

    public IEnumerator CoAddBuff(ItemBuff buff, float duration)
    {
        foreach (Attribute attribute in Stats.attributes)
        {
            if (attribute.type == buff.stat)
            {
                attribute.value.AddModifier(buff);
                yield return new WaitForSeconds(duration);
                attribute.value.RemoveModifier(buff);
            }
        }
    }

 
    protected IEnumerator CoAddHp(ItemBuff buff, int amount, float duration)
    {
        int count = amount;
        if (buff.stat != Define.UnitAttribute.HP)
            yield break;
        while (count > 0)
        {
            Stats.AddHP(buff.value/ amount);
            yield return new WaitForSeconds(duration / amount);
            count--;
        }
    }

    protected IEnumerator CoAddMana(ItemBuff buff, int amount, float duration)
    {
        int count = amount;
        if (buff.stat != Define.UnitAttribute.Mana)
            yield break;
        while (count > 0)
        {
            Stats.AddMana(buff.value / amount);
            yield return new WaitForSeconds(duration / amount);
            count--;
        }
    }
}

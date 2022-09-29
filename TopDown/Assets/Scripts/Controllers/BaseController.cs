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
        AwakeInit();
    }

    public void Start()
    {
        AttackBehaviorSet();
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
}

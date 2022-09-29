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
    {   //시작 시 게임오브젝트에 있는 모든 어택 비헤비어 자동으로 리스트에 추가
        AttackBehavior[] behaviors = GetComponents<AttackBehavior>();
        if (behaviors == null)
            return;
        foreach (AttackBehavior behavior in behaviors)
        {
            attackBehaviors.Add(behavior); //기본적으로 스킬이든 평타든 어텍비헤비어에 넣고
            if (behavior.type == Define.AttackType.Skill_Target || behavior.type == Define.AttackType.Skill_NoneTarget)
            {
                SkillBehaviors.Add(behavior); //스킬만 별도로 스킬 비헤비어에 추가로 모은다.
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

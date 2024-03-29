using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehavior : MonoBehaviour
{
    #region Variable

#if UNITY_EDITOR
    [Multiline]
    public string developmentDescription = "특정 하나의 공격 동작을 다루는 컴퍼넌트. 공격의 쿨타임,데미지, 애니메이션 인덱스 등";
#endif 

    public int AnimationIndex { get; protected set; }
    public int Priority { get; protected set; } //동시에 여러 공격이 가능할 때 공격 순서의 우선도.
    public int Value { get; protected set; } = 10;
    public float Range { get; protected set; } = 2f;
    public bool isAvailable => calcCoolTime >= coolTime;
    public float RemainCoolTime => coolTime - calcCoolTime > 0 ? coolTime - calcCoolTime : 0;
    public bool Ready { get; set; } = true;
    public Define.AttackType type { get; protected set; } = Define.AttackType.Default;
    protected float coolTime = 0f;
    protected float calcCoolTime = 0f;
    public KeyCode Key { get; protected set; } = KeyCode.None;
    public bool Active = true;

    public GameObject effectPrefab;
    public AudioClip soundPrefab;
    public int targetMask;



    #endregion

    protected abstract void Init();

    void Awake()
    {
        Init();
    }

    void Update()
    {
        if(calcCoolTime < coolTime)
        {
            calcCoolTime += Time.deltaTime;

        }

    }

    //타겟이 없는 범위 공격을 고려 default null, startPoint는 투사체 등의 생성 위치
    public abstract void ExecuteAttack(GameObject target = null,Transform startPoint = null);

    public abstract void AttackUpdate();
    public abstract void AttackStart();
    public abstract void AttackEnd();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehavior : MonoBehaviour
{
    #region Variable

#if UNITY_EDITOR
    [Multiline]
    public string developmentDescription = "";
#endif 

    public int animationIndex;
    public int priority; //동시에 여러 공격이 가능할 때 공격 순서의 우선도.
    public int damage = 10;
    public float range = 5f;

    protected float coolTime;
    protected float calcCoolTime = 0f;

    public GameObject effectPrefab;
    public int targetMask;



#endregion
    void Start()
    {
        calcCoolTime = coolTime;
    }

    void Update()
    {
        if(calcCoolTime < coolTime)
        {
            calcCoolTime += Time.deltaTime;
        }

    }

    //타겟이 없는 범위 공격을 고려 default null, startPoint는 투사체 등의 생성 위치
    public abstract void ExecuteAttakc(GameObject target = null, Transform startPoint = null);
}

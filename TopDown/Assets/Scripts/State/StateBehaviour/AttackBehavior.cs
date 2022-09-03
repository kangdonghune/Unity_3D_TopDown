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
    public int priority; //���ÿ� ���� ������ ������ �� ���� ������ �켱��.
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

    //Ÿ���� ���� ���� ������ ��� default null, startPoint�� ����ü ���� ���� ��ġ
    public abstract void ExecuteAttakc(GameObject target = null, Transform startPoint = null);
}

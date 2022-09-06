using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehavior : MonoBehaviour
{
    #region Variable

#if UNITY_EDITOR
    [Multiline]
    public string developmentDescription = "Ư�� �ϳ��� ���� ������ �ٷ�� ���۳�Ʈ. ������ ��Ÿ��,������, �ִϸ��̼� �ε��� ��";
#endif 

    public int animationIndex;
    public int priority; //���ÿ� ���� ������ ������ �� ���� ������ �켱��.
    public int damage = 10;
    public float range = 2f;
    public bool isAvailable = false;


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
            return;
        }
        calcCoolTime = 0f;
        isAvailable = true;
    }

    //Ÿ���� ���� ���� ������ ��� default null, startPoint�� ����ü ���� ���� ��ġ
    public abstract void ExecuteAttack(GameObject target = null, Transform startPoint = null);
}

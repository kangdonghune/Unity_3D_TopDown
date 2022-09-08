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

    public int AnimationIndex { get; protected set; }
    public int Priority { get; protected set; } //���ÿ� ���� ������ ������ �� ���� ������ �켱��.
    public int Damage { get; protected set; } = 10;
    public float Range { get; protected set; } = 2f;
    public bool isAvailable => calcCoolTime >= coolTime;


    protected float coolTime = 0f;
    protected float calcCoolTime = 0f;

    public GameObject effectPrefab;
    public int targetMask;



    #endregion

    protected abstract void Init();

    void Start()
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

    //Ÿ���� ���� ���� ������ ��� default null, startPoint�� ����ü ���� ���� ��ġ
    public abstract void ExecuteAttack(GameObject target = null, Transform startPoint = null);
}

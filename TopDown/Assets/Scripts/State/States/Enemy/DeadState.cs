using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<EnemyController>
{
    private Animator _animator;

    public int isAliveHash = Animator.StringToHash("IsAlive");

    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
    }

    public override void Enter()
    {
        _animator.SetBool(isAliveHash, false);
        context.ReserveDestroy(10f); //���� ����
        context.gameObject.AddComponent<ItemBox>();//��ü ���ÿ� �����۹ڽ� ����
        context.enabled = false; //��Ʈ�ѷ� ��Ȱ��ȭ ���� �� Ȱ��ȭ
    }

    public override void Update(float deltaTime)
    {
    }
    public override void Exit()
    {
    }

  
}

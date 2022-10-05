using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<EnemyController>
{
    private Animator _animator;

    public int HashDead = Animator.StringToHash("isDead");

    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
    }

    public override void Enter()
    {
        _animator.SetBool(HashDead, true);
        context.ReserveDestroy(30f); //���� ����
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

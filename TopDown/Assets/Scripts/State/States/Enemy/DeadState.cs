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
        context.ReserveDestroy(10f); //삭제 예약
        context.gameObject.AddComponent<ItemBox>();//시체 루팅용 아이템박스 생성
        context.enabled = false; //컨트롤러 비활성화 스폰 시 활성화
    }

    public override void Update(float deltaTime)
    {
    }
    public override void Exit()
    {
    }

  
}

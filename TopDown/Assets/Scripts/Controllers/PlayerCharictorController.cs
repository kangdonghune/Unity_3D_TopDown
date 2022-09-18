using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCharictorController : PlayerController
{
    #region Variable

    #endregion

    protected override void Init()
    {
        base.Init();
        _stateMachine = new StateMachine<PlayerController>(this, new PlayerIdleState());
        _stateMachine.AddState(new PlayerMoveState());
        _stateMachine.AddState(new PlayerAttackState());
    }

    private void Update()
    {
        _isOnUI = EventSystem.current.IsPointerOverGameObject();
        _unitUI.Value = _hp;
        _stateMachine.Update(Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateController : MonoBehaviour
{
    public delegate void OnEnterAttackState();
    public delegate void OnExitAttackState();

    public OnEnterAttackState enterAttackStateHandler;
    public OnExitAttackState exitAttackStateHandler;

    public bool IsInAttackState { get; private set; }

    void Start()
    {
        enterAttackStateHandler = new OnEnterAttackState(EnterAttackState);
        exitAttackStateHandler = new OnExitAttackState(ExitAttackState);
    }

    #region Helper Method
     
    public void OnStartOfAttackState()
    {
        IsInAttackState = true;
        enterAttackStateHandler.Invoke();
    }

    public void OnEndOfAttackState()
    {
        IsInAttackState = false;
        exitAttackStateHandler.Invoke();
    }


    private void EnterAttackState()
    {

    }

    private void ExitAttackState()
    {

    }

    //애니메이터에서 해당 이름으로 이벤트 삽입하고 해당 타이밍에 이 함수 호출
    public void OnCheckAttackCollider(GameObject target)
    {
        GetComponent<IAttackable>().OnExecuteAttack(target);
    }
    #endregion

}

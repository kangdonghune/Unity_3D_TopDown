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

    public void OnCheckAttackCollider(int attackIndex)
    {
        GetComponent<IAttackable>().OnExecuteAttack(attackIndex);
    }
    #endregion

}

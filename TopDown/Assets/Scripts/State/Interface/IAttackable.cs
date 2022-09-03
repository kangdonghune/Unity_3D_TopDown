using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    AttackBehavior CurrentAttackBehavior { get; }

    void OnExecuteAttack(int attackIndex);
}

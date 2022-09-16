using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterAttribute
{
    HP,
    Mana,
    Attack,
    AttackSpeed,
    Defence,
    MoveSpeed,
}

[Serializable]
public class ItemBuff
{
    #region Variable
    public CharacterAttribute stat;
    public float value;



    public ItemBuff(float value)
    {
        this.value = value;
    }

    public void AddValue(ref float refValue)
    {
        refValue += value;
    }

    #endregion
}

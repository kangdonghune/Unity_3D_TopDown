using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterAttribute
{
    Agility,
    Intellect,
    Stamina,
    Strength,
}

[Serializable]
public class ItemBuff
{
    #region Variable
    public CharacterAttribute stat;
    public int value;

    [SerializeField]
    private int min;
    [SerializeField]
    private int max;

    public int Min => min;
    public int Max => max;

    public ItemBuff(int min, int max)
    {
        this.min = min;
        this.max = max;

        InitValue();
    }

    private void InitValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }

    public void AddValue(ref int refValue)
    {
        refValue += value;
    }

    #endregion
}

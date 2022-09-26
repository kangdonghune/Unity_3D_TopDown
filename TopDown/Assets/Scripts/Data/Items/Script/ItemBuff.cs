using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class ItemBuff : IModifier
{
    #region Variable
    public Define.UnitAttribute stat;
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

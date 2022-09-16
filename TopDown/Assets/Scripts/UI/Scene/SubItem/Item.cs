using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item 
{
    public int id = -1;
    public string name;

    public ItemBuff[] buffs;

    public Item()
    {
        id = -1; //-1은 비어있는 아이템
        name = "";
    }

    public Item(ItemObject itemObject)
    {
        name = itemObject.name;
        id = itemObject.data.id;

        buffs = new ItemBuff[itemObject.data.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(itemObject.data.buffs[i].value) { 
                        stat = itemObject.data.buffs[i].stat};
        }
    }
}

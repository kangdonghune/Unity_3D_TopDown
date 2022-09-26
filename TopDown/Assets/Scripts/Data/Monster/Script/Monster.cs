using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Monster
{
    public int id;
    public string name;

    

    public Monster()
    {
        id = -1;
        name = "";
    }

    public Monster(MonsterObject monsterObject)
    {
        id = monsterObject.data.id;
        name = monsterObject.name;
    }
}

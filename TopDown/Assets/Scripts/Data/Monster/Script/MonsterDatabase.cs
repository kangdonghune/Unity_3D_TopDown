using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MonsterDatabase", menuName = "Unit System/Monster/Database")]
public class MonsterDatabase : ScriptableObject
{
    public MonsterObject[] MonsterObjects;

    public void OnValidate()
    {
        for (int i = 0; i < MonsterObjects.Length; ++i)
        {
            if (MonsterObjects[i] == null)
            {
                continue;
            }
            MonsterObjects[i].data.id = i;
        }
    }
}

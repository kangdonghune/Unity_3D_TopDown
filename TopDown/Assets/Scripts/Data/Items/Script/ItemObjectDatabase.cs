using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemObjectDatabase", menuName = "Inventory System/Items/Database")]
public class ItemObjectDatabase : ScriptableObject
{
    public ItemObject[] itemObjects;

    public void OnValidate()
    {
        for (int i = 0; i < itemObjects.Length; ++i)
        {
            if (itemObjects[i] == null)
            {
                continue;
            }
            itemObjects[i].data.id = i;
        }
    }
}

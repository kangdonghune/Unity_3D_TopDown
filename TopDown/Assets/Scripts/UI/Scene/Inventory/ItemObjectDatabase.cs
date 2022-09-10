using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemObjectDatabase", menuName = "Inventory System/Items/Database")]
public class ItemObjectDatabase : ScriptableObject
{
    public ItemObject[] itemObjects;

    public void OnValidate()
    {
        if (itemObjects.Length == 0)
            return;
        for(int i = 0; i < itemObjects.Length; i++)
        {
            itemObjects[i].data.id = i; 
        }
    }
}

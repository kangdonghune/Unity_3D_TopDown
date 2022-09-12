using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryObjectDatabase : ScriptableObject
{
    public InventoryObject[] invenObjects;

    public void OnValidate()
    {
        for (int i = 0; i < invenObjects.Length; i++)
        {
        }
    }
}

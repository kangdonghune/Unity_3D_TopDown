using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New InventoryDatabase", menuName = "Inventory Sysyem/Inventory")]
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

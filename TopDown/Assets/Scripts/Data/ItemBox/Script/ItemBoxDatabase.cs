using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemBoxDatabase", menuName = "Inventory System/ItemBox/New ItemBoxDatabase")]
public class ItemBoxDatabase : ScriptableObject
{
    public ItemBoxContents[] Contents;
}

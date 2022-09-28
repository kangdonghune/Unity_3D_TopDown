using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemBoxContents", menuName = "Inventory System/ItemBox/New ItemBoxContents")]
public class ItemBoxContents : ScriptableObject
{
    public Define.Tag tag;

    [SerializeField]
    public ItemBoxContent[] items = new ItemBoxContent[10];

}

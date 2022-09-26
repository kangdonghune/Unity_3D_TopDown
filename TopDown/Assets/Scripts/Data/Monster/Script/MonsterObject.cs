using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Unit System/Monster/New Monster")]
public class MonsterObject : ScriptableObject
{
    public StatsObject stats;
    public Monster data = new Monster();
    public bool isPatrol;

    [TextArea(15, 20)]
    public string description;
}

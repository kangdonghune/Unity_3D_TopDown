using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrolable
{
    bool isPatrol { get; set; }

    void SettingWayPoint();

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public List<Transform> WayPoints { get; private set; }
    public Transform targetWayPoint = null;
    private int wayPointIndex = 0;

    public float MinIdleTime { get; private set; } = 0.0f;
    public float MaxIdleTime { get; private set; } = 2.0f;
    public float idleTime = 0.0f;

    //현재는 해당 컴퍼넌트를 소유한 게임오브젝트와 way프리팹의 이름을 연동해서 찾지만 
    //경우에 따라 way별로 이름을 짓고 해당 이름을 enum에 저장 후 enum에서 원하는 way를 찾아서 연결하는 방식도 고려
    enum WayPointEnum
    {
        Bear,

    }

    void Init()
    {
        //컴퍼넌트 소유 오브젝트의 이름으로 WayPointManager가 생성한 prefab 목록을 탐색한다.
        string objName = transform.gameObject.name + "_Way";
        WayPoints = Managers.Way.GetWays(objName);
    }

    void Start()
    {
        Init();
    }

    public Transform FindNextWayPoint()
    {
        targetWayPoint = null;
        if (WayPoints == null || WayPoints.Count == 0)
            return targetWayPoint;
        if (WayPoints.Count > 0)
        {
            targetWayPoint = WayPoints[wayPointIndex];
        }

        wayPointIndex = (wayPointIndex + 1) % WayPoints.Count;

        return targetWayPoint;

    }


}

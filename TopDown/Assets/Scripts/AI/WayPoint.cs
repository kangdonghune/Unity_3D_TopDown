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

    //����� �ش� ���۳�Ʈ�� ������ ���ӿ�����Ʈ�� way�������� �̸��� �����ؼ� ã���� 
    //��쿡 ���� way���� �̸��� ���� �ش� �̸��� enum�� ���� �� enum���� ���ϴ� way�� ã�Ƽ� �����ϴ� ��ĵ� ���
    enum WayPointEnum
    {
        Bear,

    }

    void Init()
    {
        //���۳�Ʈ ���� ������Ʈ�� �̸����� WayPointManager�� ������ prefab ����� Ž���Ѵ�.
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

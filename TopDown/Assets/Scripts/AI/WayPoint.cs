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

    private int wayDir = 1;

    //����� �ش� ���۳�Ʈ�� ������ ���ӿ�����Ʈ�� way�������� �̸��� �����ؼ� ã���� 
    //��쿡 ���� way���� �̸��� ���� �ش� �̸��� enum�� ���� �� enum���� ���ϴ� way�� ã�Ƽ� �����ϴ� ��ĵ� ���

    void Init()
    {
        //���۳�Ʈ ���� ������Ʈ�� �̸����� WayPointManager�� ������ prefab ����� Ž���Ѵ�.
        string objName = transform.gameObject.name + "_Way";
        Managers.Way.AddWayables(objName);
        WayPoints = Managers.Way.GetWays(objName);
        SetFirstWayPoint();
    }

    void Start()
    {
        Init();
    }

    public void SetFirstWayPoint()
    {
        float nearDistance = float.MaxValue;

        for (int i = 0; i< WayPoints.Count; i++)
        {
            float distance = (WayPoints[i].position - transform.position).magnitude;
            if(distance < nearDistance)
            {
                nearDistance = distance;
                wayPointIndex = i;
            }
        }
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

        if (wayPointIndex + 1 * wayDir == WayPoints.Count || wayPointIndex + 1 * wayDir < 0)
            wayDir *= -1;

        wayPointIndex = (wayPointIndex + 1 * wayDir) % WayPoints.Count;
        return targetWayPoint;

    }


}

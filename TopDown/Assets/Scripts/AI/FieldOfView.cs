using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 5f;
    [Range(0, 360)]
    public float viewAngle = 90f;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    private List<Transform> visiableTagets = new List<Transform>();
    private Transform nearestTarget;

    private float distanceToTarget = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        FindVisialbeTargets();
    }

    void FindVisialbeTargets()
    {
        distanceToTarget = 0f;
        nearestTarget = null;
        visiableTagets.Clear();

        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        
        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2)
            {
                float distToTarget = Vector3.Distance(target.position, transform.position);
                if(!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visiableTagets.Add(target);
                    if (nearestTarget == null || (distanceToTarget > distToTarget))
                    {
                        nearestTarget = target;
                        distanceToTarget = distToTarget;
                    }
                }
            }

        }
    }
}

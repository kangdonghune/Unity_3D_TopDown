using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float ViewRadius { get; set; } = 5f;
    private float _viewAngle = 90f;
    private float _delay = 0.2f;

    public float ViewAngle
    {
        get { return _viewAngle; }
    }

    private int targetMask = (1 << (int)Define.Layer.Player);
    private int obstacleMask = (1 << (int)Define.Layer.Wall);

    private List<Transform> _visiableTagets = new List<Transform>();
    public List<Transform> VisiableTagets { get { return _visiableTagets; } }
    
    private Transform _nearestTarget;
    public Transform NearestTarget { get { return _nearestTarget; }}

    private float _distanceToTarget = 0f;

    void Start()
    {
        StartCoroutine("FindTargetwithDelay", _delay);
    }

    void Update()
    {
        
    }

    IEnumerator FindTargetwithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisialbeTargets();
        }

    }

    void FindVisialbeTargets()
    {
        _distanceToTarget = 0f;
        _nearestTarget = null;
        _visiableTagets.Clear();

        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, _viewAngle, targetMask);
        
        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < _viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(target.position, transform.position);
                if(!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    _visiableTagets.Add(target);
                    if (_nearestTarget == null || (_distanceToTarget > distToTarget))
                    {
                        _nearestTarget = target;
                        _distanceToTarget = distToTarget;
                    }
                }
            }

        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        float x = Mathf.Sin(angleInDegrees * Mathf.Deg2Rad);
        float z = Mathf.Cos(angleInDegrees * Mathf.Deg2Rad);
        return new Vector3(x, 0, z);

    }
}

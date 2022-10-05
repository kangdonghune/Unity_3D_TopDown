using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float ViewRadius { get; set; } = 5f;
    private float _viewAngle = 120f;
    private float _delay = 0.2f;

    public float ViewAngle
    {
        get { return _viewAngle; }
        set { _viewAngle = value; }
    }

    private int _targetMask = (1 << (int)Define.Layer.Player);
    private int _obstacleMask = (1 << (int)Define.Layer.Wall);

    private List<Transform> _visiableTagets = new List<Transform>();
    public List<Transform> VisiableTagets { get { return _visiableTagets; } }
    
    private Transform _nearestTarget;
    public Transform NearestTarget { get { return _nearestTarget; }}

    private float _distanceToTarget = 0f;

    void Start()
    {
        StartCoroutine("FindTargetwithDelay", _delay);
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
        //범위 내 타겟 마스크 레이어의 충돌체 탐색
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, _targetMask);
        
        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            //순회하며 view 앵글 안에 있는 지 체크(forward 기준으로 체크하기 때문에 실제 시야각도의 절반을 해줘야 앵글체크 정상)
            if(Vector3.Angle(transform.forward, dirToTarget) < _viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(target.position, transform.position);
                //앵글과 거리가 둘 다 범위 안이라면 장애물 마스크와 레이캐스트 후 통과 시 추가
                if(!Physics.Raycast(transform.position, dirToTarget, distToTarget, _obstacleMask))
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

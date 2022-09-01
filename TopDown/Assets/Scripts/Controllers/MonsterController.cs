using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    #region Variable

    private int _targetMask = 1 << (int)Define.Layer.Player;
    public Transform target;
    public float viewRadius = 5f;
    public float attackRange = 1.5f;

    public bool IsAvailableAttack
    {
        get
        {
            if (target == null)
            {
                return false;
            }
            float distance = Vector3.Distance(transform.position, target.position);
            return (distance <= attackRange);
        }
    }
    #endregion

    internal Transform SearchEnemy()
    {
        target = null;

        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, _targetMask);
        if (targetInViewRadius.Length > 0)
        {
            target = targetInViewRadius[0].transform;
        }
        return target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected override void Init()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackRange;
        agent.updatePosition = false; //이동은 컨트롤러가 시행
        agent.updateRotation = true; // 회전은 네비가 하도록
    }
}

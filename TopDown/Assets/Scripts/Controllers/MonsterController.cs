using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    #region Variable
    public float MoveSpeed { get; protected set; } = 5f;
    private int _targetMask = 1 << (int)Define.Layer.Player;
    public Transform Target { get; private set; }
    public float viewRadius = 5f;
    public float attackRange = 1.5f;
    public bool isPatrol = false;

    public bool IsAvailableAttack
    {
        get
        {
            if (Target == null)
            {
                return false;
            }
            float distance = Vector3.Distance(transform.position, Target.position);
            return (distance <= attackRange);
        }
    }
    #endregion

    internal Transform SearchEnemy()
    {
        Target = null;

        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, _targetMask);
        if (targetInViewRadius.Length > 0)
        {
            Target = targetInViewRadius[0].transform;
        }
        return Target;
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

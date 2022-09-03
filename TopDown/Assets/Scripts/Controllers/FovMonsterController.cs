using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FovMonsterController : BaseController
{
    #region Variable

    public float MoveSpeed { get; protected set; } = 5f;
    public float AttackRange { get; private set; } = 1.5f;
    public Transform Target { get { return _fov.NearestTarget; } }
    private FieldOfView _fov;
    public bool IsAvailableAttack
    {
        get
        {
            if (Target == null)
            {
                return false;
            }
            float distance = Vector3.Distance(transform.position, Target.position);
            return (distance <= AttackRange);
        }
    }
    #endregion

    internal Transform SearchEnemy()
    {
        return Target;
    }


    protected override void Init()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        _fov = transform.gameObject.GetOrAddComponent<FieldOfView>();
        agent.stoppingDistance = AttackRange;
        agent.updatePosition = false; //�̵��� ��Ʈ�ѷ��� ����
        agent.updateRotation = true; // ȸ���� �׺� �ϵ���
    }
}

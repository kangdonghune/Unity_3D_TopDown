using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FovMonsterController : BaseController
{
    #region Variable

    public float MoveSpeed { get; protected set; } = 5f;
    private float _attackRange = 3f;
    public Transform Target { get {return _fov.NearestTarget; } }

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
            return (distance <= _attackRange);
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
        agent.stoppingDistance = _attackRange;
        agent.updatePosition = false; //이동은 컨트롤러가 시행
        agent.updateRotation = true; // 회전은 네비가 하도록
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCraftState : State<PlayerController>
{
    private Animator _animator;
    private NavMeshAgent _navAgent;
    protected int hashCraft = Animator.StringToHash("Craft");

    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _navAgent = context.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        context.state = Define.PlayerState.Craft;
        _animator.SetBool(hashCraft, true);
        context.attackCollider?.transform.parent.parent.gameObject.SetActive(false);
    }

    public override void Update(float deltaTime)
    {
        _navAgent.velocity = Vector3.zero;
        _navAgent.ResetPath();
    }

    public override void Exit()
    {
        context.attackCollider?.transform.parent.parent.gameObject.SetActive(true);
        _animator.SetBool(hashCraft, false);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttackState : State<PlayerController>
{
    private Animator _animator;
    private NavMeshAgent _navAgent;
    private CharacterController _characterController;
    private AttackStateController _attackStateController;
    private IAttackable _attackable;
    private Vector3 _lookDir;

    private int hashAttack = Animator.StringToHash("Attack");
    private int hashAttackIndex = Animator.StringToHash("AttackIndex");

    public override void Init()
    {
        _animator = context.GetComponent<Animator>();
        _navAgent = context.GetComponent<NavMeshAgent>();
        _characterController = context.GetComponent<CharacterController>();
        _attackStateController = context.GetComponent<AttackStateController>();
        _attackable = context.GetComponent<IAttackable>();
    }

    public override void Enter()
    {
        context.CheckAttackBehavior();
        if (_attackable.CurrentAttackBehavior == null)
        {
            stateMachine.ChangeState<PlayerIdleState>();
            return;
        }
        if(context.CurrentAttackBehavior.type != Define.AttackType.Skill_NoneTarget)
        {
            context.transform.rotation = Quaternion.LookRotation((context.Target.position - context.transform.position).normalized);
        }
        //트리거를 키기 전에 컨트롤러 이벤트핸들러에 함수 추가
        _attackStateController.enterAttackStateHandler += OnEnterAttackState;
        _attackStateController.exitAttackStateHandler += OnExitAttackState;
        //실제 애니메이터에서 state로 넘어가는 건 트리거를 켜주는 순간
        _animator.SetBool(hashAttack, true);
        _animator.SetInteger(hashAttackIndex, context.CurrentAttackBehavior.AnimationIndex);
        context.isMove = false;
    }

    public override void Update(float deltaTime)
    {
        if(context.isMove)
        {
            _animator.SetBool(hashAttack, false);
            stateMachine.ChangeState<PlayerMoveState>();
        }
        if(context.Target != null && context.Target.GetComponent<IDamageable>()?.IsAlive == false)
        {
            stateMachine.ChangeState<PlayerIdleState>();
        }
        
    }
    public override void Exit()
    {
        //퇴장 시 추가 했던 함수 제거(+는 중첩이 되지만 -는 중첩이 안되니 안전상 걸어두는편이 좋다.)
        _attackStateController.enterAttackStateHandler -= OnEnterAttackState;
        _attackStateController.exitAttackStateHandler -= OnExitAttackState;
        _animator.SetBool(hashAttack, false);
    }

    public void OnEnterAttackState()
    {
    }

    public void OnExitAttackState()
    {
        _animator.SetBool(hashAttack, false);
        context.CurrentAttackBehavior.Ready = false;
    }
}

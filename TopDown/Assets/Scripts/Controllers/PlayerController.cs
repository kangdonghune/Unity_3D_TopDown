using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    #region Variable
    public LayerMask groundLayerMask;
    public float groundCheckDistance = 0.3f;

    private CharacterController _characterController;
    private NavMeshAgent _navAgent;
    private Camera _camera;
    private Animator _animator;

    readonly int moveHash = Animator.StringToHash("Move");



    #endregion

    protected override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _characterController = GetComponent<CharacterController>();
        _navAgent = GetComponent<NavMeshAgent>();
        _navAgent.updatePosition = false; //이동은 컨트롤러가 시행
        _navAgent.updateRotation = true; // 회전은 네비가 하도록
        _camera = Camera.main;
        _animator = GetComponent<Animator>();

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    void Update()
    {
        PlayerMove();
    }

    private void LateUpdate()
    {
    }

    private void OnMouseEvent(Define.MouseEvent evt)
    {
        switch(evt)
        {
            case Define.MouseEvent.LClick:
                OnMousePicking();
                break;
            case Define.MouseEvent.LPointerDown:
                break;
            case Define.MouseEvent.LPointerUp:
                break;
            case Define.MouseEvent.LPress:
                break;
            case Define.MouseEvent.RClick:
                OnMousePicking();
                break;
            case Define.MouseEvent.RPointerDown:
                break;
            case Define.MouseEvent.RPointerUp:
                break;
            case Define.MouseEvent.RPress:
                break;
        }
    }

    private void OnMousePicking()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, groundLayerMask))
        {
            Debug.Log("We hit " + hit.collider.name + " " + hit.point);
            _navAgent.SetDestination(hit.point);
        }
    }

    private void PlayerMove()
    {
        if (_navAgent.remainingDistance > _navAgent.stoppingDistance)
        {
            _characterController.Move(_navAgent.velocity * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, _navAgent.nextPosition.y, transform.position.z);
            _animator.SetBool(moveHash, true);
        }
        else
        {
            _characterController.Move(Vector3.zero);
            transform.position = new Vector3(transform.position.x, _navAgent.nextPosition.y, transform.position.z);
            _animator.SetBool(moveHash, false);
        }
    }
}

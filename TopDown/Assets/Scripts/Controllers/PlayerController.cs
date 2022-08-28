using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    #region Variable
    public LayerMask groundLayerMask;
    public float groundCheckDistance = 0.3f;

    private CharacterController _characterController;
    private NavMeshAgent _navAgent;
    private Camera camera;
    private bool _isGrounded = false;
    private Vector3 calcVelocity;



    #endregion

    private void Init()
    {
        _characterController = GetComponent<CharacterController>();
        _navAgent = GetComponent<NavMeshAgent>();
        _navAgent.updatePosition = false; //�̵��� ��Ʈ�ѷ��� ����
        _navAgent.updateRotation = true; // ȸ���� �׺� �ϵ���
        camera = Camera.main;

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        PlayerMove();
    }

    private void LateUpdate()
    {
        transform.position = _navAgent.nextPosition;
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
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
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
            _characterController.SimpleMove(_navAgent.velocity);
        }
        else
        {
            _characterController.Move(Vector3.zero);
        }
    }
}

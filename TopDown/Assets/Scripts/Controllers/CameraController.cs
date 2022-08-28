using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    public Define.CameraMode _mode = Define.CameraMode.QuarterView;
    [SerializeField]
    public GameObject _target;

    public Vector3 _delta = new Vector3();

    private Vector3[] CameraModePos = { new Vector3(),new Vector3(0.0f, 6.0f, -5.0f) };

    public float height = 5f;
    public float lookAtHeight = 2f;
    public float distance = 10f;
    public float angle = 45f;
    public float smoothSpeed = 0.5f;
    private Vector3 refVelocity;


    void Init()
    {
        _target = GameObject.Find("Player");
        if (_target == null)
            Debug.LogWarning("CameraController Searching Player Failed!");

        Managers.Input.KeyAction -= SetCameraMode;
        Managers.Input.KeyAction += SetCameraMode;

        _mode = Define.CameraMode.QuarterView;
    }

    void Start()
    {
        Init();

    }

    void LateUpdate()
    {
        if (_target.IsValid() == false)
            return;

        CameraSetting(_mode); // default 위치 지정
        CameraRayCast(_mode); // 카메라와 플레이어 레이캐스팅 후 위치 조정
    }

    public void SetTarget(GameObject obj)
    {
        _target = obj;
    }

    private void SetCameraMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _mode = Define.CameraMode.TopView;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            _mode = Define.CameraMode.QuarterView;

    }

    private void HandleCamera()
    {
        Vector3 worldPositon = (Vector3.forward * -distance) + (Vector3.up * height);
        Debug.DrawLine(_target.transform.position, worldPositon, Color.black);

        Vector3 rotateVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPositon;
        Debug.DrawLine(_target.transform.position, rotateVector, Color.green);


        Vector3 target = _target.transform.position;
        target.y += lookAtHeight;
     

        Vector3 finalPosition = target + rotateVector;
        Debug.DrawLine(_target.transform.position, finalPosition, Color.blue);

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        transform.LookAt(target);

    }

    void CameraSetting(Define.CameraMode mode)
    {
        switch (mode)
        {

            case Define.CameraMode.TopView:
                HandleCamera();
                break;

            case Define.CameraMode.QuarterView:
                _delta = CameraModePos[(int)mode];
                transform.position = _target.transform.position + _delta + Vector3.up * 2.0f;
                Vector3 target = _target.transform.position;
                target.y += lookAtHeight;
                transform.LookAt(target);
                break;

            default:
                _delta = CameraModePos[(int)Define.CameraMode.QuarterView];
                transform.position = _target.transform.position + _delta + Vector3.up * 2.0f;
                transform.LookAt(_target.transform);
                break;
        }
    }
    
    void CameraRayCast(Define.CameraMode mode)
    {
        if (mode != Define.CameraMode.QuarterView)
            return;

        //1. 플레이어와 카메라 사이에 물체 여부 확인
        //2. 플레이어 외에 ray hit가 발생하다면 hit point와 플레이어 사이의 70퍼 위치에 카메라 위치 이동.

        Vector3 dir = _target.transform.position - transform.position;
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Block");
        Debug.DrawRay(transform.position, dir.normalized * dir.magnitude, Color.green);
        if (Physics.Raycast(transform.position, dir.normalized, out hit, dir.magnitude, mask))
        {
            Vector3 hitDir = hit.point - _target.transform.position;
            float dist = hitDir.magnitude;
            transform.position = _target.transform.position + hitDir.normalized * dist * 0.7f;
            CameraRayCast(mode);
        }

        else
            return;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0, 0, 0.5f);
        if(_target.IsValid())
        {
            Vector3 lookAtPosition = _target.transform.position;
            lookAtPosition.y += lookAtHeight;
            Gizmos.DrawLine(transform.position, lookAtPosition);
            Gizmos.DrawSphere(lookAtPosition, 0.25f);
        }
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}

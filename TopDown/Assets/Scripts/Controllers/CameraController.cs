using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public struct CameraValue
    {
        public float Distance;
        public float Height;
        public float Angle;
        public float LookAtHeight;
        public bool isNotNull;
        public float smoothSpeed;
    }

    [SerializeField]
    public Define.CameraMode mode = Define.CameraMode.TopView;

    private Define.CameraMode oldMode = Define.CameraMode.TopView;
    
    [SerializeField]
    public GameObject target;

    public CameraValue[] CameraValues = new CameraValue[(int)Define.CameraMode.End]; 

    public float distance = 10f;
    public float height = 5f;
    public float lookAtHeight = 1f;
    public float angle = 45f;
    public float smoothSpeed = 0.5f;
    private Vector3 refVelocity;


    private Vector3 _lookTargetPosition;
    private bool _isSmooth = true;

    void Init()
    {
        target = GameObject.Find("Player");
        if (target == null)
            Debug.LogWarning("CameraController Searching Player Failed!");

        Managers.Input.KeyAction -= SetCameraMode;
        Managers.Input.KeyAction += SetCameraMode;
        CameraValueSave(mode);
        DefaultCameraVaule();
    }

    void Start()
    {
        Init();

    }

    void LateUpdate()
    {
        if (target.IsValid() == false)
            return;

        _lookTargetPosition = target.transform.position;
        _lookTargetPosition.y += lookAtHeight;

        CameraSetting(mode); // default 위치 지정
        CameraRayCast(mode); // 카메라와 플레이어 레이캐스팅 후 위치 조정
    }

    public void SetTarget(GameObject obj)
    {
        target = obj;
    }

    private void SetCameraMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(mode != Define.CameraMode.TopView)
            {
                oldMode = mode;
                mode = Define.CameraMode.TopView;
                CameraValueSave(oldMode);
                CameraValueLoad(mode);
                _isSmooth = true;
            }
        }    
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (mode != Define.CameraMode.QuarterView)
            {
                oldMode = mode;
                mode = Define.CameraMode.QuarterView;
                CameraValueSave(oldMode);
                CameraValueLoad(mode);
            }
        }

    }

    public void HandleTopViewCamera()
    {
        CameraValueSave(mode);

        Vector3 worldPositon = (Vector3.forward * -CameraValues[(int)mode].Distance) + (Vector3.up * CameraValues[(int)mode].Height);
        Debug.DrawLine(target.transform.position, worldPositon, Color.black);

        Vector3 rotateVector = Quaternion.AngleAxis(CameraValues[(int)mode].Angle, Vector3.up) * worldPositon;
        Debug.DrawLine(target.transform.position, rotateVector, Color.green);
     

        Vector3 finalPosition = _lookTargetPosition + rotateVector;
        Debug.DrawLine(target.transform.position, finalPosition, Color.blue);

        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        transform.LookAt(_lookTargetPosition);
    }

    public void HandleQuarterViewViewCamera()
    {
        CameraValueSave(mode);

        Vector3 worldPositon = (Vector3.forward * -CameraValues[(int)mode].Distance) + (Vector3.up * CameraValues[(int)mode].Height);
        if (_isSmooth)
        {
            Vector3 finalPosition = target.transform.position + worldPositon;
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        }
        else
        {
            transform.position = target.transform.position + worldPositon;
        }

        transform.LookAt(_lookTargetPosition);
    }

    void DefaultCameraVaule()
    {
        if(mode != Define.CameraMode.TopView)
        {
            CameraValues[(int)Define.CameraMode.TopView].Distance = 6f;
            CameraValues[(int)Define.CameraMode.TopView].Height = 5f;
            CameraValues[(int)Define.CameraMode.TopView].Angle = 45.0f;
            CameraValues[(int)Define.CameraMode.TopView].LookAtHeight = 2f;
            CameraValues[(int)Define.CameraMode.TopView].smoothSpeed = 0.5f;
        }

        if (mode != Define.CameraMode.QuarterView)
        {
            CameraValues[(int)Define.CameraMode.QuarterView].Distance = 8f;
            CameraValues[(int)Define.CameraMode.QuarterView].Height = 8f;
            CameraValues[(int)Define.CameraMode.QuarterView].Angle = 0f;
            CameraValues[(int)Define.CameraMode.QuarterView].LookAtHeight = 1f;
            CameraValues[(int)Define.CameraMode.QuarterView].smoothSpeed = 0.5f;
        }
    }

    public void CameraValueSave(Define.CameraMode mode)
    {
        CameraValues[(int)mode].Distance = distance;
        CameraValues[(int)mode].Height = height;
        CameraValues[(int)mode].Angle = angle;
        CameraValues[(int)mode].LookAtHeight = lookAtHeight;

    }

    public void CameraValueLoad(Define.CameraMode mode)
    {
        distance = CameraValues[(int)mode].Distance;
        height = CameraValues[(int)mode].Height;
        angle = CameraValues[(int)mode].Angle;
        lookAtHeight = CameraValues[(int)mode].LookAtHeight;

    }

    public void CameraSetting(Define.CameraMode mode)
    {
        switch (mode)
        {
            case Define.CameraMode.TopView:
                HandleTopViewCamera();
                break;
            case Define.CameraMode.QuarterView:
                HandleQuarterViewViewCamera();
                break;
            default:
                HandleTopViewCamera();
                break;
        }
    }
    
    void CameraRayCast(Define.CameraMode mode)
    {
        if (mode != Define.CameraMode.QuarterView)
            return;

        //1. 플레이어와 카메라 사이에 물체 여부 확인
        //2. 플레이어 외에 ray hit가 발생하다면 hit point와 플레이어 사이의 70퍼 위치에 카메라 위치 이동.

        Vector3 dir = _lookTargetPosition - transform.position;
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Wall");
        
        Debug.DrawRay(transform.position, dir.normalized * dir.magnitude, Color.green);
        if (Physics.Raycast(transform.position, dir.normalized, out hit, dir.magnitude, mask))
        {
            Vector3 hitDir = hit.point - _lookTargetPosition;
            float dist = hitDir.magnitude;
            transform.position = target.transform.position + hitDir.normalized * dist * 0.7f;
            CameraRayCast(mode);
            _isSmooth = false;
        }

        else
        {
            _isSmooth = true;
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0, 0, 0.5f);
        if(target.IsValid())
        {
            Vector3 lookAtPosition = target.transform.position;
            lookAtPosition.y += lookAtHeight;
            Gizmos.DrawLine(transform.position, lookAtPosition);
            Gizmos.DrawSphere(lookAtPosition, 0.25f);
        }
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}

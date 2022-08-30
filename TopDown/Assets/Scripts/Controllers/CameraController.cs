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
        public float SmoothSpeed;

        public CameraValue(float Distance, float Height, float Angle, float LookAtHeight, float SmoothSpeed)
        {
            this.Distance = Distance;
            this.Height = Height;
            this.Angle = Angle;
            this.LookAtHeight = LookAtHeight;
            this.SmoothSpeed = SmoothSpeed;
        }
    }

    [SerializeField]
    public Define.CameraMode mode = Define.CameraMode.TopView;
    
    [SerializeField]
    public GameObject target;

    public CameraValue[] CameraValues = new CameraValue[(int)Define.CameraMode.End]
                                        {new CameraValue(6f, 5f,45f,1.5f, 0.2f),
                                         new CameraValue(8f, 8f,0f, 1f, 0.2f),
                                          new CameraValue()}; 

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
        //에디터에서 값을 넣어주고자 할 땐 넣어주고
        if (mode == Define.CameraMode.None || mode == Define.CameraMode.End)
            mode = Define.CameraMode.TopView;
        //아니면 디펄트 값을 이용하고 모드는 탑 뷰로
        else
            CameraValueSave(mode);

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
                CameraValueSave(mode);
                mode = Define.CameraMode.TopView;
                CameraValueLoad(mode);
                _isSmooth = true;
            }
        }    
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (mode != Define.CameraMode.QuarterView)
            {
                CameraValueSave(mode);
                mode = Define.CameraMode.QuarterView;
                CameraValueLoad(mode);
                _isSmooth = true;
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

        float cameraMoveDist = (finalPosition - transform.position).magnitude;
        if (cameraMoveDist < 0.01f)
            _isSmooth = false;

        if (_isSmooth)
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        else
            transform.position = finalPosition;

       transform.LookAt(_lookTargetPosition);
    }

    public void HandleQuarterViewViewCamera()
    {
        CameraValueSave(mode);

        Vector3 worldPositon = (Vector3.forward * -CameraValues[(int)mode].Distance) + (Vector3.up * CameraValues[(int)mode].Height);
        Vector3 finalPosition = target.transform.position + worldPositon;
       
        float cameraMoveDist = (finalPosition - transform.position).magnitude;
        if (cameraMoveDist < 0.01f)
            _isSmooth = false;

        if (_isSmooth)
        {
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
        }
        else
        {
            transform.position = target.transform.position + worldPositon;
        }

        transform.LookAt(_lookTargetPosition);
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

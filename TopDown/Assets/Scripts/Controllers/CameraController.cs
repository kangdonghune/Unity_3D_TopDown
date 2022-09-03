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

    public GameObject Target { get; private set; }

    public CameraValue[] CameraValues = new CameraValue[(int)Define.CameraMode.End]
                                        {new CameraValue(6f, 5f,45f,1.5f, 0.8f),
                                         new CameraValue(8f, 8f,0f, 1f, 0.8f),
                                          new CameraValue()}; 

    public float distance = 10f;
    public float height = 5f;
    public float lookAtHeight = 1f;
    public float angle = 45f;
    public float smoothSpeed = 0.5f;
    private Vector3 refVelocity;

    private bool _isSmooth = true;

    void Init()
    {
        Target = GameObject.Find("Arisa");
        if (Target == null)
            Debug.LogWarning("CameraController Searching Player Failed!");

        Managers.Input.KeyAction -= SetCameraMode;
        Managers.Input.KeyAction += SetCameraMode;
        //에디터에서 값을 넣어주고자 할 땐 넣어주고
        if (mode == Define.CameraMode.None || mode == Define.CameraMode.End)
        {
            mode = Define.CameraMode.TopView;
            CameraValueLoad(mode);
        }
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
        if (Target.IsValid() == false)
            return;

        CameraSetting(mode); // default 위치 지정
        CameraRayCast(mode); // 카메라와 플레이어 레이캐스팅 후 위치 조정
    }

    public void SetTarget(GameObject obj)
    {
        Target = obj;
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
        Debug.DrawLine(Target.transform.position, worldPositon, Color.black);

        Vector3 rotateVector = Quaternion.AngleAxis(CameraValues[(int)mode].Angle, Vector3.up) * worldPositon;
        Debug.DrawLine(Target.transform.position, rotateVector, Color.green);


        Vector3 _lookTargetPosition = Target.transform.position;
        _lookTargetPosition.y += lookAtHeight;

        Vector3 finalPosition = _lookTargetPosition + rotateVector;
        Debug.DrawLine(Target.transform.position, finalPosition, Color.blue);

      
        
        if(_isSmooth == false)
        {
           transform.position = finalPosition;
        }
        else
        {
            float cameraMoveDist = (finalPosition - transform.position).magnitude;
            if (cameraMoveDist < 0.1f)
            {
                transform.position = finalPosition;
                _isSmooth = false;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, CameraValues[(int)mode].SmoothSpeed);

            }

        }

        transform.LookAt(_lookTargetPosition);
    }

    public void HandleQuarterViewViewCamera()
    {
        CameraValueSave(mode);

        Vector3 worldPositon = (Vector3.forward * -CameraValues[(int)mode].Distance) + (Vector3.up * CameraValues[(int)mode].Height);
        Vector3 finalPosition = Target.transform.position + worldPositon;
       
        float cameraMoveDist = (finalPosition - transform.position).magnitude;
        if (cameraMoveDist < 0.1f)
            _isSmooth = false;

        if (_isSmooth)
        {
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, CameraValues[(int)mode].SmoothSpeed);
        }
        else
        {
            transform.position = Target.transform.position + worldPositon;
        }

        Vector3  _lookTargetPosition = Target.transform.position;
        _lookTargetPosition.y += lookAtHeight;
        transform.LookAt(_lookTargetPosition);
    }


    public void CameraValueSave(Define.CameraMode mode)
    {
        CameraValues[(int)mode].Distance = distance;
        CameraValues[(int)mode].Height = height;
        CameraValues[(int)mode].Angle = angle;
        CameraValues[(int)mode].LookAtHeight = lookAtHeight;
        CameraValues[(int)mode].SmoothSpeed = smoothSpeed;

    }

    public void CameraValueLoad(Define.CameraMode mode)
    {
        distance = CameraValues[(int)mode].Distance;
        height = CameraValues[(int)mode].Height;
        angle = CameraValues[(int)mode].Angle;
        lookAtHeight = CameraValues[(int)mode].LookAtHeight;
        smoothSpeed = CameraValues[(int)mode].SmoothSpeed;
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

        Vector3 _lookTargetPosition = Target.transform.position;
        _lookTargetPosition.y += lookAtHeight;

        Vector3 dir = _lookTargetPosition - transform.position;
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Wall");
        
        Debug.DrawRay(transform.position, dir.normalized * dir.magnitude, Color.green);
        if (Physics.Raycast(transform.position, dir.normalized, out hit, dir.magnitude, mask))
        {
            Vector3 hitDir = hit.point - _lookTargetPosition;
            float dist = hitDir.magnitude;
            transform.position = Target.transform.position + hitDir.normalized * dist * 0.7f;
            _isSmooth = false;
            CameraRayCast(mode);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0, 0, 0.5f);
        if(Target.IsValid())
        {
            Vector3 lookAtPosition = Target.transform.position;
            lookAtPosition.y += lookAtHeight;
            Gizmos.DrawLine(transform.position, lookAtPosition);
            Gizmos.DrawSphere(lookAtPosition, 0.25f);
        }
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}

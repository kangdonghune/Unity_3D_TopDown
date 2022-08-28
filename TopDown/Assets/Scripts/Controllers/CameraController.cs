using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    private Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f);

    [SerializeField]
    private GameObject _player { get; set; } = null;

    //private Vector3[] CameraModePos = { new Vector3(0.0f, 6.0f, -5.0f) };

    void Init()
    {
        _player = GameObject.Find("Player");
        if (_player == null)
            Debug.LogWarning("CameraController Searching Player Failed!");
    }

    void Start()
    {
        Init();
    }

    void LateUpdate()
    {
        if (_player.IsValid() == false)
            return;

        CameraSetting(_mode); // default 위치 지정
        CameraRayCast(_mode); // 카메라와 플레이어 레이캐스팅 후 위치 조정
    }


    void CameraSetting(Define.CameraMode mode)
    {
        switch (mode)
        {
            case Define.CameraMode.QuarterView:
                //_delta = CameraModePos[(int)mode];
                transform.position = _player.transform.position + _delta + Vector3.up * 2.0f;
                transform.LookAt(_player.transform);
                break;

            default:
                //_delta = CameraModePos[(int)Define.CameraMode.QuarterView];
                transform.position = _player.transform.position + _delta + Vector3.up * 2.0f;
                transform.LookAt(_player.transform);
                break;
        }
    }
    
    void CameraRayCast(Define.CameraMode mode)
    {
        if (mode != Define.CameraMode.QuarterView)
            return;

        //1. 플레이어와 카메라 사이에 물체 여부 확인
        //2. 플레이어 외에 ray hit가 발생하다면 hit point와 플레이어 사이의 70퍼 위치에 카메라 위치 이동.

        Vector3 dir = _player.transform.position - transform.position;
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Block");
        Debug.DrawRay(transform.position, dir.normalized * dir.magnitude, Color.green);
        if (Physics.Raycast(transform.position, dir.normalized, out hit, dir.magnitude, mask))
        {
            Vector3 hitDir = hit.point - _player.transform.position;
            float dist = hitDir.magnitude;
            transform.position = _player.transform.position + hitDir.normalized * dist * 0.7f;
            CameraRayCast(mode);
        }

        else
            return;
    }

}

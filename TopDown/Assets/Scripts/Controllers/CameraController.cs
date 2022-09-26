using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public Define.CameraMode mode = Define.CameraMode.QuarterView;

    public GameObject Target { get; private set; }
    private List<GameObject> _enabledObjects = new List<GameObject>();

    public float distance = 10f;
    public float height = 5f;
    public float lookAtHeight = 1f;
    public float angle = 45f;
    public float smoothSpeed = 0.5f;
    private Vector3 refVelocity;

    private bool _isSmooth = true;

    void Init()
    {
        Target = GameObject.Find("Arissa");
        if (Target == null)
            Debug.LogWarning("CameraController Searching Player Failed!");
    }

    void Start()
    {
        Init();

    }

    void LateUpdate()
    {
        if (Target.IsValid() == false)
            return;

        HandleQuarterViewViewCamera(); // default ��ġ ����
        UpdateEnabled();// ��Ȱ��ȭ ����Ʈ �ʱ�ȭ;
        CameraRayCast(mode); // ī�޶�� �÷��̾� ����ĳ���� �� ��ġ ����
    }

    public void SetTarget(GameObject obj)
    {
        Target = obj;
    }

    public void HandleQuarterViewViewCamera()
    {
        Vector3 worldPositon = (Vector3.forward * -distance) + (Vector3.up * height);
        Debug.DrawLine(Target.transform.position, worldPositon, Color.black);

        Vector3 rotateVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPositon;
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
                transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed);
            }

        }

        transform.LookAt(_lookTargetPosition);
    }
    
    void CameraRayCast(Define.CameraMode mode)
    {
        if (mode != Define.CameraMode.QuarterView)
            return;

        //1. �÷��̾�� ī�޶� ���̿� ��ü ���� Ȯ��
        //2. �ش� ��ü�� �ǹ����̾�� �÷��̾ ������ �ִٰ� �Ǵܵȴٸ� �Ⱥ��̵��� ����

        Vector3 _lookTargetPosition = Target.transform.position;
        _lookTargetPosition.y += lookAtHeight;

        Vector3 dir = _lookTargetPosition - transform.position;
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Wall");

        if (Physics.Raycast(transform.position, dir.normalized, out hit, dir.magnitude, mask))
        {
            hit.collider.gameObject.GetComponent<MeshRenderer>().enabled =false;
            hit.collider.gameObject.layer = (int)Define.Layer.Enabled;
            _enabledObjects.Add(hit.collider.gameObject);
            CameraRayCast(mode);
        }
    }

    private void UpdateEnabled()
    {
       foreach (GameObject go in _enabledObjects)
       {
            go.GetComponent<MeshRenderer>().enabled = true;
            go.layer = (int)Define.Layer.Wall;
       }
        _enabledObjects.Clear();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TargetPointer : UI_World
{
    public float playTime = 0.5f;
    public float surfaceOffset = 0.2f; //���鿡�� Ư�� ���� ��ŭ ����
    public Transform target = null;

    private Coroutine coroutine = null;
 
    void Update()
    {
        if(target)
        {
            transform.position = target.position + Vector3.up * surfaceOffset;
            //���� ���� ���ڰ� �þ �� ���� �з�(��,��,����)�� ���� Ÿ�� �̹��� ũ�� ����
        }
    }

    public void SetPosition(RaycastHit hit)
    {
        target = null;
        transform.position = hit.point + hit.normal * surfaceOffset;
        DestroyPoint(playTime);
    }

    public void SetTarget(RaycastHit hit)
    {
        target = hit.transform;
    }

    public void DestroyPoint(float time = -1f)
    {
        //�̹� ���� ��� ���̸� ����
        if (coroutine != null)
            return;

        //���� �ð� �Է� ���� �� playTime���� ������ �ش� �ð�����
        if(time == -1f)
            coroutine = StartCoroutine(CoDisEnablePoiter(playTime));
        else
            coroutine = StartCoroutine(CoDisEnablePoiter(time));
    }

    IEnumerator CoDisEnablePoiter(float playTime)
    {
        yield return new WaitForSeconds(playTime);
        Managers.Resource.Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TargetPointer : UI_World
{
    public float playTime = 0.5f;
    public float surfaceOffset = 0.2f; //지면에서 특정 높이 만큼 위로
    public Transform target = null;

    private Coroutine coroutine = null;
 
    void Update()
    {
        if(target)
        {
            transform.position = target.position + Vector3.up * surfaceOffset;
            //추후 몬스터 숫자가 늘어날 시 몬스터 분류(대,중,소형)에 따라 타겟 이미지 크기 수정
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
        //이미 삭제 대기 중이면 리턴
        if (coroutine != null)
            return;

        //별도 시간 입력 없을 시 playTime으로 있으면 해당 시간으로
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

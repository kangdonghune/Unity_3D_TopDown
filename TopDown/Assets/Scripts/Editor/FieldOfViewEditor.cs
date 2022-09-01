using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;

        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.ViewRadius);
        //왼쪽 
        Vector3 viewAngleA = fov.DirFromAngle(-fov.ViewAngle / 2, false).normalized;
        //오른쪽
        Vector3 viewAngleB = fov.DirFromAngle(fov.ViewAngle / 2, false).normalized;

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.ViewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.ViewRadius);

        //시야 범위 내 보이는 플레이어 표시
        Handles.color = Color.yellow;
        foreach (Transform visiableTarget in fov.VisiableTagets)
        {
            if (fov.NearestTarget != visiableTarget)
            {
                Handles.DrawLine(fov.transform.position, visiableTarget.position);
            }
        }

        //가장 가까운 플레이어 표시
        Handles.color = Color.red;
        if(fov.NearestTarget != null)
            Handles.DrawLine(fov.transform.position, fov.NearestTarget.position);

        

    }


}

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
        //���� 
        Vector3 viewAngleA = fov.DirFromAngle(-fov.ViewAngle / 2, false).normalized;
        //������
        Vector3 viewAngleB = fov.DirFromAngle(fov.ViewAngle / 2, false).normalized;

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.ViewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.ViewRadius);

        //�þ� ���� �� ���̴� �÷��̾� ǥ��
        Handles.color = Color.yellow;
        foreach (Transform visiableTarget in fov.VisiableTagets)
        {
            if (fov.NearestTarget != visiableTarget)
            {
                Handles.DrawLine(fov.transform.position, visiableTarget.position);
            }
        }

        //���� ����� �÷��̾� ǥ��
        Handles.color = Color.red;
        if(fov.NearestTarget != null)
            Handles.DrawLine(fov.transform.position, fov.NearestTarget.position);

        

    }


}

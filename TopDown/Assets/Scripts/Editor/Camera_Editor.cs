using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraController))]
public class Camera_Editor : Editor
{
    #region
    private CameraController targetCamera;
    #endregion

    public override void OnInspectorGUI()
    {
        targetCamera = (CameraController)target;
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        if(!targetCamera || !targetCamera.Target)
        {
            return;
        }

        Transform cameraTarget = targetCamera.Target.transform;
        Vector3 targetPosition = cameraTarget.transform.position;
        targetPosition.y += targetCamera.lookAtHeight;

        //Draw Distance Circle
        Handles.color = new Color(1f, 0, 0, 0.15f);
        Handles.DrawSolidDisc(targetPosition, Vector3.up, targetCamera.distance);

        Handles.color = new Color(0f, 1f, 0f, 0.75f);
        Handles.DrawWireDisc(targetPosition, Vector3.up, targetCamera.distance);

        // Create Slider handle
        Handles.color = new Color(0f, 1f, 0f, 0.5f);
        targetCamera.distance = Handles.ScaleSlider(targetCamera.distance,
                              targetPosition, - cameraTarget.forward, 
                              Quaternion.identity, targetCamera.distance, 0.1f);
        targetCamera.distance = Mathf.Clamp(targetCamera.distance, 2f, float.MaxValue);

        Handles.color = new Color(0f, 0f, 1f, 0.5f);
        targetCamera.height = Handles.ScaleSlider(targetCamera.height,
                      targetPosition, Vector3.up,
                      Quaternion.identity, targetCamera.height, 0.1f);
        targetCamera.height = Mathf.Clamp(targetCamera.height, 2f, 50f);

        //Create Labels
        GUIStyle labelsStyle = new GUIStyle();
        labelsStyle.fontSize = 15;
        labelsStyle.normal.textColor = Color.white;
        labelsStyle.alignment = TextAnchor.UpperCenter;

        Handles.Label(targetPosition + (-cameraTarget.forward * targetCamera.distance), "Distance", labelsStyle);

        labelsStyle.alignment = TextAnchor.MiddleRight;

        Handles.Label(targetPosition + (Vector3.up * targetCamera.height), "Height", labelsStyle);


        targetCamera.HandleQuarterViewViewCamera();


    }


}

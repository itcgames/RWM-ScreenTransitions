//#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ScreenTransition))]
public class ScreenTransitionEditor : Editor
{
    private ScreenTransition sT;
    private TransitionPoint currPoint = new TransitionPoint();
	private Camera mainCam;

    private void OnEnable()
	{
		// Method 1
		mainCam = Camera.main;
		sT = mainCam.GetComponent<ScreenTransition>();
        currPoint.transitionPoint = mainCam.transform.position + new Vector3(1, 1, 0);
	}

    protected virtual void OnSceneGUI()
    {
        // Safety against selecting a null GameObject
        if (sT == null)
        {
            return;
        }

        // draw Handles for all points
        if(sT.transitionPoints.Count > 0)
        {
            foreach(TransitionPoint point in sT.transitionPoints)
            {
                Handles.color = Color.yellow;
                Vector2 pos = Handles.PositionHandle(point.transitionPoint, Quaternion.identity);
                point.transitionPoint = pos;
            }
        }

        // handle point used to add new transition points to the list
        Vector2 newPos = Handles.PositionHandle(currPoint.transitionPoint, Quaternion.identity);
        currPoint.transitionPoint = newPos;
        Handles.Label(newPos, "Current Point");
    }

    public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Add Point"))
		{
            currPoint.type = sT.type;
            Vector2 vec = currPoint.transitionPoint;
            sT.AddPoint(currPoint);
            currPoint = new TransitionPoint();
            currPoint.transitionPoint = vec;
        }

        if (GUILayout.Button("Remove Last Point"))
        {
            sT.RemoveLastPoint();
        }

        // Draw default inspector after button...
        base.OnInspectorGUI();
	}
}
//#endif
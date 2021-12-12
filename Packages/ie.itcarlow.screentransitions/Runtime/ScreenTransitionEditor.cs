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

        // draw lines between all points in the vector
        if (sT.transitionPoints.Count > 0)
        {
            Handles.DrawLine(mainCam.transform.position, sT.transitionPoints[0].transitionPoint);

            for (int i = 0; i < sT.transitionPoints.Count; i++)
            {
                if (i + 1 >= sT.transitionPoints.Count)
                {
                    break;
                }

                Handles.DrawLine(sT.transitionPoints[i].transitionPoint, sT.transitionPoints[i + 1].transitionPoint);
            }

        }

        // now draw a line from the last point added to the current point being edited
        Handles.color = new Color(0, 1, 0, 1);

        if (sT.transitionPoints.Count > 0)
        {
            Handles.DrawLine(sT.transitionPoints[sT.transitionPoints.Count - 1].transitionPoint, currPoint.transitionPoint);
        }
        else
        {
            Handles.DrawLine(mainCam.transform.position, currPoint.transitionPoint);
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
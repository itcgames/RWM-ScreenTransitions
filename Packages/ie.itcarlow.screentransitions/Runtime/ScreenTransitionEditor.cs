#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ScreenTransition))]
public class ScreenTransitionEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        ScreenTransition customClass = (ScreenTransition)target;

        // Safety against selecting a null GameObject
        if (customClass == null)
        {
            return;
        }

        Handles.color = Color.yellow;
        Handles.DrawLine(customClass.transform.position, customClass.transitionPoint);
        Vector2 newPos = Handles.PositionHandle(customClass.transitionPoint, Quaternion.identity);

        if (customClass.type == ScreenTransition.transitionTypes.HORIZONTAL)
        {
            customClass.transitionPoint.x = newPos.x;
            customClass.transitionPoint.y = customClass.transform.position.y;
        }
        else
        {
            customClass.transitionPoint.x = customClass.transform.position.x;
            customClass.transitionPoint.y = newPos.y;
        }
            
    }
}
#endif
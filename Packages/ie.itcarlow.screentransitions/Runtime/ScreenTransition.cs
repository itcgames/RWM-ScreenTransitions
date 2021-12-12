using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TransitionTypes
{
    HORIZONTAL, VERTICAL
}

[System.Serializable]
public class TransitionPoint
{
    public Vector2 transitionPoint;
    public TransitionTypes type = TransitionTypes.HORIZONTAL;
}

public class ScreenTransition : MonoBehaviour
{
    public TransitionTypes type = TransitionTypes.HORIZONTAL;
    public List<TransitionPoint> transitionPoints;

    private Camera cam;
    private bool transitioning = false;

    // Start is called before the first frame update
    void Start()
    {
        // this component must be put on the main camera of the scene
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPoint(TransitionPoint t_point)
    {
        transitionPoints.Add(t_point);
    }

    public void RemoveLastPoint()
    {
        if (transitionPoints.Count > 0) // make sure points exist first
            transitionPoints.RemoveAt(transitionPoints.Count - 1); // removes the last item in the list
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum TransitionTypes
{
    FREE, HORIZONTAL, VERTICAL, INSTANT
}


[System.Serializable]
public class TransitionPoint
{
    public Vector2 transitionPoint;
    public TransitionTypes type;
}

public class ScreenTransition : MonoBehaviour
{
    public TransitionPoint currPoint = new TransitionPoint();
    public List<TransitionPoint> transitionPoints;
    public float transitionSpeed = 2.0f;
    public bool transitioning = false;

    private Camera cam;
    private TransitionPoint pickedPoint;
    private float distanceToNextPoint = 0.0f;
    private float minDistanceToStop = 0.1f;
    private float speedCap = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        // this component must be put on the main camera of the scene
        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (distanceToNextPoint < minDistanceToStop && transitioning)
        {
            StopCoroutine("transition");
            cam.transform.position = new Vector3(pickedPoint.transitionPoint.x, pickedPoint.transitionPoint.y, cam.transform.position.z);
            transitioning = false;
        }
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

    public void BeginTransition(int t_transitionTo)
    {
        if(!transitioning)
        { // cannot enter a transition if already transitioning to a point
            if (transitionPoints.Count > t_transitionTo) // if the point they passed in exists in the array
            { // example, if there's 2 items, and they select point 1, 1 < 2, so it goes through
                pickedPoint = transitionPoints[t_transitionTo];
                StartCoroutine("transition");
            }
            else
                Debug.LogWarning("Transition Point to move to does not exist! " +
                    "You may have entered the wrong number. " +
                    "Chosen Point:" + t_transitionTo +
                    ", Number of Points: " + transitionPoints.Count + ".");
        }
        
    }

    IEnumerator transition()
    {
        if(pickedPoint.type == TransitionTypes.INSTANT)
        { // if the camera should move instantly, don't bother with the rest of the function
            cam.transform.position = new Vector3(pickedPoint.transitionPoint.x, pickedPoint.transitionPoint.y, cam.transform.position.z);
            transitioning = false; // set to false in case of the variable being set to true elsewhere

            yield break; // we are done now, so break out of the co-routine
        }


        transitioning = true;
        // first we will get a vector towards the transition point,
        // so we can move smoothly towards it
        Vector2 normal;
        Vector2 point = cam.transform.position;

        float calculatedSpeed = transitionSpeed * Time.deltaTime;

        normal = (pickedPoint.transitionPoint - point).normalized;

        switch (pickedPoint.type)
        {
            case TransitionTypes.FREE: // camera moves directly towards transition point
                normal = (pickedPoint.transitionPoint - new Vector2(cam.transform.position.x, cam.transform.position.y)).normalized;
                break;
            case TransitionTypes.HORIZONTAL: // camera moves along X Axis towards transition point's X Axis
                normal = (pickedPoint.transitionPoint - new Vector2(cam.transform.position.x, 0)).normalized;
                break;
            case TransitionTypes.VERTICAL:  // camera moves along Y Axis towards transition point's Y Axis
                normal = (pickedPoint.transitionPoint - new Vector2(0, cam.transform.position.y)).normalized;
                break;
        }

        // if the user put the speed at an over the top amount
        // it isn't possible for the camera to not reach the point in about 1 frame,
        // so just place the camera at that point
        bool tooFast = calculatedSpeed > speedCap;

        // now that we have the normal, we can begin moving towards it
        while (!tooFast)
        { // camera stopping will be implemented in a later feature

            switch (pickedPoint.type)
            {
                case TransitionTypes.FREE: // camera moves directly towards transition point
                    cam.transform.position = new Vector3(cam.transform.position.x + (normal.x * calculatedSpeed),
                    cam.transform.position.y + (normal.y * calculatedSpeed), cam.transform.position.z);
                    break;
                case TransitionTypes.HORIZONTAL: // camera moves along X Axis towards transition point's X Axis
                    cam.transform.position = new Vector3(cam.transform.position.x + (normal.x * calculatedSpeed),
                    cam.transform.position.y, cam.transform.position.z);
                    break;
                case TransitionTypes.VERTICAL:  // camera moves along Y Axis towards transition point's Y Axis
                    cam.transform.position = new Vector3(cam.transform.position.x,
                    cam.transform.position.y + (normal.y * calculatedSpeed), cam.transform.position.z);
                    break;
            }

            switch (pickedPoint.type)
            {
                case TransitionTypes.FREE: // camera moves directly towards transition point
                    distanceToNextPoint = Vector2.Distance(cam.transform.position, pickedPoint.transitionPoint);
                    break;
                case TransitionTypes.HORIZONTAL: // camera moves along X Axis towards transition point's X Axis
                    distanceToNextPoint = Mathf.Abs(cam.transform.position.x - pickedPoint.transitionPoint.x);
                    break;
                case TransitionTypes.VERTICAL:  // camera moves along Y Axis towards transition point's Y Axis
                    distanceToNextPoint = Mathf.Abs(cam.transform.position.y - pickedPoint.transitionPoint.y);
                    break;
            }

            yield return new WaitForSeconds(0.01f);
        }
    }
}
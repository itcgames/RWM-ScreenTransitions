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
    public float transitionSpeed = 2.0f;
    public bool transitioning = false;

    private Camera cam;
    private TransitionPoint pickedPoint;
    private float minDistanceToStop = 0.1f;
    private float speedCap = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        // this component must be put on the main camera of the scene
        cam = Camera.main;
        transitionSpeed = transitionSpeed * Time.deltaTime;
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

    public void BeginTransition(int t_transitionTo)
    {
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

    IEnumerator transition()
    {
        transitioning = true;
        // first we will get a vector towards the transition point,
        // so we can move smoothly towards it
        Vector2 normal;
        Vector2 point = cam.transform.position;

        normal = (pickedPoint.transitionPoint - point).normalized;

        // if the user put the speed at an over the top amount
        // it isn't possible for the camera to not reach the point in about 1 frame,
        // so just place the camera at that point
        bool tooFast = transitionSpeed > speedCap;

        // now that we have the normal, we can begin moving towards it
        while (!tooFast)
        { // camera stopping will be implemented in a later feature
            cam.transform.position = new Vector3(cam.transform.position.x + (normal.x * transitionSpeed),
                cam.transform.position.y + (normal.y * transitionSpeed), cam.transform.position.z);
            
            yield return new WaitForSeconds(0.01f);

            // since the camera could be moving towards our transition point at any direction,
            // we will do a simple check to see if the camera is close enough
            // then set the camera's position to the specified point, and stop transitioning

            float distanceToPoint = Vector2.Distance(cam.transform.position, pickedPoint.transitionPoint);

            if(distanceToPoint < minDistanceToStop)
            {
                break;
            }
        }

        cam.transform.position = pickedPoint.transitionPoint;
        transitioning = false;
    }
}
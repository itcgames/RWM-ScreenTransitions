using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScreenTransition : MonoBehaviour
{
    public Camera cam;

    public enum transitionTypes { HORIZONTAL, VERTICAL };
    public transitionTypes type = transitionTypes.HORIZONTAL;
    public Vector2 transitionPoint = Vector2.zero;
    private bool transitioning = false;

    // Start is called before the first frame update
    void Start()
    {
        // this component must be attached to a camera
        cam = this.gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

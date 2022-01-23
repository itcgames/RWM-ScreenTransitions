using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space"))
        {
            this.GetComponent<ScreenTransition>().BeginTransition(0);
            Debug.Log("space pressed, going to point 1");
        }

        if (Input.GetKeyDown("z"))
        {
            this.GetComponent<ScreenTransition>().BeginTransition(1);
            Debug.Log("z pressed, going to point 2");
        }
    }
}

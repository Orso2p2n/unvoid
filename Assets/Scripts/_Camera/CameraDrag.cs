using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraDrag : MonoBehaviour
{
    public float rotationSpeed;
    public float lerpVal;
    private float trueLerpVal;
    
    private Vector3 dragOrigin;
    private Vector3 dragCurrent;
    private Vector3 trueOrigin;
    
    public Camera cam;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            trueOrigin = dragOrigin;
            trueLerpVal = lerpVal;
        }

        if (Input.GetMouseButton(1))
        {
            dragCurrent = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            trueLerpVal = (dragCurrent - dragOrigin).magnitude / (dragCurrent - trueOrigin).magnitude;
            trueLerpVal /= 10;
            trueLerpVal = 0.1f - trueLerpVal;
            trueLerpVal /= 10;
        }
        
        Vector3 pos = cam.ScreenToViewportPoint(dragCurrent - dragOrigin);
            
        Vector3 rotX = new Vector3(-pos.y, 0, 0) * rotationSpeed;
        transform.Rotate(rotX, Space.Self);
            
        Vector3 rotY = new Vector3(0, pos.x, 0) * rotationSpeed;
        transform.Rotate(rotY, Space.Self);
        
        dragOrigin = Vector3.Lerp(dragOrigin, dragCurrent, trueLerpVal);
    }
}
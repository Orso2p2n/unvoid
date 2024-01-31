using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets;
    public float smoothTime;

    public Camera cam;
    public float minDist;
    public float maxDist;
    public float maxDistance;
    
    private Vector3 velocity;

    private void LateUpdate()
    {
        Zoom();
    }
    
    private void Zoom()
    {
        float newZoom = Mathf.Lerp(minDist, maxDist, GetGreatestDistance() / maxDistance);
        transform.localPosition = new Vector3(0, 0, Mathf.Lerp(transform.localPosition.z, newZoom, smoothTime));
    }

    private float GetGreatestDistance()
    {
        var bounds = GetBounds();
        var greatestDistance = Mathf.Pow(bounds.size.x, 2) + Mathf.Pow(bounds.size.z, 2);
        greatestDistance = Mathf.Sqrt(greatestDistance);
        return greatestDistance;
    }

    private Bounds GetBounds()
    {
        if (targets.Count == 0)
        {
            return new Bounds(Vector3.zero, Vector3.zero);
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        
        foreach (var target in targets)
        {
            bounds.Encapsulate(target.position);
        }

        return bounds;
    }
}

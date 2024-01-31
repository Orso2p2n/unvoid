using System;
using System.Collections;
using System.Collections.Generic;
using Clayxels;
using DG.Tweening;
using UnityEngine;

public class ClayColor : MonoBehaviour
{
    private ClayObject clayObject;
    private Gradient colorRamp;

    public Spawner spawner;
    public float distanceFromPlanet;

    private void Start()
    {
        clayObject = GetComponent<ClayObject>();
        StartCoroutine(SemiUpdate());

        var scale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(scale, 0.25f);
    }

    private IEnumerator SemiUpdate()
    {
        while (true)
        {
            UpdateColor();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Initialize()
    {
        colorRamp = spawner.colorRamp;

        if (distanceFromPlanet > spawner.maxHeight)
            distanceFromPlanet -= spawner.maxHeight;
    }
    
    public void UpdateColor()
    {
        var distanceWithOffset = distanceFromPlanet + spawner.offset;
        
        if (distanceWithOffset > spawner.maxHeight)
            distanceWithOffset -= spawner.maxHeight;
        
        var lerpVal = distanceWithOffset / spawner.maxHeight;

        var newColor = colorRamp.Evaluate(lerpVal);

        GetComponent<ClayObject>().color = newColor;
        GetComponent<ClayObject>().forceUpdate();
    }
}

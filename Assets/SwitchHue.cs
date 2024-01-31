using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SwitchHue : MonoBehaviour
{
    public float toShift;
    public float shiftingTime;

    public PostProcessProfile postProcess;

    public void Shift()
    {
        var shiftTo = postProcess.GetSetting<ColorGrading>().hueShift.value + toShift;

        if (shiftTo > 180)
        {
            shiftTo -= 360;
        }

        DOTween.To(()=> postProcess.GetSetting<ColorGrading>().hueShift.value, x=> postProcess.GetSetting<ColorGrading>().hueShift.value = x, shiftTo, shiftingTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxColor : MonoBehaviour
{
    public Material skybox;
    public Spawner spawner;

    private Gradient colorRamp;
    
    private static readonly int SkyGradientTop = Shader.PropertyToID("_SkyGradientTop");
    private static readonly int SkyGradientBottom = Shader.PropertyToID("_SkyGradientBottom");

    private void Start()
    {
        colorRamp = spawner.colorRamp;
        
        StartCoroutine(SemiUpdate());
    }

    private IEnumerator SemiUpdate()
    {
        while (true)
        {
            var lerpVal = spawner.offset / spawner.maxHeight;

            var color = colorRamp.Evaluate(lerpVal) + Color.black;

            color = DarkenColor(color);

            skybox.SetColor(SkyGradientTop, color);
            skybox.SetColor(SkyGradientBottom, color);
            
            yield return new WaitForSeconds(0.05f);
        }
    }

    private Color DarkenColor(Color color)
    {
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);

        S = 1f;
        V = 0.1f;

        return Color.HSVToRGB(H, S, V);
    }
}

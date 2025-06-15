using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RemoveDuplicateGlobalLight2D : MonoBehaviour
{
    void Awake()
    {
        var lights = Object.FindObjectsByType<Light2D>(FindObjectsSortMode.None);
        int globalCount = 0;
        foreach (var light in lights)
        {
            if (light.lightType == Light2D.LightType.Global)
            {
                globalCount++;
                if (globalCount > 1)
                    Destroy(light.gameObject);
            }
        }
    }
}
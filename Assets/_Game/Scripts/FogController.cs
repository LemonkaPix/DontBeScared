using UnityEngine;
using System.Collections;

public class FogController : MonoBehaviour
{
    public static FogController instance;
    public static float OriginalDensity = 0;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        OriginalDensity = RenderSettings.fogDensity;
    }
    public void ChangeFogDensity(float targetDensity, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(ChangeFogDensityCoroutine(targetDensity, duration));
    }

    private IEnumerator ChangeFogDensityCoroutine(float targetDensity, float duration)
    {
        float startDensity = RenderSettings.fogDensity;
        float elapsed = 0f;

        if (duration == 0)
        {
            RenderSettings.fogDensity = targetDensity;
        }
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newDensity = Mathf.Lerp(startDensity, targetDensity, elapsed / duration);
            RenderSettings.fogDensity = newDensity;
            yield return null;
        }

        
        
        RenderSettings.fogDensity = targetDensity;
    }
}

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class VignetteController : MonoBehaviour
{
    public static VignetteController instance;
    public static float OriginalVignette;

    private Volume volume;             
    private Vignette vignette;          
    public float transitionTime = 2.0f; 

    private void Awake()
    {
        if (instance == null) instance = this;

        volume = GetComponent<Volume>();

        if (volume == null || !volume.profile.TryGet(out vignette))
        {
            Debug.LogError("Nie znaleziono profilu Volume lub efektu Vignette.");
        }
    }

    private void Start()
    {
        OriginalVignette = vignette.intensity.value;
    }

    public void ChangeVignetteIntensity(float targetIntensity, float duration)
    {
        StopAllCoroutines();
        if (vignette != null)
        {
            StartCoroutine(ChangeVignetteIntensityCoroutine(targetIntensity, duration));
        }
        else
        {
            Debug.LogWarning("Efekt Vignette nie jest przypisany do profilu Volume.");
        }
    }

    private IEnumerator ChangeVignetteIntensityCoroutine(float targetIntensity, float duration)
    {
        float startIntensity = vignette.intensity.value;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
            vignette.intensity.value = newIntensity;
            yield return null;
        }

        vignette.intensity.value = targetIntensity;
    }
}

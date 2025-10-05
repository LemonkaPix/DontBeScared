using System;
using System.Collections;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ChangeBrightnessOfUI : MonoBehaviour
{
    private Image _image;

    [SerializeField] [MinMaxSlider(0f, 10000000f)]
    private Vector2 lightningRange;

    // Właściwość zamiast wskaźnika
    private float LightLevel
    {
        get => LightCheck.instance.LightLevel;
        set => LightCheck.instance.LightLevel = value;
    }

    private void Start()
    {
        _image = GetComponent<Image>();
        // StartCoroutine(Tick());
    }

    // IEnumerator Tick()
    // {
    //     while (true)
    //     {
    //         // Wykorzystaj LightLevel w obliczeniach - np. zmiana jasności na podstawie poziomu światła
    //         float brightness = Mathf.InverseLerp(lightningRange.x, lightningRange.y, LightLevel);
    //         _image.color = Color.HSVToRGB(0, 0, brightness);
    //
    //         yield return new WaitForSeconds(tick);
    //     }
    // }

    private void Update()
    {
        float brightness = math.max(0.2f, Mathf.InverseLerp(lightningRange.x, lightningRange.y, LightLevel));
        _image.color = Color.HSVToRGB(0, 0, brightness);

    }
}
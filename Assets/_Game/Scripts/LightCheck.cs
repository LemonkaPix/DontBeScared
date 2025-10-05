using System;
using UnityEngine;

public class LightCheck : MonoBehaviour
{
    public static LightCheck instance;
    
    public RenderTexture lightCheckTexture;
    public float LightLevel;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Update()
    {
        RenderTexture tmpTexture = RenderTexture.GetTemporary(lightCheckTexture.width, lightCheckTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(lightCheckTexture, tmpTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmpTexture;

        Texture2D temp2DTexture = new Texture2D(lightCheckTexture.width, lightCheckTexture.height);
        temp2DTexture.ReadPixels(new Rect(0, 0, tmpTexture.width, tmpTexture.height), 0, 0);
        temp2DTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmpTexture);

        Color32[] color = temp2DTexture.GetPixels32();

        LightLevel = 0;
        for (int i = 0; i < color.Length; i++)
        {
            LightLevel += ((0.2126f * color[i].r) + (0.7152f * color[i].g) + (0.0722f * color[i].b));
        }

        // print((int)LightLevel);

    }
}

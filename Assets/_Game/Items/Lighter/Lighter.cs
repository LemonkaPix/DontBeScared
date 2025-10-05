using System;
using UnityEngine;

public class Lighter : Item
{
    [SerializeField] Light light;
    Inventory inventory;
    [SerializeField] Sprite[] sprites = new Sprite[2];

    private void Start()
    {
        inventory = Inventory.instance;
    }
    public override void LeftClick()
    {
        light.enabled = !light.enabled;

        UIController.Instance.ChangeRightHandSprite(light.enabled ? sprites[1] : sprites[0]);

        // if (light.enabled)
        // {
        //     FogController.instance.ChangeFogDensity(0.1f,0);
        // }
        // else
        // {
        //     FogController.instance.ChangeFogDensity(FogController.OriginalDensity,0);
        // }
    }
    public override void RightClick()
    {

    }

    private void OnDisable()
    {
        light.enabled = false;
        FogController.instance.ChangeFogDensity(FogController.OriginalDensity,0);
    }
}

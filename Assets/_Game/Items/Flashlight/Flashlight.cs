using System;
using UnityEngine;

public class Flashlight : Item
{
    [SerializeField] Light spotLight;
    [SerializeField] SmoothSpotlight smoothSpotlight;
    [SerializeField] Light pointLight;
    [SerializeField] Sprite middleHoldSprite;
    Inventory inventory;
    bool isOn = false;
    bool isAiming = false;

    float originLightIntensity;
    float originRange;
    float originInnerAngle;
    float originOuterAngle;


    private void Start()
    {
        inventory = Inventory.instance;
        originLightIntensity = spotLight.intensity;
        originRange = spotLight.range;
        originInnerAngle = spotLight.innerSpotAngle;
        originOuterAngle = spotLight.spotAngle;
    }
    public override void LeftClick()
    {
        if (!isAiming)
        {
            isOn = !isOn;
            spotLight.enabled = !spotLight.enabled;
            pointLight.enabled = !pointLight.enabled;

            //inventory.handImage.sprite = spotLight.enabled ? sprites[1] : sprites[0];

            if (isOn)
            {
                FogController.instance.ChangeFogDensity(0.05f, 0);
            }
            else
            {
                FogController.instance.ChangeFogDensity(FogController.OriginalDensity, 0);
            }

        }
    }
    public override void RightClick()
    {
        if (isOn)
        {
            isAiming = !isAiming;
            Aim(isAiming);
        }

    }

    public void Aim(bool state)
    {
        //UIController.Instance.RightHandVisible(true);
        PlayerMovement.isAiming = state;
        //spotLight.enabled = !state;

        if (isAiming)
        {
            UIController.Instance.MiddleHoldVisible(true);
            UIController.Instance.SwitchFromRightToMiddleHold();
            VignetteController.instance.ChangeVignetteIntensity(0.4f, 0.5f);
            FogController.instance.ChangeFogDensity(0.025f, 0.5f);
            smoothSpotlight.SetTargetValues(60, 17, 24, 20, 0.3f);
            inventory.BlockChangingItem = true;
        }
        else
        {
            UIController.Instance.SwitchFromMiddleToRightHand();
            VignetteController.instance.ChangeVignetteIntensity(VignetteController.OriginalVignette, 0.3f);
            FogController.instance.ChangeFogDensity(0.1f, 0.5f);
            smoothSpotlight.SetTargetValues(originLightIntensity, originInnerAngle, originOuterAngle, originRange, 0.3f);
            inventory.BlockChangingItem = false;
        }
    }

    private void OnEnable()
    {
        UIController.Instance.ChangeMiddleHoldSprite(middleHoldSprite);
    }

    private void OnDisable()
    {
        Aim(false);
        isOn = false;
        spotLight.enabled = false;
        pointLight.enabled = false;

        if (FogController.instance != null)
        {
            FogController.instance.ChangeFogDensity(FogController.OriginalDensity, 0);
        }
    }
}

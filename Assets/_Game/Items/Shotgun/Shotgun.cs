using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Shotgun : Item
{
    [SerializeField] Sprite middleHoldSprite;
    [SerializeField] ShotCollider shortShot;
    [SerializeField] ShotCollider longShot;
    [SerializeField] Light shotLight;
    [SerializeField] AudioSource shotgunAudioSource;
    [SerializeField] AudioClip shotgunSound;
    Inventory inventory;
    bool isAiming = false;

    [SerializeField] private float shotDelay = 0.5f;
    private float lastShotTime = 0f;

    private void Awake()
    {
        inventory = Inventory.instance;
    }

    public override void LeftClick()
    {

        if (Time.time - lastShotTime >= shotDelay)
        {
            lastShotTime = Time.time;

            // Odtwórz dźwięk strzału
            PlayShotgunSound();

            StartCoroutine(lightShot());

            if (isAiming)
            {
                longShot.Shot();
                UIController.Instance.ShotgunRecoilMiddle();
            }
            else
            {
                shortShot.Shot();
                UIController.Instance.ShotgunRecoilHand();
            }
        }
    }

    public override void RightClick()
    {
        // isAiming = !isAiming;
        // Aim(isAiming);
    }

    public void Aim(bool state)
    {
        PlayerMovement.isAiming = state;

        if (state)
        {
            UIController.Instance.MiddleHoldVisible(true);
            UIController.Instance.SwitchFromRightToMiddleHold();
            VignetteController.instance.ChangeVignetteIntensity(0.3f, 0.5f);
            FogController.instance.ChangeFogDensity(0.1f, 0.5f);

            //inventory.BlockChangingItem = true;
        }
        else
        {
            UIController.Instance.SwitchFromMiddleToRightHand();
            VignetteController.instance.ChangeVignetteIntensity(VignetteController.OriginalVignette, 0.3f);
            FogController.instance.ChangeFogDensity(FogController.OriginalDensity, 0.5f);

            inventory.BlockChangingItem = false;
        }
    }

    private void OnEnable()
    {
        print("Shotgun enabled");
        isAiming = true;
        Aim(true);
        UIController.Instance.ChangeMiddleHoldSprite(middleHoldSprite);
    }

    private void OnDisable()
    {
        print("Shotgun disabled");
        isAiming = false;
        Aim(false);
        FogController.instance.ChangeFogDensity(FogController.OriginalDensity, 0);
    }

    IEnumerator lightShot()
    {
        shotLight.enabled = true;
        yield return new WaitForSeconds(.1f);
        shotLight.enabled = false;

    }

    private void PlayShotgunSound()
    {
        if (shotgunAudioSource != null && shotgunSound != null)
        {
            shotgunAudioSource.PlayOneShot(shotgunSound);
        }
    }
}

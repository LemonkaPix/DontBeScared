using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BlackScreenEnabler : MonoBehaviour
{
    [SerializeField]
    private Volume effectsVolume;

    [SerializeField]
    private CanvasGroup screenCanvas;

    [SerializeField]
    private Image insideBorder;

    [SerializeField]
    private float holdingTime;
    [SerializeField] AudioSource blackAudioSource;

    private bool gameBlocked = false;

    private float currentTimer = 0;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("BLOCK_GAME") == 1)
        {
            gameBlocked = true;
            // play music
            if (blackAudioSource) blackAudioSource.Play();
            effectsVolume.enabled = false;
            screenCanvas.alpha = 1;
        }
    }


    private void Update()
    {
        if (!gameBlocked)
            return;

        if (Input.GetKey(KeyCode.Delete))
        {
            currentTimer += Time.deltaTime;
            insideBorder.fillAmount = Mathf.Clamp(currentTimer / holdingTime, 0, 1);

            if (currentTimer > holdingTime)
            {
                // stop music
                if (blackAudioSource) blackAudioSource.Stop();

                PlayerPrefs.DeleteKey("BLOCK_GAME");
                insideBorder.fillAmount = 1;
                gameBlocked = false;
                effectsVolume.enabled = true;
                screenCanvas.alpha = 0;
                Terminal.Instance.StartMethod();
            }
        }
        else
        {
            insideBorder.fillAmount = 0;
            currentTimer = 0;
        }
    }
}

using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Globalny manager dźwięków interfejsu użytkownika
/// </summary>
public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;

    [Header("Keyboard Sounds")]
    [SerializeField] private AudioClip[] keyboardClickSounds = new AudioClip[2];

    [Header("UI Sounds")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip successSound;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource uiAudioSource;
    [SerializeField][Range(0f, 1f)] private float masterUIVolume = 0.8f;
    [SerializeField][Range(0f, 1f)] private float keyboardVolume = 0.6f;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Sprawdź AudioSource
        if (uiAudioSource == null)
        {
            uiAudioSource = GetComponent<AudioSource>();
            if (uiAudioSource == null)
            {
                uiAudioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Konfiguruj AudioSource dla UI
        uiAudioSource.playOnAwake = false;
        uiAudioSource.loop = false;
    }

    #region Keyboard Sounds

    /// <summary>
    /// Odtwarza losowy dźwięk kliknięcia klawisza
    /// </summary>
    public void PlayRandomKeyboardClick()
    {
        if (keyboardClickSounds != null && keyboardClickSounds.Length > 0)
        {
            AudioClip randomClip = keyboardClickSounds[Random.Range(0, keyboardClickSounds.Length)];
            PlayUISound(randomClip, keyboardVolume);
        }
    }

    /// <summary>
    /// Odtwarza konkretny dźwięk klawiatury (0 lub 1)
    /// </summary>
    public void PlayKeyboardClick(int soundIndex)
    {
        if (keyboardClickSounds != null && soundIndex >= 0 && soundIndex < keyboardClickSounds.Length)
        {
            PlayUISound(keyboardClickSounds[soundIndex], keyboardVolume);
        }
    }

    #endregion

    #region UI Sounds

    public void PlayButtonClick()
    {
        PlayUISound(buttonClickSound);
    }

    public void PlayButtonHover()
    {
        PlayUISound(buttonHoverSound, 0.5f);
    }

    public void PlayErrorSound()
    {
        PlayUISound(errorSound);
    }

    public void PlaySuccessSound()
    {
        PlayUISound(successSound);
    }

    #endregion

    #region Private Methods

    private void PlayUISound(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip != null && uiAudioSource != null)
        {
            float finalVolume = masterUIVolume * volumeMultiplier;
            uiAudioSource.PlayOneShot(clip, finalVolume);
        }
    }

    #endregion

    #region Public Utilities

    /// <summary>
    /// Ustawia głośność master dla wszystkich dźwięków UI
    /// </summary>
    public void SetMasterUIVolume(float volume)
    {
        masterUIVolume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// Ustawia głośność klawiatury
    /// </summary>
    public void SetKeyboardVolume(float volume)
    {
        keyboardVolume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// Zwraca dostępne dźwięki klawiatury do użycia w innych skryptach
    /// </summary>
    public AudioClip[] GetKeyboardSounds()
    {
        return keyboardClickSounds;
    }

    #endregion

    #region Debug Methods

    [Button("Test Keyboard Sound 1")]
    private void TestKeyboard1()
    {
        if (Application.isPlaying) PlayKeyboardClick(0);
    }

    [Button("Test Keyboard Sound 2")]
    private void TestKeyboard2()
    {
        if (Application.isPlaying) PlayKeyboardClick(1);
    }

    [Button("Test Random Keyboard")]
    private void TestRandomKeyboard()
    {
        if (Application.isPlaying) PlayRandomKeyboardClick();
    }

    [Button("Test Button Click")]
    private void TestButtonClick()
    {
        if (Application.isPlaying) PlayButtonClick();
    }

    #endregion
}
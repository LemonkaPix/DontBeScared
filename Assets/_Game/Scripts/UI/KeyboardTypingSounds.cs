using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

/// <summary>
/// System odtwarzający losowe dźwięki klikania klawiatury podczas pisania w Input Field
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class KeyboardTypingSounds : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] keyClickSounds = new AudioClip[2];
    [SerializeField][Range(0f, 1f)] private float volume = 0.7f;
    [SerializeField][Range(0.8f, 1.2f)] private float pitchVariation = 0.1f;

    [Header("Input Field")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private InputField legacyInputField;

    [Header("Settings")]
    [SerializeField] private bool playOnlyWhenFocused = true;
    [SerializeField] private bool debugMode = false;

    private AudioSource audioSource;
    private string previousText = "";
    private int lastTextLength = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Automatycznie znajdź InputField jeśli nie został przypisany
        if (inputField == null && legacyInputField == null)
        {
            inputField = GetComponent<TMP_InputField>();
            if (inputField == null)
            {
                legacyInputField = GetComponent<InputField>();
            }
        }

        if (inputField == null && legacyInputField == null)
        {
            Debug.LogError("KeyboardTypingSounds: Nie znaleziono TMP_InputField ani InputField!");
            enabled = false;
            return;
        }

        // Sprawdź czy są przypisane dźwięki
        if (keyClickSounds == null || keyClickSounds.Length == 0)
        {
            Debug.LogWarning("KeyboardTypingSounds: Brak przypisanych dźwięków klawiatury!");
            enabled = false;
            return;
        }

        // Inicjalizuj poprzedni tekst
        if (inputField != null)
        {
            previousText = inputField.text;
            lastTextLength = previousText.Length;
        }
        else if (legacyInputField != null)
        {
            previousText = legacyInputField.text;
            lastTextLength = previousText.Length;
        }

        if (debugMode)
            Debug.Log("KeyboardTypingSounds: Zainicjalizowano pomyślnie");
    }

    void Update()
    {
        CheckForTextChanges();
    }

    private void CheckForTextChanges()
    {
        string currentText = "";
        bool isFocused = false;

        // Pobierz aktualny tekst i status focus
        if (inputField != null)
        {
            currentText = inputField.text;
            isFocused = inputField.isFocused;
        }
        else if (legacyInputField != null)
        {
            currentText = legacyInputField.text;
            isFocused = legacyInputField.isFocused;
        }

        // Sprawdź czy tekst się zmienił (dodano znak)
        if (currentText.Length > lastTextLength)
        {
            // Sprawdź czy pole jest aktywne (jeśli wymagane)
            if (!playOnlyWhenFocused || isFocused)
            {
                PlayRandomKeySound();

                if (debugMode)
                    Debug.Log($"KeyboardTypingSounds: Odtworzyłem dźwięk dla znaku: '{currentText[currentText.Length - 1]}'");
            }
        }

        // Zaktualizuj poprzedni stan
        previousText = currentText;
        lastTextLength = currentText.Length;
    }

    private void PlayRandomKeySound()
    {
        if (keyClickSounds == null || keyClickSounds.Length == 0 || audioSource == null)
            return;

        // Wybierz losowy dźwięk
        AudioClip randomSound = keyClickSounds[Random.Range(0, keyClickSounds.Length)];

        if (randomSound == null)
        {
            if (debugMode)
                Debug.LogWarning("KeyboardTypingSounds: Wybrany dźwięk jest null!");
            return;
        }

        // Ustaw losową wysokość tonu dla większej różnorodności
        float randomPitch = 1f + Random.Range(-pitchVariation, pitchVariation);
        audioSource.pitch = randomPitch;
        audioSource.volume = volume;

        // Odtwórz dźwięk
        audioSource.PlayOneShot(randomSound);
    }

    // Metoda do ręcznego odtworzenia dźwięku (dla testów)
    [Button("Test Random Key Sound")]
    public void TestKeySound()
    {
        if (Application.isPlaying)
        {
            PlayRandomKeySound();
        }
    }

    [Button("Test All Key Sounds")]
    public void TestAllKeySounds()
    {
        if (Application.isPlaying && keyClickSounds != null)
        {
            StartCoroutine(TestAllSoundsCoroutine());
        }
    }

    private System.Collections.IEnumerator TestAllSoundsCoroutine()
    {
        for (int i = 0; i < keyClickSounds.Length; i++)
        {
            if (keyClickSounds[i] != null)
            {
                Debug.Log($"Testuję dźwięk {i + 1}: {keyClickSounds[i].name}");
                audioSource.pitch = 1f;
                audioSource.volume = volume;
                audioSource.PlayOneShot(keyClickSounds[i]);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void OnValidate()
    {
        // Upewnij się że volume jest w odpowiednim zakresie
        volume = Mathf.Clamp01(volume);
        pitchVariation = Mathf.Clamp(pitchVariation, 0f, 0.5f);
    }
}
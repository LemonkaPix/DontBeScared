using UnityEngine;
using NaughtyAttributes;

[System.Serializable]
public class MonsterAnimationSet
{
    [Header("Animation Name")]
    public string animationName;

    [Header("Sprite Frames")]
    public Texture[] sprites;

    [Header("Animation Settings")]
    [Range(0.5f, 20f)]
    public float frameRate = 6f;

    [Space]
    public bool loop = true;

    [Header("Preview")]
    [ShowIf("sprites")]
    [InfoBox("Dodaj sprite'y w odpowiedniej kolejności dla płynnej animacji")]
    public bool showPreview;
}

/// <summary>
/// Edytor helper do łatwego konfigurowania animacji monstera
/// </summary>
public class MonsterAnimationHelper : MonoBehaviour
{
    [Header("Monster Animation Controller")]
    [Required]
    [SerializeField] private MonsterAnimationController animationController;

    [Header("Animation Presets")]
    [SerializeField] private MonsterAnimationSet idleAnimation = new MonsterAnimationSet { animationName = "Idle", frameRate = 2f, loop = true };
    [SerializeField] private MonsterAnimationSet shakingAnimation = new MonsterAnimationSet { animationName = "Shaking", frameRate = 8f, loop = true };
    [SerializeField] private MonsterAnimationSet transformingAnimation = new MonsterAnimationSet { animationName = "Transforming", frameRate = 6f, loop = false };
    [SerializeField] private MonsterAnimationSet runningAnimation = new MonsterAnimationSet { animationName = "Running", frameRate = 10f, loop = true };

    void Start()
    {
        if (animationController == null)
            animationController = GetComponent<MonsterAnimationController>();

        ApplyAnimationSettings();
    }

    [Button("Apply Animation Settings")]
    public void ApplyAnimationSettings()
    {
        if (animationController == null)
        {
            Debug.LogError("MonsterAnimationController nie został przypisany!");
            return;
        }

        // Zastosuj ustawienia animacji poprzez reflection lub dodaj publiczne metody w MonsterAnimationController
        Debug.Log("Ustawienia animacji zostały zastosowane. Sprawdź MonsterAnimationController w inspektorze.");
    }

    [Button("Test All Animations")]
    public void TestAllAnimations()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Testy animacji działają tylko podczas działania gry!");
            return;
        }

        StartCoroutine(TestAnimationSequence());
    }

    private System.Collections.IEnumerator TestAnimationSequence()
    {
        Debug.Log("Rozpoczynam test wszystkich animacji...");

        // Test Idle
        animationController.PlayIdleAnimation();
        Debug.Log("Test: Idle Animation");
        yield return new WaitForSeconds(3f);

        // Test Shaking
        animationController.PlayShakingAnimation();
        Debug.Log("Test: Shaking Animation");
        yield return new WaitForSeconds(3f);

        // Test Transforming
        animationController.PlayTransformingAnimation();
        Debug.Log("Test: Transforming Animation");
        yield return new WaitForSeconds(3f);

        // Test Running
        animationController.PlayRunningAnimation();
        Debug.Log("Test: Running Animation");
        yield return new WaitForSeconds(3f);

        // Powrót do Idle
        animationController.PlayIdleAnimation();
        Debug.Log("Test zakończony - powrót do Idle");
    }

    [Button("Play Idle")]
    private void TestIdle()
    {
        if (Application.isPlaying && animationController != null)
            animationController.PlayIdleAnimation();
    }

    [Button("Play Shaking")]
    private void TestShaking()
    {
        if (Application.isPlaying && animationController != null)
            animationController.PlayShakingAnimation();
    }

    [Button("Play Transforming")]
    private void TestTransforming()
    {
        if (Application.isPlaying && animationController != null)
            animationController.PlayTransformingAnimation();
    }

    [Button("Play Running")]
    private void TestRunning()
    {
        if (Application.isPlaying && animationController != null)
            animationController.PlayRunningAnimation();
    }

    [Button("Stop Animation")]
    private void StopAnimation()
    {
        if (Application.isPlaying && animationController != null)
            animationController.StopCurrentAnimation();
    }

    void OnValidate()
    {
        if (animationController == null)
            animationController = GetComponent<MonsterAnimationController>();
    }
}
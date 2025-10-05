using System.Collections;
using UnityEngine;
using NaughtyAttributes;

public class MonsterAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Renderer monsterRenderer;

    [Header("Idle Animation")]
    [SerializeField] private Texture[] idleSprites;
    [SerializeField] private float idleFrameRate = 2f;

    [Header("Shaking Animation")]
    [SerializeField] private Texture[] shakingSprites;
    [SerializeField] private float shakingFrameRate = 8f;

    [Header("Transforming Animation")]
    [SerializeField] private Texture[] transformingSprites;
    [SerializeField] private float transformingFrameRate = 6f;

    [Header("Running Animation")]
    [SerializeField] private Texture[] runningSprites;
    [SerializeField] private float runningFrameRate = 10f;

    [Header("Debug")]
    [SerializeField] private bool debugMode = false;

    private Material monsterMaterial;
    private AnimationType currentAnimation = AnimationType.None;
    private Coroutine animationCoroutine;

    public enum AnimationType
    {
        None,
        Idle,
        Shaking,
        Transforming,
        Running
    }

    void Start()
    {
        if (monsterRenderer == null)
            monsterRenderer = GetComponent<Renderer>();

        if (monsterRenderer != null)
            monsterMaterial = monsterRenderer.material;
        else
            Debug.LogError("MonsterAnimationController: Nie można znaleźć Renderer na obiekcie!");
    }

    public void PlayAnimation(AnimationType animationType, bool loop = true)
    {
        if (currentAnimation == animationType)
            return;

        StopCurrentAnimation();
        currentAnimation = animationType;

        if (debugMode)
            Debug.Log($"Odtwarzanie animacji: {animationType}");

        switch (animationType)
        {
            case AnimationType.Idle:
                animationCoroutine = StartCoroutine(PlaySpriteAnimation(idleSprites, idleFrameRate, loop));
                break;
            case AnimationType.Shaking:
                animationCoroutine = StartCoroutine(PlaySpriteAnimation(shakingSprites, shakingFrameRate, loop));
                break;
            case AnimationType.Transforming:
                animationCoroutine = StartCoroutine(PlaySpriteAnimation(transformingSprites, transformingFrameRate, false));
                break;
            case AnimationType.Running:
                animationCoroutine = StartCoroutine(PlaySpriteAnimation(runningSprites, runningFrameRate, loop));
                break;
        }
    }

    public void StopCurrentAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
        currentAnimation = AnimationType.None;
    }

    private IEnumerator PlaySpriteAnimation(Texture[] sprites, float frameRate, bool loop)
    {
        if (sprites == null || sprites.Length == 0 || monsterMaterial == null)
        {
            Debug.LogWarning("MonsterAnimationController: Brak sprite'ów lub materiału do animacji!");
            yield break;
        }

        float frameDuration = 1f / frameRate;
        int currentFrame = 0;

        do
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (sprites[i] != null)
                {
                    monsterMaterial.mainTexture = sprites[i];

                    if (debugMode)
                        Debug.Log($"Klatka animacji: {i}/{sprites.Length - 1}");
                }

                yield return new WaitForSeconds(frameDuration);

                // Przerwij animację jeśli została zatrzymana
                if (currentAnimation == AnimationType.None)
                    yield break;
            }

            currentFrame++;

        } while (loop);

        // Po zakończeniu animacji nieprętlonej, wyczyść currentAnimation ale nie przełączaj automatycznie
        if (!loop)
        {
            currentAnimation = AnimationType.None;
        }
    }

    public AnimationType GetCurrentAnimation()
    {
        return currentAnimation;
    }

    public bool IsAnimationPlaying(AnimationType animationType)
    {
        return currentAnimation == animationType && animationCoroutine != null;
    }

    // Metody pomocnicze do szybkiego dostępu
    public void PlayIdleAnimation()
    {
        PlayAnimation(AnimationType.Idle);
    }

    public void PlayShakingAnimation()
    {
        PlayAnimation(AnimationType.Shaking);
    }

    public void PlayTransformingAnimation()
    {
        PlayAnimation(AnimationType.Transforming, false);
    }

    public void PlayRunningAnimation()
    {
        PlayAnimation(AnimationType.Running);
    }

    void OnDestroy()
    {
        StopCurrentAnimation();
    }

    // Metoda do testowania animacji w edytorze
    [Button("Test Idle")]
    private void TestIdle()
    {
        if (Application.isPlaying)
            PlayIdleAnimation();
    }

    [Button("Test Shaking")]
    private void TestShaking()
    {
        if (Application.isPlaying)
            PlayShakingAnimation();
    }

    [Button("Test Transforming")]
    private void TestTransforming()
    {
        if (Application.isPlaying)
            PlayTransformingAnimation();
    }

    [Button("Test Running")]
    private void TestRunning()
    {
        if (Application.isPlaying)
            PlayRunningAnimation();
    }
}
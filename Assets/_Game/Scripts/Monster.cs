using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;
using Core.Web;

public class Monster : MonoBehaviour
{
    public static Monster Instance;

    [SerializeField][MinMaxSlider(0, 60)] Vector2 spawnTimeRange = new Vector2(10, 60);
    [SerializeField] GameObject monsterVisuals;
    [SerializeField] float spawnDistanceBehindPlayer = 5f;
    [SerializeField] AudioSource monsterAudioSource;
    [SerializeField] float chaseSpeed = 8f;
    [SerializeField] float stopDistance = 2f;
    [SerializeField] AudioClip jumpscareSound;
    [SerializeField] AudioClip approachingSound;
    [SerializeField] AudioClip footstepsSound;
    [SerializeField] AudioClip flashSound;
    [SerializeField] GameObject jumpscareEffect;
    [SerializeField] float waitTimeout = 1.5f;
    [SerializeField] MonsterAnimationController animationController;
    [SerializeField] bool waitForPlayerToLeaveSpawnArea = true;
    [SerializeField] bool debugMonsterActivation = false;
    [SerializeField] GameObject blackScreenOverlay;
    [SerializeField] AudioSource backgroundMusicSource;
    [SerializeField] AudioSource footstepsAudioSource;
    [SerializeField] float footstepVolume = 0.5f;
    public bool isFlashed = false;

    private bool monsterActivated = false;

    public GameObject BlackScreenOverlay => blackScreenOverlay;
    public AudioSource BackgroundMusicSource => backgroundMusicSource;

    private Coroutine monsterActivation = null;

    public bool MonsterActivated { get => monsterActivated; set => monsterActivated = value; }

    void Awake()
    {
        if (Instance == null) Instance = this;

        // Subskrybuj wydarzenie opuszczenia obszaru spawnu
        SpawnAreaTrigger.OnPlayerLeftSpawnArea += ActivateMonster;
    }

    void Start()
    {
        if (animationController == null)
            animationController = GetComponent<MonsterAnimationController>();

        if (!waitForPlayerToLeaveSpawnArea)
        {
            // Jeśli nie czekamy na wyjście gracza, aktywuj od razu
            ActivateMonster();
        }
        else
        {
            if (debugMonsterActivation)
                Debug.Log("Monster: Oczekiwanie na wyjście gracza z obszaru spawnu...");
        }
    }

    public void DactivateMonster()
    {
        if (!monsterActivated)
            return;

        monsterActivated = false;

        if (monsterActivation != null)
        {
            StopAllCoroutines();
            monsterActivation = null;
        }

        if (monsterAudioSource != null)
        {
            monsterAudioSource.Stop();
        }

        StopFootstepsSound();
    }

    private void ActivateMonster()
    {
        if (monsterActivated) return;

        monsterActivated = true;

        if (debugMonsterActivation)
            Debug.Log("Monster: AKTYWOWANY! Rozpoczynam cykl monstera...");

        monsterActivation = StartCoroutine(MonsterCycle());
    }
    IEnumerator MonsterCycle()
    {
        while (monsterActivated)
        {
            if (!monsterActivated)
                yield break;

            if (isFlashed)
            {
                isFlashed = false;
                continue;
            }

            float waitTime = Random.Range(spawnTimeRange.x, spawnTimeRange.y);
            yield return new WaitForSeconds(waitTime);

            Vector3 positionBehindPlayer = GetPositionBehindPlayer(spawnDistanceBehindPlayer);
            transform.position = positionBehindPlayer;
            monsterVisuals.SetActive(true);


            // Rozpocznij animację drżenia po pojawieniu się
            if (animationController != null)
                animationController.PlayShakingAnimation();


            if (monsterAudioSource != null)
            {
                monsterAudioSource.clip = approachingSound;
                monsterAudioSource.Play();
            }

            for (int i = 0; i < waitTimeout * 10; i++)
            {

                if (!monsterActivated)
                    yield break;

                if (isFlashed)
                {
                    //monsterVisuals.SetActive(false);
                    //if (animationController != null)
                    //    animationController.StopCurrentAnimation();
                    //if (monsterAudioSource != null)
                    //{
                    //    monsterAudioSource.Stop();
                    //}

                    //yield return new WaitForSeconds(1f);
                    break;
                }
                yield return new WaitForSeconds(0.1f);
            }

            if (isFlashed)
            {
                isFlashed = false;
                continue;
            }

            if (!monsterActivated)
                yield break;

            // Przejdź do animacji transformacji przed rozpoczęciem pościgu
            if (animationController != null)
                animationController.PlayTransformingAnimation();


            yield return new WaitForSeconds(waitTimeout);

            if (isFlashed)
            {
                isFlashed = false;
                continue;
            }

            yield return StartCoroutine(ChasePlayer());

            monsterVisuals.SetActive(false);

            // Zatrzymaj animacje po zakończeniu cyklu
            if (animationController != null)
                animationController.StopCurrentAnimation();

        }

        monsterActivation = null;
    }

    Vector3 GetPositionBehindPlayer(float distance = 5f, float heightOffset = 0f)
    {
        if (PlayerMovement.Instance == null)
        {
            Debug.LogWarning("PlayerMovement.Instance jest null!");
            return Vector3.zero;
        }

        Transform player = PlayerMovement.Instance.transform;

        Vector3 playerForward = player.forward;

        Vector3 behindDirection = -playerForward;

        Vector3 behindPosition = player.position + (behindDirection * distance);

        behindPosition.y = 0;

        return behindPosition;
    }

    Vector3 GetRandomPositionBehindPlayer(float distance = 5f, float randomSpread = 2f, float heightOffset = 0f)
    {
        Vector3 basePosition = GetPositionBehindPlayer(distance, heightOffset);

        Vector3 randomOffset = new Vector3(
            Random.Range(-randomSpread, randomSpread),
            0f,
            Random.Range(-randomSpread, randomSpread)
        );

        return basePosition + randomOffset;
    }

    IEnumerator ChasePlayer()
    {
        if (!monsterActivated)
            yield break;

        if (PlayerMovement.Instance == null)
        {
            Debug.LogWarning("Nie można ścigać gracza - PlayerMovement.Instance jest null!");
            yield break;
        }

        // Rozpocznij animację biegania
        if (animationController != null)
            animationController.PlayRunningAnimation();

        // Rozpocznij odtwarzanie dźwięku kroków
        StartFootstepsSound();

        Vector3 startPosition = transform.position;

        while (true)
        {
            if (!monsterActivated)
                yield break;

            Transform player = PlayerMovement.Instance.transform;

            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0;
            Vector3 playerFeetPosition = new Vector3(player.position.x, 0, player.position.z);
            float distanceToPlayerFeet = Vector3.Distance(transform.position, playerFeetPosition);

            if (distanceToPlayerFeet <= stopDistance)
            {
                Debug.Log("JUMPSCARE! Potwór złapał gracza!");
                StopFootstepsSound();
                TriggerJumpscare();
                yield break;
            }

            if (isFlashed)
            {
                yield break;
            }


            Vector3 newPosition = transform.position + directionToPlayer * chaseSpeed * Time.deltaTime;
            transform.position = newPosition;

            yield return null;
        }
    }

    void TriggerJumpscare()
    {
        // Zatrzymaj animacje
        if (animationController != null)
            animationController.StopCurrentAnimation();

        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.canMove = false;
        }

        if (monsterAudioSource != null)
        {
            monsterAudioSource.Stop();
        }

        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
        }

        if (jumpscareSound != null && monsterAudioSource != null)
        {
            monsterAudioSource.clip = jumpscareSound;
            monsterAudioSource.Play();
        }

        if (jumpscareEffect != null)
        {
            monsterVisuals.SetActive(false);
            jumpscareEffect.SetActive(true);
        }

        Debug.Log("JUMPSCARE AKTYWOWANY! Gra kończy się!");


        StartCoroutine(HandleGameOver());
    }

    IEnumerator HandleGameOver()
    {
        yield return new WaitForSeconds(1f);
        blackScreenOverlay.SetActive(true);
        yield return new WaitForSeconds(2f);

        MessageDisplayer.Instance.ResetCurrentMessage();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void FlashMonster()
    {
        Debug.LogError("Flash monster");
        isFlashed = true;

        // Odtwórz dźwięk flasha
        if (flashSound != null && monsterAudioSource != null)
        {
            monsterAudioSource.Stop(); // Zatrzymaj obecny dźwięk
            monsterAudioSource.PlayOneShot(flashSound);
        }

        monsterVisuals.SetActive(false);
        if (animationController != null)
            animationController.StopCurrentAnimation();
        StopFootstepsSound();
    }

    private void StartFootstepsSound()
    {
        if (footstepsAudioSource != null && footstepsSound != null)
        {
            footstepsAudioSource.clip = footstepsSound;
            footstepsAudioSource.volume = footstepVolume;
            footstepsAudioSource.loop = true;
            footstepsAudioSource.Play();
        }
    }

    private void StopFootstepsSound()
    {
        if (footstepsAudioSource != null)
        {
            footstepsAudioSource.Stop();
        }
    }

    void OnDestroy()
    {
        // Odsubskrybuj wydarzenia żeby uniknąć błędów
        SpawnAreaTrigger.OnPlayerLeftSpawnArea -= ActivateMonster;
    }


}
using UnityEngine;
using NaughtyAttributes;
using Core.Web;

public class SpawnAreaTrigger : MonoBehaviour
{
    [Header("Spawn Area Settings")]
    [SerializeField] private bool debugMode = true;
    [InfoBox("Ten collider określa obszar spawnu. Gdy gracz go opuści, potwór zostanie aktywowany.")]

    [Header("Events")]
    public static System.Action OnPlayerLeftSpawnArea;

    private bool playerInSpawnArea = false;
    private bool hasPlayerEverLeft = false;

    [Header("Debug Info")]
    [SerializeField][ReadOnly] private bool playerCurrentlyInArea = false;
    [SerializeField][ReadOnly] private bool monsterCanActivate = false;

    void Start()
    {
        // Sprawdź czy collider jest ustawiony jako trigger
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError("SpawnAreaTrigger: Brak Collider na obiekcie!");
            return;
        }

        if (!col.isTrigger)
        {
            Debug.LogWarning("SpawnAreaTrigger: Collider nie jest ustawiony jako Trigger! Ustawianie automatycznie...");
            col.isTrigger = true;
        }

        if (debugMode)
            Debug.Log("SpawnAreaTrigger: Inicjalizacja zakończona. Oczekiwanie na gracza...");
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            playerInSpawnArea = true;
            playerCurrentlyInArea = true;

            if (debugMode)
                Debug.Log("SpawnAreaTrigger: Gracz wszedł do obszaru spawnu");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            playerInSpawnArea = false;
            playerCurrentlyInArea = false;

            if (!hasPlayerEverLeft)
            {
                hasPlayerEverLeft = true;
                monsterCanActivate = true;

                if (debugMode)
                    Debug.Log("SpawnAreaTrigger: Gracz po raz pierwszy opuścił obszar spawnu! Aktywuję potwora...");

                // Powiadom wszystkich słuchaczy, że gracz opuścił obszar spawnu
                OnPlayerLeftSpawnArea?.Invoke();

                MessageDisplayer.Instance.DisplayNextMessage();
            }
            else
            {
                if (debugMode)
                    Debug.Log("SpawnAreaTrigger: Gracz opuścił obszar spawnu (już wcześniej go opuszczał)");
            }
        }
    }

    private bool IsPlayer(Collider other)
    {
        // Sprawdź czy to gracz po komponencie PlayerMovement
        return other.GetComponent<PlayerMovement>() != null;
    }

    public bool CanMonsterActivate()
    {
        return hasPlayerEverLeft;
    }

    public bool IsPlayerInSpawnArea()
    {
        return playerInSpawnArea;
    }

    // Metody do debugowania w edytorze
    [Button("Simulate Player Left Area")]
    private void SimulatePlayerLeftArea()
    {
        if (Application.isPlaying)
        {
            if (!hasPlayerEverLeft)
            {
                hasPlayerEverLeft = true;
                monsterCanActivate = true;
                OnPlayerLeftSpawnArea?.Invoke();
                Debug.Log("SpawnAreaTrigger: Symulacja - gracz opuścił obszar spawnu!");
            }
            else
            {
                Debug.Log("SpawnAreaTrigger: Gracz już wcześniej opuścił obszar spawnu");
            }
        }
    }

    [Button("Reset Spawn Area State")]
    private void ResetSpawnAreaState()
    {
        if (Application.isPlaying)
        {
            hasPlayerEverLeft = false;
            playerInSpawnArea = false;
            playerCurrentlyInArea = false;
            monsterCanActivate = false;
            Debug.Log("SpawnAreaTrigger: Stan obszaru spawnu został zresetowany");
        }
    }

    void OnDrawGizmos()
    {
        // Rysuj obszar spawnu w Scene View
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = hasPlayerEverLeft ? Color.green : Color.yellow;
            if (playerInSpawnArea)
                Gizmos.color = Color.blue;

            Gizmos.matrix = transform.localToWorldMatrix;

            if (col is BoxCollider)
            {
                BoxCollider boxCol = col as BoxCollider;
                Gizmos.DrawWireCube(boxCol.center, boxCol.size);
            }
            else if (col is SphereCollider)
            {
                SphereCollider sphereCol = col as SphereCollider;
                Gizmos.DrawWireSphere(sphereCol.center, sphereCol.radius);
            }
            else if (col is CapsuleCollider)
            {
                CapsuleCollider capsuleCol = col as CapsuleCollider;
                Gizmos.DrawWireCube(capsuleCol.center, new Vector3(capsuleCol.radius * 2, capsuleCol.height, capsuleCol.radius * 2));
            }
        }
    }
}
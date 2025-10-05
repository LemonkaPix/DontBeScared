using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Pomocniczy skrypt do wizualizacji i testowania systemu aktywacji monstera
/// </summary>
public class MonsterActivationDebugger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Monster monster;
    [SerializeField] private SpawnAreaTrigger spawnAreaTrigger;

    [Header("Debug Display")]
    [SerializeField][ReadOnly] private bool monsterActivated = false;
    [SerializeField][ReadOnly] private bool playerLeftSpawnArea = false;
    [SerializeField][ReadOnly] private bool playerInSpawnArea = false;


    void Start()
    {
        if (monster == null)
            monster = FindObjectOfType<Monster>();

        if (spawnAreaTrigger == null)
            spawnAreaTrigger = FindObjectOfType<SpawnAreaTrigger>();

        // Subskrybuj wydarzenia
        SpawnAreaTrigger.OnPlayerLeftSpawnArea += OnPlayerLeftSpawnArea;
    }

    void Update()
    {
        UpdateDebugInfo();
    }

    private void UpdateDebugInfo()
    {
        if (spawnAreaTrigger != null)
        {
            playerLeftSpawnArea = spawnAreaTrigger.CanMonsterActivate();
            playerInSpawnArea = spawnAreaTrigger.IsPlayerInSpawnArea();
        }
    }

    private void OnPlayerLeftSpawnArea()
    {
        monsterActivated = true;
        Debug.Log("MonsterActivationDebugger: Otrzymano sygnał - gracz opuścił obszar spawnu!");
    }

    [Button("Force Activate Monster")]
    private void ForceActivateMonster()
    {
        if (Application.isPlaying && monster != null)
        {
            // Wymusz aktywację przez wywołanie wydarzenia
            SpawnAreaTrigger.OnPlayerLeftSpawnArea?.Invoke();
            Debug.Log("MonsterActivationDebugger: Wymuszona aktywacja monstera!");
        }
    }

    [Button("Test Spawn Area Exit")]
    private void TestSpawnAreaExit()
    {
        if (Application.isPlaying && spawnAreaTrigger != null)
        {
            // Symuluj wyjście gracza z obszaru spawnu
            SpawnAreaTrigger.OnPlayerLeftSpawnArea?.Invoke();
            Debug.Log("MonsterActivationDebugger: Symulacja wyjścia gracza z obszaru spawnu!");
        }
    }

    [Button("Show Current Status")]
    private void ShowCurrentStatus()
    {
        Debug.Log("=== MONSTER ACTIVATION STATUS ===");
        Debug.Log($"Monster Activated: {monsterActivated}");
        Debug.Log($"Player Left Spawn Area: {playerLeftSpawnArea}");
        Debug.Log($"Player Currently In Spawn Area: {playerInSpawnArea}");

        if (monster != null)
        {
            Debug.Log($"Monster Instance Found: {monster.name}");
        }
        else
        {
            Debug.Log("Monster Instance: NULL");
        }

        if (spawnAreaTrigger != null)
        {
            Debug.Log($"Spawn Area Trigger Found: {spawnAreaTrigger.name}");
        }
        else
        {
            Debug.Log("Spawn Area Trigger: NULL");
        }

        Debug.Log("================================");
    }

    void OnDestroy()
    {
        SpawnAreaTrigger.OnPlayerLeftSpawnArea -= OnPlayerLeftSpawnArea;
    }

    void OnGUI()
    {
        if (!Application.isPlaying) return;

        // Wyświetl informacje na ekranie podczas gry
        GUILayout.BeginArea(new Rect(10, 10, 300, 150));
        GUILayout.BeginVertical("box");

        GUILayout.Label("Monster Activation Debug", "label");
        GUILayout.Space(5);

        GUILayout.Label($"Monster Activated: {(monsterActivated ? "YES" : "NO")}");
        GUILayout.Label($"Player Left Spawn: {(playerLeftSpawnArea ? "YES" : "NO")}");
        GUILayout.Label($"Player In Spawn: {(playerInSpawnArea ? "YES" : "NO")}");

        GUILayout.Space(10);

        if (GUILayout.Button("Force Activate Monster"))
        {
            ForceActivateMonster();
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
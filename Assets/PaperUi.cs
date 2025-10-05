using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaperUi : MonoBehaviour
{
    public static PaperUi Instance;
    [SerializeField] private GameObject paperUI;
    [SerializeField] TMP_Text paperText;
    [SerializeField] Button closeButton;

    public static bool IsReadingPaper { get; private set; } = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        paperUI.SetActive(false);
    }

    public void ShowPaper(string text, bool ignoreTimeScale = false)
    {
        paperText.text = text;
        paperUI.SetActive(true);

        if(!ignoreTimeScale)
            Time.timeScale = 0f; // Zatrzymaj czas gry
        Cursor.lockState = CursorLockMode.None; // Odblokuj kursor
        Cursor.visible = true; // Pokaż kursor

        // Ustaw flagę czytania papieru
        IsReadingPaper = true;

        // Zablokuj ruch gracza
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.canMove = false;
        }
    }

    public void HidePaper()
    {
        paperUI.SetActive(false);
        Time.timeScale = 1f; // Wznów czas gry
        Cursor.lockState = CursorLockMode.Locked; // Zablokuj kursor
        Cursor.visible = false; // Ukryj kursor

        // Wyłącz flagę czytania papieru
        IsReadingPaper = false;

        // Przywróć ruch gracza
        if (PlayerMovement.Instance != null)
        {
            PlayerMovement.Instance.canMove = true;
        }
    }

    public void BlockPaperClose()
    {
        closeButton.gameObject.SetActive(false);
    }
}

using AYellowpaper.SerializedCollections;
using Core.Web;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using URPGlitch;

public enum CellState
{
    Empty,
    Mine,
    Player,
    Exit,
    Border
}

public class GridController : MonoBehaviour
{


    [SerializedDictionary("Coordinates", "State")]
    [SerializeField] Vector2Int playerPos;
    [SerializeField] TMP_Text gridText;
    [SerializeField] TMP_Text pathText;
    [SerializeField] TMP_Text fileMessage;
    [SerializeField] Volume volume;
    [SerializeField] private float messageDuration = 3f;

    [SerializeField] AudioSource minigameAudioSource;
    [SerializeField] AudioClip mineSound;
    [SerializeField] float glitchDuration = 3f;
    [SerializeField] AudioClip winSound;


    private int gridSize = 10;
    private Vector2Int currentPlayerPos = new Vector2Int(1, 1);

    bool gameWon = false;
    bool endGame = false;
    private Coroutine messageCoroutine = null;

    public static GridController instance;
    DigitalGlitchVolume digitalGlitch;
    IEnumerator Start()
    {
        instance = this;
        DrawGrid(true);

        if (volume == null || !volume.profile.TryGet(out digitalGlitch))
        {
            Debug.LogError("Nie znaleziono profilu Volume lub efektu digitalglitch.");
        }

        digitalGlitch.intensity.value = 0.02f;

        yield return new WaitForSeconds(.3f);
        MessageDisplayer.Instance.DisplayNextMessage();
        MessageDisplayer.Instance.DisplayNextMessage();
        MessageDisplayer.Instance.DisplayNextMessage();
    }


    string[,] level1Map = new string[,]
    {
        {"[#]", "[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]"},
        {"[#]", "[@]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[$]","[#]"},
        {"[#]", "[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]"}
    };

    string[,] level1Mines = new string[,]
    {
        {"[#]", "[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]"},
        {"[#]", "[ ]","[x]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[ ]","[x]","[ ]","[x]","[ ]","[x]","[x]","[x]","[x]","[ ]","[#]"},
        {"[#]", "[ ]","[x]","[ ]","[x]","[ ]","[x]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[ ]","[x]","[ ]","[x]","[ ]","[x]","[x]","[x]","[x]","[ ]","[#]"},
        {"[#]", "[ ]","[ ]","[ ]","[x]","[ ]","[x]","[x]","[x]","[x]","[ ]","[#]"},
        {"[#]", "[x]","[x]","[x]","[ ]","[ ]","[x]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[x]","[x]","[ ]","[ ]","[x]","[x]","[ ]","[x]","[x]","[x]","[#]"},
        {"[#]", "[ ]","[ ]","[ ]","[x]","[x]","[ ]","[ ]","[x]","[x]","[x]","[#]"},
        {"[#]", "[ ]","[x]","[x]","[x]","[x]","[ ]","[x]","[x]","[x]","[x]","[#]"},
        {"[#]", "[ ]","[ ]","[ ]","[ ]","[x]","[ ]","[ ]","[ ]","[ ]","[ ]","[#]"},
        {"[#]", "[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]"}
    };

    string[,] level1Win = new string[,]
{
        {"[#]", "[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]"},
        {"[#]", "[1]","[0]","[1]","[1]","[1]","[1]","[1]","[1]","[1]","[1]","[#]"},
        {"[#]", "[1]","[0]","[1]","[0]","[0]","[0]","[0]","[0]","[0]","[1]","[#]"},
        {"[#]", "[1]","[0]","[1]","[0]","[0]","[0]","[0]","[0]","[0]","[1]","[#]"},
        {"[#]", "[1]","[0]","[1]","[0]","[0]","[0]","[0]","[0]","[0]","[1]","[#]"},
        {"[#]", "[1]","[1]","[1]","[0]","[0]","[0]","[0]","[0]","[0]","[1]","[#]"},
        {"[#]", "[0]","[0]","[0]","[0]","[0]","[0]","[1]","[1]","[1]","[1]","[#]"},
        {"[#]", "[0]","[0]","[0]","[0]","[0]","[0]","[1]","[0]","[0]","[0]","[#]"},
        {"[#]", "[0]","[0]","[0]","[0]","[0]","[1]","[1]","[0]","[0]","[0]","[#]"},
        {"[#]", "[0]","[0]","[0]","[0]","[0]","[1]","[0]","[0]","[0]","[0]","[#]"},
        {"[#]", "[0]","[0]","[0]","[0]","[0]","[1]","[1]","[1]","[1]","[1]","[#]"},
        {"[#]", "[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]","[#]"}
};

    string[,] levelInUse;



    void Update()
    {
        if (gameWon)
            return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            MovePlayer('w');
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MovePlayer('a');

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MovePlayer('s');

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MovePlayer('d');
        }
    }


    void MovePlayer(char dir)
    {
        Vector2Int oldPlayerPos = currentPlayerPos;
        switch (dir)
        {
            case 'w':
                if (currentPlayerPos.x == 1)
                    return;
                currentPlayerPos.x -= 1;
                break;
            case 'a':
                if (currentPlayerPos.y == 1)
                    return;
                currentPlayerPos.y -= 1;
                break;
            case 's':
                if (currentPlayerPos.x == gridSize)
                    return;
                currentPlayerPos.x += 1;
                break;
            case 'd':
                if (currentPlayerPos.y == gridSize)
                    return;
                currentPlayerPos.y += 1;
                break;
        }

        DisableOldPlayer(oldPlayerPos);

        if (level1Mines[currentPlayerPos.x, currentPlayerPos.y] == "[x]")
        {
            ResetPlayerAndDisplayMine();
        }
        else if (level1Map[currentPlayerPos.x, currentPlayerPos.y] == "[$]")
        {
            gameWon = true;
            level1Map = level1Win;
            DrawGrid();
            pathText.text = RandomFileCreation.Instance.RandomGridFile;
            pathText.gameObject.SetActive(true);
            if (minigameAudioSource != null)
            {
                minigameAudioSource.clip = winSound;
                minigameAudioSource.Play();
            }
            StartCoroutine(DisplayMessageWithDelay());
            return;
        }


        level1Map[currentPlayerPos.x, currentPlayerPos.y] = "[@]";
        DrawGrid();
    }


    void ResetPlayerAndDisplayMine()
    {
        if (minigameAudioSource != null)
        {
            minigameAudioSource.clip = mineSound;
            minigameAudioSource.Play();

        }
        level1Map[currentPlayerPos.x, currentPlayerPos.y] = "[x]";
        currentPlayerPos = new Vector2Int(1, 1);
    }

    void DisableOldPlayer(Vector2Int oldPosition)
    {
        level1Map[oldPosition.x, oldPosition.y] = "[ ]";
    }

    [Button]
    public void DrawGrid(bool initial = false)
    {
        gridText.text = "<mspace=.5em>";

        float arraySize = Mathf.Sqrt(level1Map.Length);

        for (int x = 0; x < arraySize; x++)
        {
            string line = "";
            for (int y = 0; y < arraySize; y++)
            {
                line += level1Map[x, y];
            }
            gridText.text += line + "\n";
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!gameWon || endGame)
            return;


        if (hasFocus)
        {
            List<string> fileLines = RandomFileCreation.Instance.GetGridFile();

            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
                fileMessage.gameObject.SetActive(false);
                messageCoroutine = null;
            }

            for (int i = 0; i < fileLines.Count; i++)
            {
                string[] spllitedText = fileLines[i].Split(" ");

                for (int j = 0; j < spllitedText.Length; j++)
                {
                    if (spllitedText[j] != level1Win[i, j][1].ToString())
                    {
                        messageCoroutine = StartCoroutine(DisplayFileMessage("File is incorrect, fix it!"));
                        return;
                    }
                }
            }

            endGame = true;
            messageCoroutine = StartCoroutine(DisplayFileMessage("File is correct!", true));
        }
    }

    [Button]
    public void Win()
    {
        StartCoroutine(DisplayFileMessage("File is correct!", true));
    }

    private IEnumerator DisplayFileMessage(string message, bool displayMessage = false)
    {
        fileMessage.text = message;
        fileMessage.gameObject.SetActive(true);

        if (displayMessage)
        {
            yield return new WaitForSeconds(0.1f);
            MessageDisplayer.Instance.DisplayNextMessage();

            float elapsed = 0f;


            yield return new WaitForSeconds(0.5f);
            while (elapsed < glitchDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / glitchDuration);
                digitalGlitch.intensity.value = Mathf.Lerp(0f, 1f, t);
                yield return null; // wait for next frame
            }
            digitalGlitch.intensity.value = 1f;
            yield return new WaitForSeconds(.3f);

            SceneManager.LoadScene("SampleScene");
            messageCoroutine = null;
            yield break;
        }

        yield return new WaitForSecondsRealtime(messageDuration);
        fileMessage.gameObject.SetActive(false);
        messageCoroutine = null;
    }

    private IEnumerator DisplayMessageWithDelay()
    {
        yield return new WaitForSeconds(0.1f);
        MessageDisplayer.Instance.DisplayNextMessage();
        MessageDisplayer.Instance.DisplayNextMessage();
        RandomFileCreation.Instance.OpenDirectory();
    }

    private void OnDestroy()
    {
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
            messageCoroutine = null;
        }
    }
}

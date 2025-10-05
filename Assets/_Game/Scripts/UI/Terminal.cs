using Core.Web;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using URPGlitch;

public class Terminal : MonoBehaviour
{
    public static Terminal Instance;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] TMP_InputField input;
    [SerializeField] TMP_Text output;

    [Header("Enable/Disable")] [SerializeField]
    TMP_InputField inputField;

    public bool activated = false;

    [Header("Booting")]
    [TextArea(10, 20)]
    [SerializeField] string bootMessage;
    [SerializeField] GameObject bootTextSpamField;
    [SerializeField] GameObject notBootedAlert;
    [SerializeField] Volume volume;
    public GameObject terminalMainPanel;
    public GameObject minigame;
    [SerializeField] float timeBetweenTexts = 0.15f;
    public bool connected = false;
    DigitalGlitchVolume digitalGlitch;
    CommandHandler commandHandler;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (volume == null || !volume.profile.TryGet(out digitalGlitch))
        {
            Debug.LogError("Nie znaleziono profilu Volume lub efektu digitalglitch.");
        }
        digitalGlitch.intensity.value = 0.01f;

        if (PlayerPrefs.GetInt("BLOCK_GAME") == 1)
        {
            return;
        }

        StartMethod();
    }

    public void StartMethod()
    {
        RandomFileCreation.Instance.CreateDirectory();
        Cursor.lockState = CursorLockMode.Locked;
        commandHandler = GetComponent<CommandHandler>();

        ShowMessage("Type help to see available commands.");

        TerminalBoot();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && activated)
        {
            EnterCommand();
        }
        if(activated) input.ActivateInputField();

    }

    void TerminalBoot()
    {
        activated = true;
        notBootedAlert.SetActive(false);
        bootTextSpamField.SetActive(true);


        StartCoroutine(bootSpamMsg());
    }

    IEnumerator bootSpamMsg()
    {
        TMP_Text spamField = bootTextSpamField.GetComponent<TMP_Text>();
        string[] messages = bootMessage.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        for (int i = messages.Length - 1; i >= 0; i--)
        {
            spamField.text = messages[i] + "\n" + spamField.text;
            if(i == messages.Length - 1)
                yield return new WaitForSeconds(1.5f);
            else 
                yield return new WaitForSeconds(timeBetweenTexts);

        }
        yield return new WaitForSeconds(1f);
        bootTextSpamField.SetActive(false);
        terminalMainPanel.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        MessageDisplayer.Instance.DisplayNextMessage();
        input.ActivateInputField();

        activated = true;
    }

    public void EnterCommand()
    {
        if (!activated) return;
        string[] command = input.text.Split(" ");
        string cmd = command[0];
        string[] args = command.Skip(1).ToArray();
        input.text = "";
        input.ActivateInputField();
        string message;
        if(!connected)
        message = "C:\\users\\Administrator>" + string.Join(" ", command) + "\n";
        else message = "admin%admin " + string.Join(" ", command) + "\n";

        output.text += message;
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        commandHandler.FindCommand(cmd, args);
    }

    public void ShowMessage(string message)
    {
        output.text += message + "\n";
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void ShowMessageNoNewline(string message)
    {
        output.text += message;
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void ClearTerminal()
    {
        output.text = "";
        Canvas.ForceUpdateCanvases();

    }

    public void DeactivateField()
    {
        input.readOnly = true;
        input.DeactivateInputField();
        activated = false;
    }

}
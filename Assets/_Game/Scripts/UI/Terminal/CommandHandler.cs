using AYellowpaper.SerializedCollections;
using System.Collections;
using TMPro;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
    [SerializeField] TMP_Text[] texts;

    [SerializedDictionary("Syntax", "Command Class")]
    public SerializedDictionary<string, Command> commands;
    [SerializeField] Terminal terminal;

    private void Awake()
    {
        RegisterCommand(new Command("help", "help: Shows descriptions about all availble commands"));
        RegisterCommand(new Command("genwrld", "a"));

    }

    void RegisterCommand(Command command)
    {
        commands[command.syntax.ToLower()] = command;
    }

    public void FindCommand(string syntax, string[] args = null)
    {
        if (syntax == "") return;

        string loweredSyntax = syntax.ToLower();

        if (!commands.ContainsKey(loweredSyntax)) { terminal.ShowMessage($"{loweredSyntax} is not recognized as an internal or external command."); return; }

        string response = "";

        if (loweredSyntax == "help")
        {

            print(terminal);
            terminal.ShowMessage("READY SCRIPTS FOR RUN (build 24.6.β)\r\n-------------------------------------------------\r\n[NE█INIT] - in█ti█lize so█ke%@t ch█n#el█ — TIm%eo@ut thrEsh█ld??? (ERR: %█#@?)\r\n[N█TSCAN] - deTeC█ ro#uTe a@n█maly / g@aTeway dr█p%% — pAc?Ket lOsS#%#@\r\n[DN█FLUSH] - cl█aR ca#█hE / re%so█vE pr█cESS un@St█ble?? — Acks dropp█d#@%%\r\n[GENWRLD] - Initiate a connection.\r\n[FI█EWA█L] - EnfOr@ce poLi█y%... ru#leS█t corru█tEd@?? — cOnfig mi#s@%█ng?%\r\n[PO█TSTAT] - reTRI█ve po%█T l#i█t — fAIlEd tO bi█d%@@ (port ███#%@)\r\n-------------------------------------------------");
        }
        else if(loweredSyntax == "genwrld")
        {
            terminal.DeactivateField();
            StartCoroutine(ConnectionScript());
        }
        else
        {
            response = commands[loweredSyntax].Execute(args);
            terminal.ShowMessage(response);
        }

    }

    public IEnumerator ConnectionScript()
    {
        terminal.ClearTerminal();
        terminal.ShowMessageNoNewline("TRYING TO GENERATE A WORLD");
        yield return new WaitForSeconds(.75f);
        terminal.ShowMessageNoNewline(".");
        yield return new WaitForSeconds(.75f);
        terminal.ShowMessageNoNewline(".");
        yield return new WaitForSeconds(.75f);
        terminal.ShowMessageNoNewline(".");
        yield return new WaitForSeconds(.75f);
        terminal.ShowMessageNoNewline(".");
        yield return new WaitForSeconds(1.5f);
        terminal.ShowMessage("");
        terminal.ShowMessage("ESTABLISHED CONNECTION");
        terminal.connected = true;

        for (int i = 0; i < 50; i++)
        {
            terminal.ShowMessage("########################################################################");
            yield return new WaitForSeconds(0.01f);
        }
        terminal.ShowMessage("");
        yield return new WaitForSeconds(0.5f);
        terminal.ShowMessage("Hello :)");
        yield return new WaitForSeconds(5f);
        terminal.terminalMainPanel.SetActive(false);
        terminal.minigame.SetActive(true);

        yield return new WaitForSeconds(0.1f);

    }
}

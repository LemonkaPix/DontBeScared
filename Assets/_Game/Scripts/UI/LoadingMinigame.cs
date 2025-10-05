using Core.Web;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class LoadingMinigame : MonoBehaviour
{
    [SerializeField] GameObject minigameLevel;
    private IEnumerator Start()
    {
        TMP_Text loadingText = GetComponent<TMP_Text>();

        yield return new WaitForSeconds(.35f);
        loadingText.text = "LOADING \n░░░░░";
        yield return new WaitForSeconds(.35f);
        loadingText.text = "LOADING \n▒░░░░";
        yield return new WaitForSeconds(.35f);
        loadingText.text = "LOADING \n▒▒░░░";
        yield return new WaitForSeconds(.35f);
        loadingText.text = "LOADING \n▒▒▒░░";
        yield return new WaitForSeconds(.35f);
        loadingText.text = "LOADING \n▒▒▒▒░";
        yield return new WaitForSeconds(.75f);
        loadingText.text = "LOADING \n▒▒▒▒▒";
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
        minigameLevel.SetActive(true);
    }
}

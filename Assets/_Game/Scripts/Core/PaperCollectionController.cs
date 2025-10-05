using Core.Web;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PaperCollectionController : Singleton<PaperCollectionController>
{
    private List<ItemPaper> itemPapers = new List<ItemPaper>();

    private List<string> separatedPath = new List<string>();

    string path = "";

    int currentPage = 0;
    bool checkEnd = false;
    bool grandeFinale = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        string path = RandomFileCreation.Instance.RandomDelFile;
        string[] subPath1 = path.Split(RandomFileCreation.Instance.MainDirectoryName);
        separatedPath.Add(subPath1[0] + RandomFileCreation.Instance.MainDirectoryName);

        string[] subPath2 = subPath1[1].Split("/");

        for (int i=1; i< subPath2.Length; i++)
        {
            separatedPath.Add("/" + subPath2[i]);
        }
    }

    public void RegisterNewPaper(ItemPaper itemPaper)
    {
        itemPapers.Add(itemPaper);
    }

    public void CollectPaper(ItemPaper itemPaper)
    {
        bool ignoreTimescale = false;
        path += separatedPath[currentPage];

        if(itemPapers.Count == 1)
        {
            ignoreTimescale = true;
        }

        PaperUi.Instance.ShowPaper(path, ignoreTimescale);
        currentPage++;
        itemPapers.Remove(itemPaper);
        Destroy(itemPaper.gameObject);

        if(itemPapers.Count == 0)
        {
            PaperUi.Instance.BlockPaperClose();
            checkEnd = true;
            StartCoroutine(WaitForPaper());
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!checkEnd || grandeFinale)
            return;

        if (focus)
        {
            if(!RandomFileCreation.Instance.CheckIfMainFileExists())
            {
                grandeFinale = true;
                StartCoroutine(FinishGame());
            }
        }
    }

    private IEnumerator WaitForPaper()
    {
        yield return new WaitForSeconds(0.1f);
        MessageDisplayer.Instance.DisplayNextMessage();
        RandomFileCreation.Instance.OpenDirectory();
        Time.timeScale = 0;
    }

    private IEnumerator FinishGame()
    {
        Monster.Instance.DactivateMonster();
        Time.timeScale = 1;
        PlayerPrefs.SetInt("BLOCK_GAME", 1);
        PlayerPrefs.Save();
        Monster.Instance.BlackScreenOverlay.SetActive(true);
        if (Monster.Instance.BackgroundMusicSource != null)
        {
            Monster.Instance.BackgroundMusicSource.Stop();
        }
        yield return new WaitForSeconds(0.1f);
        MessageDisplayer.Instance.DisplayNextMessage();
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3f);
        CloseApp();
    }

    private void CloseApp()
    {
        Application.Quit();
    }
}

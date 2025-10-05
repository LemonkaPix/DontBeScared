using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class RandomFileCreation : Singleton<RandomFileCreation>
{
    [SerializeField]
    private int maxFolderDepth;

    [SerializeField]
    private int numberOfFolders;

    [SerializeField]
    private int itemsPerFolder;

    [SerializeField]
    private int gridSize;

    private string mainDirectoryName = "HELP_ME_MY_FRIEND";

    private string finalPath = "";

    private int fileNumberGrid = -1;
    private int fileNumberDel = -1;

    private List<int> folderNumbersGrid = new List<int>();
    private List<int> folderNumbersDel = new List<int>();

    string randomGridFile = "";
    string randomDelFile = "";

    public string RandomGridFile { get => randomGridFile; private set => randomGridFile = value; }
    public string RandomDelFile { get => randomDelFile; private set => randomDelFile = value; }
    public string MainDirectoryName => mainDirectoryName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }


    [Button("CreateDirectory")]
    public void CreateDirectory()
    {
        finalPath = Application.persistentDataPath.Split("LocalLow")[0] + "Local/TEMP/" + mainDirectoryName;

        fileNumberGrid = UnityEngine.Random.Range(0, Mathf.RoundToInt(itemsPerFolder / 2));
        fileNumberDel = UnityEngine.Random.Range(Mathf.RoundToInt(itemsPerFolder / 2), itemsPerFolder);

        for (int i = 0; i < maxFolderDepth; i++)
        {
            folderNumbersGrid.Add(UnityEngine.Random.Range(0, numberOfFolders));
            folderNumbersDel.Add(UnityEngine.Random.Range(0, numberOfFolders));
        }

        Directory.CreateDirectory(finalPath);

        GenerateRecursive(finalPath, new List<int>(), 1);
    }

    private void OnDestroy()
    {
        if (Directory.Exists(finalPath))
        {
            Directory.Delete(finalPath, true);
        }
    }


    private void GenerateRecursive(string path, List<int> folderCounts, int depth)
    {
        if (depth == maxFolderDepth + 1)
        {
            bool canBeRandomGrid = true;
            bool canBeRandomDel = true;

            for(int i=0; i<folderCounts.Count; i++)
            {
                if (folderNumbersGrid[i] != folderCounts[i])
                {
                    canBeRandomGrid = false;
                    break;
                }
            }

            for (int i = 0; i < folderCounts.Count; i++)
            {
                if (folderNumbersDel[i] != folderCounts[i])
                {
                    canBeRandomDel = false;
                    break;
                }
            }

            for (int i = 0; i < itemsPerFolder; i++)
            {
                string itemPath = "";

                if (i >= itemsPerFolder / 2)
                {
                    itemPath = path + $"/file{UnityEngine.Random.Range(100, 1000)}.log";
                }
                else
                {
                    itemPath = path + $"/file{UnityEngine.Random.Range(100, 1000)}.txt";
                }

                while (File.Exists(itemPath))
                {
                    if (i >= itemsPerFolder / 2)
                    {
                        itemPath = path + $"/file{UnityEngine.Random.Range(100, 1000)}.log";
                    }
                    else
                    {
                        itemPath = path + $"/file{UnityEngine.Random.Range(100, 1000)}.txt";
                    }
                }



                FileStream fileToEdit = File.Create(itemPath);
                fileToEdit.Close();

                if (canBeRandomGrid && fileNumberGrid == i)
                {
                    randomGridFile = itemPath;

                    string generatedGrid = "";

                    for(int j=0; j < gridSize; j++)
                    {
                        for(int k=0; k < gridSize; k++)
                        {
                            if( (k == 1 && j == 1) || (k == gridSize - 2 && j == gridSize - 2))
                            {
                                generatedGrid += "1";
                            }
                            else if(k == 0 || j == 0 || k == gridSize - 1 || j == gridSize - 1)
                            {
                                generatedGrid += "#";
                            }
                            else
                            {
                                generatedGrid += "0";
                            }

                            if(k != gridSize - 1)
                            {
                                generatedGrid += " ";
                            }
                        }

                        if(j != gridSize - 1)
                        {
                            generatedGrid += Environment.NewLine;
                        }

                    }

                    File.WriteAllText(itemPath, generatedGrid);

                }
                else if (canBeRandomDel && fileNumberDel == i)
                {
                    randomDelFile = itemPath;
                }

            }

            return;
        }

        for (int i = 0; i < numberOfFolders; i++)
        {
            string FolderPath = path + $"/FOLDER{UnityEngine.Random.Range(1000, 10000)}";
            Directory.CreateDirectory(FolderPath);
            folderCounts.Add(i);
            GenerateRecursive(FolderPath, folderCounts, depth + 1);
            folderCounts.RemoveAt(folderCounts.Count - 1);
        }
    }

    public List<string> GetGridFile()
    {
        List<string> readLine = new List<string>(File.ReadAllLines(RandomGridFile));
        return readLine;
    }

    public bool CheckIfMainFileExists()
    {
        return File.Exists(RandomDelFile);
    }

    public void OpenDirectory()
    {
        string path = Application.persistentDataPath.Split("LocalLow")[0] + "Local/TEMP/" + mainDirectoryName;

        if (!System.IO.Directory.Exists(path))
        {
            return;
        }

        Process.Start("explorer.exe", path.Replace("/", "\\"));
    }

}



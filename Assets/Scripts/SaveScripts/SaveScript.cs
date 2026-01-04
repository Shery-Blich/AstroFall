using System;
using System.IO;
using UnityEngine;

public class SaveScript : MonoBehaviour
{
    public static Action<int> OnLoadSaveData;

    public SaveScript Instance;

    private string savePath;

    private void Awake()
    {
        if(Instance == null)
        {
            savePath = $"{Application.persistentDataPath}/astrofallSaveFile.json";
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadGame();
    }

    private void OnEnable()
    {
        FallManager.GoodGameOver += SaveData;
        PlayerController.BadGameOver += SaveData;
    }

    private void OnDisable()
    {
        FallManager.GoodGameOver -= SaveData;
        PlayerController.BadGameOver -= SaveData;
    }

    private bool IsFileDateOlder(string fileDate, string otherDate)
    {
        var fileDateTime = DateTime.Parse(fileDate);
        var otherDateTime = DateTime.Parse(otherDate);

        return fileDateTime < otherDateTime;
    }

    public void SaveData()
    {
        if (MemoriesManager.Instance == null || MemoriesManager.Instance.CollectedMemories == 0)
        {
            Debug.Log("No memories collected. Skipping save.");
            return;
        }

        var collectedMemoriesInRound = MemoriesManager.Instance.CollectedMemories;
        var model = new SaveDataModel
        {
            TotalMemories = collectedMemoriesInRound,
            LastSaveDate = DateTime.Now.ToString()
        };

        var lastSaveData = LoadGame();

        if (lastSaveData != null)
        {
            model.TotalMemories += lastSaveData.TotalMemories;
        }

        var json = JsonUtility.ToJson(model);

        try
        {
            File.WriteAllText(savePath, json);
            Debug.Log($"Collected: {collectedMemoriesInRound}, in Total {model.TotalMemories} Memories were collected\nSaved to: {savePath}");
        }
        catch (Exception e)
        {
            Debug.Log($"Error trying to save new save file:\n{e.Message}");
        }     
    }

    public SaveDataModel LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found at: " + savePath + " Skipping load data");

            return null;
        }

        try
        {
            var json = File.ReadAllText(savePath);
            var loadedData = JsonUtility.FromJson<SaveDataModel>(json);
            Debug.Log($"Loaded {loadedData.TotalMemories} memories!");

            OnLoadSaveData?.Invoke(loadedData.TotalMemories);

            return loadedData;
        }
        catch (Exception e)
        {
            Debug.Log($"Error trying to load save file:\n{e.Message}");

            return null;
        }
    }
}
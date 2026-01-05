using Cysharp.Threading.Tasks;
using System;
using System.IO;
using UnityEngine;

public class SaveScript : MonoBehaviour
{
    public static Action<int> OnLoadSaveData;

    public static SaveScript Instance;

    private string savePath;

    public bool IsColdStart { get; set; } = true;

    private void Awake()
    {
        if(Instance == null)
        {
            
            savePath = Path.Combine(Application.persistentDataPath, "Saves", "astrofall.json");

            if (!Directory.Exists(Path.GetDirectoryName(savePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            }

            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

    public void SaveData()
    {
        if (MemoriesManager.Instance == null || MemoriesManager.Instance.CollectedMemories == 0)
        {
            print("No memories collected. Skipping save.");
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
            print($"Collected: {collectedMemoriesInRound}, in Total {model.TotalMemories} Memories were collected\nSaved to: {savePath}");
        }
        catch (Exception e)
        {
            Debug.Log($"Error trying to save new save file:\n{e.Message}");
        }     
    }

    public async UniTask LoadGameAsync()
    {
        if (!File.Exists(savePath))
        {
            print("No save file found at: " + savePath + " Skipping load data async");
            return;
        }

        try
        {
            var json = await File.ReadAllTextAsync(savePath);
            print("Finished reading save file async, Switching to Main thread to load file");
            await UniTask.SwitchToMainThread();

            var loadedData = JsonUtility.FromJson<SaveDataModel>(json);
            OnLoadSaveData?.Invoke(loadedData.TotalMemories);
            print($"Loaded {loadedData.TotalMemories} memories from: {savePath}");
        }
        catch (Exception e)
        {
            print($"Error trying to load save file:\n{e.Message}");
        }
    }

    public SaveDataModel LoadGame()
    {
        if (!File.Exists(savePath))
        {
            print("No save file found at: " + savePath + " Skipping load data");
            return null;
        }

        try
        {
            var json = File.ReadAllText(savePath);
            var loadedData = JsonUtility.FromJson<SaveDataModel>(json);
            print($"Loaded {loadedData.TotalMemories} memories from: {savePath}");

            OnLoadSaveData?.Invoke(loadedData.TotalMemories);

            return loadedData;
        }
        catch (Exception e)
        {
            print($"Error trying to load save file:\n{e.Message}");

            return null;
        }
    }
}
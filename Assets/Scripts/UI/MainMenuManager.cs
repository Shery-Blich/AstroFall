using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject storyScreen;
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject mainMenuScreen;
    [SerializeField] TextMeshProUGUI MemoriesCollectedCounter;

    private void Start()
    {
        //TODO: Add loading screen to not "Pop" the save data load on the screen
        // Cold Start - First time loading the game
        // On some andorid phones when being set to battery saving/flight mode
        // premission to the file system is slower to be granted
        // causing a freeze when trying to load the save data on start
        // To avoid this we use an async load on the first load
        if (SaveScript.Instance.IsColdStart)
        {
            SaveScript.Instance.IsColdStart = false;
            SaveScript.Instance.LoadGameAsync().Forget();
        }
        else
        {
            SaveScript.Instance.LoadGame();
        }

        SoundManager.Instance.PlayMusic(MusicTypeEnum.MainMenuMusic);
    }

    private void Update()
    {
        // This works on both keyboard and mobile back button
        // as Unity maps the mobile back button to the Escape key
        if (Keyboard.current?.escapeKey.wasPressedThisFrame == true)
        {
            HandleBackPressed();
        }
    }

    private void OnEnable()
    {
        SaveScript.OnLoadSaveData += UpdateMemoriesCounter;
    }

    private void OnDisable()
    {
        SaveScript.OnLoadSaveData -= UpdateMemoriesCounter;
    }

    private void UpdateMemoriesCounter(int LoadedMemoriesCounter)
    {
        this.MemoriesCollectedCounter.text = $"Memories Collected: {LoadedMemoriesCounter}";
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("Level Design");
    }

    public void OnOpenStory()
    {
        mainMenuScreen.SetActive(false);
        storyScreen.SetActive(true);
    }

    public void OnOpenCredits()
    {
        mainMenuScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    public void OnBackToMenu()
    {
        storyScreen.SetActive(false);
        creditsScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    private void HandleBackPressed()
    {
        print("Back pressed, returning to main menu if not there");
        if (!mainMenuScreen.activeSelf)
        {
            UIAudioManager.Instance.PlayClick();
            OnBackToMenu();
        }
    }
}

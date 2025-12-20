using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("UIPanels")]
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject panelStory;
    [SerializeField] private GameObject panelCredits;

    [SerializeField] private GameObject panelHUD;
    [SerializeField] private GameObject panelPause;

    [SerializeField] private GameObject panelGoodEnd;
    [SerializeField] private GameObject panelBadEnd;

    private bool isPlaying = false;

    private void OnEnable()
    {
        PlayerController.BadGameOver += OnBadEnd;
        FallManager.GoodGameOver += OnGoodEnd;
    }

    private void OnDisable()
    {
        PlayerController.BadGameOver -= OnBadEnd;
        FallManager.GoodGameOver -= OnGoodEnd;
    }

    private void Start()
    {
        ShowMainMenu();
        Time.timeScale = 0f;
    }

    //  BUTTONS
    public void Story()
    {
        panelStory.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Credits()
    {
        panelCredits.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Pause()
    {
        if (!isPlaying) return;

        panelPause.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        panelPause.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (FallManager.Instance != null)
        {
            FallManager.Instance.ResetRun();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Play()
    {
        panelStory.SetActive(false);
        panelCredits.SetActive(false);
        panelMainMenu.SetActive(false);
        panelHUD.SetActive(true);
        panelPause.SetActive(false);
        panelGoodEnd.SetActive(false);
        panelBadEnd.SetActive(false);

        StartRun();
    }

    public void MainMenu()
    {
        panelStory.SetActive(false);
        panelCredits.SetActive(false);
        Time.timeScale = 0f;
        ShowMainMenu();
    }

    // INTERNAL FLOW & keeping only one active panel at a time
    private void StartRun()
    {
        isPlaying = true;
        Time.timeScale = 1f;
    }

    private void ShowMainMenu()
    {
        isPlaying = false;

        Time.timeScale = 0f;

        panelMainMenu.SetActive(true);
        panelStory.SetActive(false);
        panelCredits.SetActive(false);

        panelHUD.SetActive(false);
        panelPause.SetActive(false);
        panelGoodEnd.SetActive(false);
        panelBadEnd.SetActive(false);
    }

    private void OnGoodEnd()
    {
        isPlaying = false;

        Time.timeScale = 0f;

        panelHUD.SetActive(false);
        panelPause.SetActive(false);
        panelGoodEnd.SetActive(true);
        panelBadEnd.SetActive(false);
    }

    private void OnBadEnd()
    {
        isPlaying = false;

        Time.timeScale = 0f;

        panelHUD.SetActive(false);
        panelPause.SetActive(false);
        panelGoodEnd.SetActive(false);
        panelBadEnd.SetActive(true);
    }

}

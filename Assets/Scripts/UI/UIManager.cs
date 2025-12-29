using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;

    private bool isPaused = false;
    private bool isGameEnded = false;

    private void Awake()
    {
        // all panels are closed
        pauseScreen.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    private void OnEnable()
    {
        FallManager.GoodGameOver += OnWin;
        PlayerController.BadGameOver += OnLose;
    }

    private void OnDisable()
    {
        FallManager.GoodGameOver -= OnWin;
        PlayerController.BadGameOver -= OnLose;
    }

    //TODO: Talk with course leads about the best practice here
    private void Update()
    {
        // This works on both keyboard and mobile back button
        // as Unity maps the mobile back button to the Escape key
        if (Keyboard.current?.escapeKey.wasPressedThisFrame == true)
        {
            HandleBackPressed();
        }
    }

    private void HandleBackPressed()
    {
        print("Back pressed, toggling pause menu");
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    //  PAUSE 

    public void Pause()
    {
        if (isPaused || isGameEnded) return;

        isPaused = true;
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
    }

    public void Resume()
    {
        if (!isPaused || isGameEnded) return;

        isPaused = false;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }

    //  GAME END 
    private void OnWin()
    {
        if (isGameEnded) return;

        isGameEnded = true;
        Time.timeScale = 0f;

        winScreen.SetActive(true);
    }

    private void OnLose()
    {
        if (isGameEnded) return;

        isGameEnded = true;
        Time.timeScale = 0f;

        loseScreen.SetActive(true);
    }

    // NAVIGATION 

    public void RestartGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level Design");
        ScreenController.Instance.SetScreen();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}

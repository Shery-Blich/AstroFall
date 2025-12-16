using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIFlowController : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject endScreen;

    [Header("End Screen")]
    [SerializeField] TextMeshProUGUI distanceText;

    private void Awake()
    {
        // start paused with start screen
        Time.timeScale = 0;
        startScreen.SetActive(true);
        pauseScreen.SetActive(false);
        endScreen.SetActive(false);
    }

    private void OnEnable()
    {
        PlayerController.BadGameOver += ShowEndScreen;
        FallManager.GoodGameOver += ShowEndScreen;
    }

    private void OnDisable()
    {
        PlayerController.BadGameOver -= ShowEndScreen;
        FallManager.GoodGameOver -= ShowEndScreen;
    }

    // ---------- Buttons ----------

    public void OnStartPressed()
    {
        startScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnRestartPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    // ---------- Screens ----------

    private void ShowEndScreen()
    {
        Time.timeScale = 0;
        endScreen.SetActive(true);

        UpdateDistanceText();
    }
// Distance wev'e passed
    private void UpdateDistanceText()
    {
        var fallManager = FallManager.Instance;
        if (fallManager == null) return;

        float remaining =
            FallManager.START_FALL_HEIGHT - GetPrivateFallDistance(fallManager);

        distanceText.text = $"Distance: {(int)remaining} m";
    }

    private float GetPrivateFallDistance(FallManager manager)
    {
        var field = typeof(FallManager)
            .GetField("fallDistance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        return field != null ? (float)field.GetValue(manager) : 0f;
    }
    // ---------- Pause ----------
    public void OnPausePressed()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void OnResumePressed()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }
}


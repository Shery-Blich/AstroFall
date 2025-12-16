using UnityEngine;
using TMPro;

public class GameEndUIController : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;

    [Header("Optional text fields in screens")]
    [SerializeField] TextMeshProUGUI winDetailsText;
    [SerializeField] TextMeshProUGUI loseDetailsText;

    private void Awake()
    {
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
    }

    private void OnEnable()
    {
        FallManager.GoodGameOver += ShowWin;
        PlayerController.BadGameOver += ShowLose;
    }

    private void OnDisable()
    {
        FallManager.GoodGameOver -= ShowWin;
        PlayerController.BadGameOver -= ShowLose;
    }

    private void ShowWin()
    {
        Time.timeScale = 0;
        loseScreen.SetActive(false);
        winScreen.SetActive(true);

        if (winDetailsText != null && FallManager.Instance != null && FallManager.Instance.distanceToFallText != null)
            winDetailsText.text = FallManager.Instance.distanceToFallText.text;
    }

    private void ShowLose()
    {
        Time.timeScale = 0;
        winScreen.SetActive(false);
        loseScreen.SetActive(true);

        if (loseDetailsText != null && FallManager.Instance != null && FallManager.Instance.distanceToFallText != null)
            loseDetailsText.text = FallManager.Instance.distanceToFallText.text;
    }
}

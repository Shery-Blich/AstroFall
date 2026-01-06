using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    public TextMeshProUGUI ScoreText;

    private bool isPotrait = false;


    void OnEnable()
    {
        FallManager.GoodGameOver += TurnOffText;
        PlayerController.BadGameOver += TurnOffText;
        ScreenController.ScreenOrientationUpdate += OnOrientationChange;
    }

    private void OnDisable()
    {
        FallManager.GoodGameOver -= TurnOffText;
        PlayerController.BadGameOver -= TurnOffText;
        ScreenController.ScreenOrientationUpdate -= OnOrientationChange;
    }

    void Update()
    {
        ScoreText.text = $"Distance To Earth:{(isPotrait ? "\n" : " ")}{FallManager.Instance.GetDistanceToEarth()}m\nCollected {MemoriesManager.Instance.CollectedMemories} Memories";
    }


    private void TurnOffText() => ScoreText.gameObject.SetActive(false);

    private void OnOrientationChange() => isPotrait = ScreenController.Instance.IsPortrait();

}

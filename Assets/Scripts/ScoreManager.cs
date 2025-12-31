using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    public TextMeshProUGUI ScoreText;


    void OnEnable()
    {
        FallManager.GoodGameOver += TurnOffText;
        PlayerController.BadGameOver += TurnOffText;
    }

    private void OnDisable()
    {
        FallManager.GoodGameOver -= TurnOffText;
        PlayerController.BadGameOver -= TurnOffText;
    }

    void Update()
    {
        ScoreText.text = $"Distance To Earth:\n{FallManager.Instance.GetDistanceToEarth()} m\nCollected {MemoriesManager.Instance.CollectedMemories} Memories";
    }

    private void TurnOffText() => ScoreText.gameObject.SetActive(false);
    
}

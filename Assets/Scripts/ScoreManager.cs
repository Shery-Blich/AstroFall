using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    public TextMeshProUGUI ScoreText;

    void Update()
    {
        ScoreText.text = $"Distance To Earth:\n{FallManager.Instance.GetDistanceToEarth()} m\nMemories X {MemoriesManager.Instance.CollectedMemories}";
    }
}

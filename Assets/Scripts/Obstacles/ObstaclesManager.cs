using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    [SerializeField]
    Asteroid[] asteroids;
    
    public int ObstacleCountInPortrait = 4;

    public static ObstaclesManager Instance{ get; private set; }

    private void Awake()
    {
        // Singleton pattern to ensure only one instance in all scenes
        if (Instance == null)
        {
            Instance = this;
            // Persist the same instance across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance exists, it means the new one is a duplicate, so we destroy it
            Destroy(gameObject);
        }
    }

    public void SetObstaclesToOrientation(bool isPortrait)
    {
        for (int i = 0; i < asteroids.Length; i++)
        {
            asteroids[i].gameObject.SetActive(
                !isPortrait || i < ObstacleCountInPortrait
            );
        }
    }
}

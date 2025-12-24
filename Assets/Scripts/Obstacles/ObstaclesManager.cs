using UnityEngine;
using System;

public class ObstaclesManager : MonoBehaviour
{
    [SerializeField]
    public ObstacleController[] obstacleControllers;

    public ObstacleTypeEnum CurrentObstacleStage = ObstacleTypeEnum.Asteroid;
    
    public int ObstacleCountInPortrait = 4;

    public static ObstaclesManager Instance{ get; private set; }

    private void OnEnable()
    {
        FallManager.OnStageChanged += UpdateObstacleTypes;
    }

    private void OnDisable()
    {
        FallManager.OnStageChanged -= UpdateObstacleTypes;
    }

    private void Awake()
    {
        if (obstacleControllers == null || obstacleControllers.Length == 0)
        {
            throw new Exception("ObstaclesManager requires at least one ObstacleController assigned in the inspector.");
        }

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
        for (int i = 0; i < obstacleControllers.Length; i++)
        {
            obstacleControllers[i].gameObject.SetActive(
                !isPortrait || i < ObstacleCountInPortrait
            );
        }
    }

    public void UpdateObstacleTypes(ObstacleTypeEnum gameStage)
    {
        this.CurrentObstacleStage = gameStage;
    }
}

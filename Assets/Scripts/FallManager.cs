using System;
using TMPro;
using UnityEngine;

public class FallManager : MonoBehaviour
{

    public static FallManager Instance { get; private set; }
    public static event Action GoodGameOver;
    public bool isGameOver { get; private set; } = false;

    [SerializeField]
    public float GlobalSpeed;

    //TODO: Make it simply a few constants instead of calculating power
    [SerializeField]
    public int GLOBAL_SPEED_UPDATE_INTERVAL_POW = 1;

    [SerializeField]
    public const float GLOBAL_SPEED_INTERVAL_UPDATE_BASE = 4.8f;

    [SerializeField]
    public const float GLOBAL_SPEED_ACCELERATION = 0.5f;

    [SerializeField]
    public int UpdateSpeedIntervalForTextInMiliSeconds = 1;

    [SerializeField]
    public float StartFallSpeedForText = 1.0f;

    [SerializeField]
    public float FallAccelerationForText = 0.1f;

    [SerializeField]
    public float MaxSpeedForTextChange = 2f;

    [SerializeField]
    public TextMeshProUGUI distanceToFallText;

    [SerializeField]
    public float Asteroid_Stage_Length= 4000.0f;

    [SerializeField]
    public float Trash_Stage_Length = 3500.0f;

    public float START_FALL_HEIGHT = 10160.0f;
    private float timeSinceLastSpeedUpdate = 0.0f;
    private float timeSinceGlobalLastSpeedUpdate = 0.0f;
    private float currentFallSpeed;
    private float fallDistance;

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

    void Start()
    {
        PlayerController.BadGameOver += OnBadGameOver;
        currentFallSpeed = StartFallSpeedForText;
        fallDistance = 0;
        GoodGameOver += OnGoodGameOver;
    }

    private void OnDestroy()
    {
        PlayerController.BadGameOver -= OnBadGameOver;
        GoodGameOver -= OnGoodGameOver;
    }

    void FixedUpdate()
    {
        timeSinceLastSpeedUpdate += Time.fixedDeltaTime;
        timeSinceGlobalLastSpeedUpdate += Time.fixedDeltaTime;
        if (!isGameOver)
        {
            if (timeSinceLastSpeedUpdate >= UpdateSpeedIntervalForTextInMiliSeconds)
            {
                UpdateSpeed();
                timeSinceLastSpeedUpdate = 0.0f;
            }


            UpdateGlobalSpeedIfNeeded();
            UpdateDistance();

            if (ObstaclesManager.Instance.CurrentObstacleStage != CalcObstacleType())
            {
                UpdateObstacleTypeIfNeeded();
            }
        }
    }

    // Change obstacle types based on how much has the player fallen
    // Asteroids -> Trash -> Planes, each stage has a fixed length,
    // for the switch we check the fall distance against the cumulative lengths of each stage
    private ObstacleType CalcObstacleType()
    {
        if (fallDistance <= Asteroid_Stage_Length)
        {
            return ObstacleType.Asteroid;
        }

        if (fallDistance <= Asteroid_Stage_Length + Trash_Stage_Length)
        {
            return ObstacleType.Trash;
        }

        return ObstacleType.Plane;
    }

    private void UpdateObstacleTypeIfNeeded()
    {
        print($"Obstacle type changed to in fall manager: {CalcObstacleType()}");
        ObstaclesManager.Instance.UpdateObstacleTypes(CalcObstacleType());
    }

    private void UpdateGlobalSpeedIfNeeded()
    {
        if (Mathf.Pow(GLOBAL_SPEED_INTERVAL_UPDATE_BASE, GLOBAL_SPEED_UPDATE_INTERVAL_POW) <= timeSinceGlobalLastSpeedUpdate)
        {
            timeSinceGlobalLastSpeedUpdate = 0;
            GLOBAL_SPEED_UPDATE_INTERVAL_POW++;
            GlobalSpeed += GLOBAL_SPEED_ACCELERATION;
            print($"Global speed increased to: {GlobalSpeed}");
        }
    }

    private void UpdateSpeed()
    {
        if (currentFallSpeed < MaxSpeedForTextChange)
        {
            currentFallSpeed += FallAccelerationForText;
        }
        else
        {
            currentFallSpeed = MaxSpeedForTextChange;
        }
    }

    private void UpdateDistance()
    {
        fallDistance += currentFallSpeed;
        distanceToFallText.text = $"Distance To Earth:\n{(int)(START_FALL_HEIGHT - fallDistance)} m\n{CalcObstacleType()} Stage";
        
        if(fallDistance >= START_FALL_HEIGHT)
        {
           GoodGameOver?.Invoke();
        }
    }

    private void OnGoodGameOver()
    {
        currentFallSpeed = 0;
        isGameOver = true;
        distanceToFallText.text = "You have landed!";
    }

    private void OnBadGameOver()
    {
        currentFallSpeed = 0;
        isGameOver = true;
        distanceToFallText.text = "Failed to reach earth!";
    }
}

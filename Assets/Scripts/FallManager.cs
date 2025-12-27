using System;
using TMPro;
using UnityEngine;

public class FallManager : MonoBehaviour
{

    public static FallManager Instance { get; private set; }
    public static event Action GoodGameOver;
    public static event Action<ObstacleTypeEnum> OnStageChanged;
    public bool isGameOver { get; private set; } = false;


    // TODO: Use ScriptableObjects instead of entire params to represent each stage
    [SerializeField]
    public float GlobalSpeed;

    [SerializeField]
    public int GlobalSpeedUpdatePow = 1;

    [SerializeField]
    public const float GLOBAL_SPEED_INTERVAL_UPDATE_BASE = 4.8f;

    [SerializeField]
    public const float START_FALL_HEIGHT = 10160.0f;

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


    public float FallDistance { get; private set; }
    private float timeSinceLastSpeedUpdate = 0.0f;
    private float timeSinceGlobalLastSpeedUpdate = 0.0f;
    private float currentFallSpeed;


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
        currentFallSpeed = StartFallSpeedForText;
        FallDistance = 0;
    }

    private void OnEnable()
    {
        PlayerController.BadGameOver += OnBadGameOver;
        GoodGameOver += OnGoodGameOver;
    }

    private void OnDisable()
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
            UpdateObstacleTypeIfNeeded();
        }
    }

    // Change obstacle types based on how much has the player fallen
    // Asteroids -> Trash -> Planes, each stage has a fixed length,
    // for the switch we check the fall distance against the cumulative lengths of each stage
    private ObstacleTypeEnum CalcObstacleType()
    {
        if (FallDistance <= Asteroid_Stage_Length)
        {
            return ObstacleTypeEnum.Asteroid;
        }

        if (FallDistance <= Asteroid_Stage_Length + Trash_Stage_Length)
        {
            return ObstacleTypeEnum.Trash;
        }

        return ObstacleTypeEnum.Plane;
    }

    private void UpdateObstacleTypeIfNeeded()
    {
        var currType = CalcObstacleType();
        if (ObstaclesManager.Instance.CurrentObstacleStage != currType)
        {
            print($"Obstacle type changed to in fall manager: {currType}");
            OnStageChanged?.Invoke(currType);
        }
    }

    private void UpdateGlobalSpeedIfNeeded()
    {
        if (Mathf.Pow(GLOBAL_SPEED_INTERVAL_UPDATE_BASE, GlobalSpeedUpdatePow) <= timeSinceGlobalLastSpeedUpdate)
        {
            timeSinceGlobalLastSpeedUpdate = 0;
            GlobalSpeedUpdatePow++;
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
        FallDistance += currentFallSpeed;
        distanceToFallText.text = $"Distance To Earth:\n{(int)(START_FALL_HEIGHT - FallDistance)} m\n{CalcObstacleType()} Stage";
        
        if(FallDistance >= START_FALL_HEIGHT)
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
    public void ResetRun()
    {
        isGameOver = false;
        currentFallSpeed = StartFallSpeedForText;
        FallDistance = 0;
        GlobalSpeedUpdatePow = 1;
        timeSinceLastSpeedUpdate = 0f;
        timeSinceGlobalLastSpeedUpdate = 0f;

        if (distanceToFallText != null)
            distanceToFallText.text =
                $"Distance To Earth:\n{(int)(START_FALL_HEIGHT - FallDistance)} m";
    }
}

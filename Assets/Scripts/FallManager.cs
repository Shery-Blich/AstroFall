using System;
using TMPro;
using UnityEngine;

public class FallManager : MonoBehaviour
{

    public static FallManager Instance { get; private set; }
    public static event Action GoodGameOver;
    public bool isGameOver = false;

    public float GlobalSpeed{ get; private set; }
    private int globalUpdatePow = 1;
    private const float globalUpdateBase = 4.8f;
    private const float GLOBAL_SPEED_ACCELERATION = 0.5f;


    private float currentFallSpeed;
    public float fallDistance;
    private const float START_FALL_SPEED = 1.0f;
    private const float FALL_ACCELERATION = 0.1f;
    public const float START_FALL_HEIGHT = 10160.0f;
    private float fallDistance;
    private const float START_FALL_SPEED = 1.0f;
    private const float FALL_ACCELERATION = 0.1f;
    private const float START_FALL_HEIGHT = 10160.0f;
    private const float MAX_SPEED = 2.0f;

    private int updateSpeedInterval = 1;
    private float timeSinceLastSpeedUpdate = 0.0f;
    private float timeSinceGlobalLastSpeedUpdate = 0.0f;


    [SerializeField]
    public TextMeshProUGUI distanceToFallText;

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
        currentFallSpeed = START_FALL_SPEED;
        fallDistance = 0;
        GlobalSpeed = UnityEngine.Random.Range(0.5f, 1f);
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
            if (timeSinceLastSpeedUpdate >= updateSpeedInterval)
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
    // Asteroids until 4000m, Trash until 7500m, Planes after that
    private ObstacleType CalcObstacleType()
    {
        return fallDistance switch
        {
            <= 4000 => ObstacleType.Asteroid,
            <= 7500 => ObstacleType.Trash,
            _ => ObstacleType.Plane
        };
    }

    private void UpdateObstacleTypeIfNeeded()
    {
        print($"Obstacle type changed to in fall manager: {CalcObstacleType()}");
        ObstaclesManager.Instance.UpdateObstacleTypes(CalcObstacleType());
    }

    private void UpdateGlobalSpeedIfNeeded()
    {
        if (Mathf.Pow(globalUpdateBase, globalUpdatePow) <= timeSinceGlobalLastSpeedUpdate)
        {
            timeSinceGlobalLastSpeedUpdate = 0;
            globalUpdatePow++;
            GlobalSpeed += GLOBAL_SPEED_ACCELERATION;
            print($"Global speed increased to: {GlobalSpeed}");
        }
    }

    private void UpdateSpeed()
    {
        if (currentFallSpeed < MAX_SPEED)
        {
            currentFallSpeed += FALL_ACCELERATION;
        }
        else
        {
            currentFallSpeed = MAX_SPEED;
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

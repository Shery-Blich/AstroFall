using TMPro;
using UnityEngine;

public class FallManager : MonoBehaviour
{

    public static FallManager Instance { get; private set; }
    public float currentFallSpeed { get; private set; }

    public const float START_FALL_SPEED = 1.0f;
    public const float FALL_ACCELERATION = 0.1f;
    public const float START_FALL_HEIGHT = 10160.0f;
    public const float MAX_SPEED = 2.0f;
    private float fallDistance;
    private bool isGameOver = false;

    private int updateSpeedInterval = 1;
    private float timeSinceLastSpeedUpdate = 0.0f;

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
        PlayerController.GameOver += OnBadGameOver;
        currentFallSpeed = START_FALL_SPEED;
        fallDistance = 0;
    }

    void FixedUpdate()
    {
        timeSinceLastSpeedUpdate += Time.fixedDeltaTime;
        if (!isGameOver)
        {
            if (timeSinceLastSpeedUpdate >= updateSpeedInterval)
            {
                UpdateSpeed();
                timeSinceLastSpeedUpdate = 0.0f;
            }

            UpdateDistance();
        }
    }
    private void OnDestroy()
    {
        PlayerController.GameOver -= OnBadGameOver;
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
        distanceToFallText.text = $"Distance To Earth:\n{(int)(START_FALL_HEIGHT - fallDistance)} m";
        
        if(fallDistance >= START_FALL_HEIGHT)
        {
            OnGoodGameOver();
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
        distanceToFallText.enabled = false;
    }
}

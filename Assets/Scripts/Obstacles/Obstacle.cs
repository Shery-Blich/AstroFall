using UnityEngine;

public class Obstacle : MonoBehaviour, IResettable
{
    [SerializeField]
    protected Rigidbody2D rb;

    [SerializeField]
    public float MaxSizeProtrait = 3.0f;

    [SerializeField]
    public float MinSizeProtrait = 1.0f;

    [SerializeField]
    public float HoriziontalSizeScaleToPortrait = 0.6f;

    protected float minForce;
    protected float Size { get; set; }

    protected float spawnYLocation = 0.35f;

    // TODO: Use Enable & Disable instead of start and destory | multiple classes -> look for them
    /// For resetting - To have a reference to its starting position and rotation

    private Vector3 startPosition;
    private Quaternion startRotation;


    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        StartObstacle();
        PlayerController.BadGameOver += OnGameOver;
        FallManager.GoodGameOver += OnGameOver;
    }

    private void OnDestroy()
    {
        PlayerController.BadGameOver -= OnGameOver;
        FallManager.GoodGameOver -= OnGameOver;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PathPaver")
            {
            var viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            var moveAdjustment = Vector3.zero;
            moveAdjustment.y += viewportPosition.y + 2f;

            transform.position = Camera.main.ViewportToWorldPoint(viewportPosition + moveAdjustment);
        }
    }

    protected virtual float SetSize()
    {
        Size = Random.Range(MinSizeProtrait, MaxSizeProtrait);

        // We Zoom in with the camera to fit the art to the app
        // to bypass it we need to fit the size of obstcales to the screen
        if (Screen.orientation == ScreenOrientation.LandscapeRight || Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            Size *= HoriziontalSizeScaleToPortrait;
        }

        transform.localScale = 0.3f * Size * Vector3.one;

        return Size;
    }

    protected virtual void AddForce(float minForce, float maxForce)
    {
        rb.linearVelocity = Vector2.zero;
        var spawnSpeed = Random.Range(minForce, maxForce);
        rb.AddForce(Vector2.up * spawnSpeed, ForceMode2D.Impulse);
    }

    //TODO: Use variables to better explain what's going on here 
    protected virtual (Vector3 viewportPosition, Vector3 moveAdjustment) GetNewObstaclePos()
    {
        var viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        var moveAdjustment = Vector3.zero;

        // Randomize horizontal movement to make it less predictable.
        // Convert back into world coordinates before assigning.
        if (viewportPosition.x < -0.1 || viewportPosition.x > 1.1)
        {
            // Find something to replace the magic of 0.35
            moveAdjustment.y -= viewportPosition.y + spawnYLocation;
            moveAdjustment.x = Random.Range(-0.7f, 0.7f);
            AddForce(minForce + FallManager.Instance.GlobalSpeed, Size + FallManager.Instance.GlobalSpeed);
        }
        else if (viewportPosition.y > 1.1f)
        {
            moveAdjustment.y -= viewportPosition.y + spawnYLocation;
            moveAdjustment.x += Random.Range(-0.2f, 0.2f);
            AddForce(minForce + FallManager.Instance.GlobalSpeed, Size + FallManager.Instance.GlobalSpeed);
        }

        return (viewportPosition, moveAdjustment);
    }

    protected virtual void UpdateObstcale()
    {
        (var viewportPosition, var moveAdjustment) = GetNewObstaclePos();

        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition + moveAdjustment);
    }

    protected virtual void StartObstacle()
    {
        SetSize();
        minForce = 1.0f / Size;
        AddForce(minForce, Size);
    }


    // Obstacles movement is physics based, so we use FixedUpdate
    protected void Update()
    {
        UpdateObstcale();
    }

    private void OnGameOver()
    {
        this.gameObject.SetActive(false);
    }

    // / IResettable implementation

    public void ResetState()
    {
        ResetManager.Instance.Register(this);
        gameObject.SetActive(true);

        transform.position = startPosition;
        transform.rotation = startRotation;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        StartObstacle();
    }

}

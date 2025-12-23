using UnityEngine;

public class Obstacle : MonoBehaviour, IResettable
{
    [SerializeField]
    protected Rigidbody2D rb;

    protected float minForce;
    protected float Size { get; set; }

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

    protected virtual float SetSize()
    {
        Size = Random.Range(1.0f, 2.0f);
        transform.localScale = 0.3f * Size * Vector3.one;

        return Size;
    }

    protected virtual void AddForce(float minForce, float maxForce)
    {
        rb.linearVelocityY = 0;
        rb.linearVelocityX = 0;

        var direction = new Vector2(0, Random.value).normalized;
        float spwanSpeed = Random.Range(minForce, maxForce);

        rb.AddForce(direction * spwanSpeed, ForceMode2D.Impulse);
    }

    protected virtual (Vector3 viewportPosition, Vector3 moveAdjustment) GetNewObstaclePos()
    {
        var viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        var moveAdjustment = Vector3.zero;

        // Randomize horizontal movement to make it less predictable.
        // Convert back into world coordinates before assigning.
        if (viewportPosition.x < -0.1f || viewportPosition.x > 1.1f)
        {
            moveAdjustment.y = -viewportPosition.y;
            moveAdjustment.x = Random.Range(-0.7f, 0.7f);
            AddForce(minForce + FallManager.Instance.GlobalSpeed, Size + FallManager.Instance.GlobalSpeed);
        }
        else if (viewportPosition.y > 1.1f)
        {
            moveAdjustment.y -= 1.1f;
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

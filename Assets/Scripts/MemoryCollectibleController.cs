using UnityEngine;

public class MemoryCollectibleController : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody2D rb;

    [SerializeField]
    public SpriteRenderer sprite;

    [SerializeField]
    public Collider2D memeoryCollider;

    [SerializeField]
    public float HoriziontalSizeScaleToPortrait = 0.6f;

    [SerializeField]
    public float minInitalSpeed = 1.0f;

    [SerializeField]
    public float maxInitalSpeed = 2.0f;

    [SerializeField]
    public Transform pathPaverPos;

    [SerializeField]
    public float MinSpawnDelay = 0f;

    [SerializeField]
    public float MaxSpawnDelay = 2f;

    private float currSpawnDelay = 0.0f;
    private float currSpawnDelayTimeLapse = 0.0f;


    void Start()
    {
        StartObstacle();
    }

    private void OnEnable()
    {
        PlayerController.BadGameOver += DeactivateObject;
        FallManager.GoodGameOver += DeactivateObject;
    }

    private void OnDisable()
    {
        PlayerController.BadGameOver -= DeactivateObject;
        FallManager.GoodGameOver -= DeactivateObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            this.CollectedByPlayer();
        }
    }

    protected void Update()
    {
        SetNewPosIfNeeded();
    }
    protected void SetNewPosIfNeeded()
    {
        var viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPosition.y > 1.0f)
        {
            if (currSpawnDelayTimeLapse < currSpawnDelay)
            {
                currSpawnDelayTimeLapse += Time.deltaTime;
                return;
            }

            RespawnCoin();
        }
    }

    protected void StartObstacle()
    {
        AddForce(FallManager.Instance.GlobalSpeed + minInitalSpeed, FallManager.Instance.GlobalSpeed + maxInitalSpeed);
        transform.position = pathPaverPos.position;
        currSpawnDelay = Random.Range(MinSpawnDelay, MaxSpawnDelay);
    }

    protected void AddForce(float minForce, float maxForce)
    {
        rb.linearVelocity = Vector2.zero;
        var spawnSpeed = Random.Range(minForce, maxForce);
        rb.AddForce(Vector2.up * spawnSpeed, ForceMode2D.Impulse);
    }

    public void RespawnCoin()
    {
        this.memeoryCollider.enabled = true;
        this.sprite.enabled = true;
        currSpawnDelayTimeLapse = 0.0f;
        StartObstacle();
    }

    private void CollectedByPlayer()
    {
        this.memeoryCollider.enabled = false;
        this.sprite.enabled = false;
    }

    private void DeactivateObject()
    {
        this.gameObject.SetActive(false);
    }

}

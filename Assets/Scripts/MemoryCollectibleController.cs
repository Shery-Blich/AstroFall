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
    public float initalSpeed = 1.0f;

    [SerializeField]
    public Transform pathPaverPos;


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
            this.DeactivateObject();
        }
    }

    protected void StartObstacle()
    {
        AddForce();
        transform.position = pathPaverPos.position;
    }

    protected void AddForce()
    {
        var currSpeed = FallManager.Instance.GlobalSpeed + initalSpeed;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * currSpeed, ForceMode2D.Impulse);
    }

    public void RespawnCoin()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
        
        this.memeoryCollider.enabled = true;
        this.sprite.enabled = true;

        StartObstacle();
    }

    private void CollectedByPlayer()
    {
        // Add particles here
        SoundManager.Instance.PlaySFX(SFXTypeEnum.CollectiblePicked);
        MemoriesManager.Instance.CollectedMemories++;
        this.DeactivateObject();
    }

    public void DeactivateObject()
    {
        MemoriesManager.Instance.InactiveMemories.Enqueue(this);
        this.gameObject.SetActive(false);
    }
}

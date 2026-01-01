using UnityEngine;

public class MemoryCollectibleController : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody2D rb;

    [SerializeField]
    public Collider2D memeoryCollider;

    [SerializeField]
    public GameObject pickUpEffect;

    [SerializeField]
    public GameObject goUpEffect;

    [SerializeField]
    public GameObject sprite;

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
        var viewportPosition = ScreenController.Instance.MainCam.WorldToViewportPoint(transform.position);

        if (viewportPosition.y > 1.2f)
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

        this.sprite.SetActive(true);
        this.pickUpEffect.SetActive(false);
        this.goUpEffect.SetActive(true);
        this.memeoryCollider.enabled = true;

        StartObstacle();
    }

    private void CollectedByPlayer()
    {
        
        MemoriesManager.Instance.CollectedMemories++;
        this.pickUpEffect.SetActive(true);
        this.sprite.SetActive(false);
        this.goUpEffect.SetActive(false);
        SoundManager.Instance.PlaySFX(SFXTypeEnum.CollectiblePicked);
    }

    public void DeactivateObject()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }

        MemoriesManager.Instance.QueueMemoryForRespawn(this);
        this.gameObject.SetActive(false);
    }
}

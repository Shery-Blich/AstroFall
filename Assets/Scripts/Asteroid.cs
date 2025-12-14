using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    public float size;  
    public Rigidbody2D rb;

    public float minForce;

    void Start()
    {
        transform.localScale = 0.3f * size * Vector3.one;
        minForce = 1.0f / size;
        AddForce(minForce, size);
    }

    private void AddForce(float minForce, float maxForce)
    {
        var direction = new Vector2(0, Random.value).normalized;
        float spwanSpeed = Random.Range(minForce, maxForce);
        rb.AddForce(direction * spwanSpeed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        var viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        var moveAdjustment = Vector3.zero;
        if (viewportPosition.x < 0 || viewportPosition.x > 1)
        {
            moveAdjustment.y = -viewportPosition.y;
            moveAdjustment.x = Random.Range(-0.7f, 0.7f);
            AddForce(minForce, FallManager.Instance.currentFallSpeed);
        }
        else if (viewportPosition.y > 1)
        {
            moveAdjustment.y -= 1;
            moveAdjustment.x += Random.Range(-0.2f, 0.2f);
            AddForce(minForce, FallManager.Instance.currentFallSpeed);
        }

        // Randomize horizontal movement to make it less predictable.
        // Convert back into world coordinates before assigning.
        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition + moveAdjustment);
    }
}

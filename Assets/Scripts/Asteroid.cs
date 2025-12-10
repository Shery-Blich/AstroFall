using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float size;  
    public Rigidbody2D rb;

    void Start()
    {
        transform.localScale = 0.3f * size * Vector3.one;

        var direction = new Vector2(0, Random.value).normalized;
        float spwanSpeed = Random.Range(1f/size, size);
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
        }
        else if (viewportPosition.y > 1)
        {
            moveAdjustment.y -= 1;
            moveAdjustment.x += Random.Range(-0.2f, 0.2f);
        }

        // Randomize horizontal movement to make it less predictable.
        // Convert back into world coordinates before assigning.
        transform.position = Camera.main.ViewportToWorldPoint(viewportPosition + moveAdjustment);
    }
}

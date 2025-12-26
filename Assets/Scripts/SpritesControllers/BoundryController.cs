using UnityEngine;

public class BoundryController : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;

    void Update()
    {
        var spriteHalfWidth = spriteRenderer.bounds.extents.x;
        var spriteHalfHeight = spriteRenderer.bounds.extents.y;
        var screenBounds = ScreenController.Instance.ScreenBounds;

        // Clamp the position of the Sprite to stay within screen bounds
        // If the sprite is already within screen bounds, this does not affect it's position
        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, -screenBounds.x + spriteHalfWidth, screenBounds.x - spriteHalfWidth),
            Mathf.Clamp(transform.position.y, -screenBounds.y + spriteHalfHeight, screenBounds.y - spriteHalfHeight)
        );
    }
}

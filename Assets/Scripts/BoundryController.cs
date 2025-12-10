using System.Net.WebSockets;
using UnityEngine;

public class BoundryController : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;

    // Update is called once per frame
    void Update()
    {
        var spriteWidthRadius = spriteRenderer.bounds.extents.x;
        var spriteHeightRadius = spriteRenderer.bounds.extents.y;
        var screenBounds = RotationController.Instance.ScreenBounds;

        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, -screenBounds.x + spriteWidthRadius, screenBounds.x - spriteWidthRadius),
            Mathf.Clamp(transform.position.y, -screenBounds.y + spriteHeightRadius, screenBounds.y - spriteHeightRadius)
        );
    }
}

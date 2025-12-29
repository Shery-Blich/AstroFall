using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Vector2 direction = new Vector2(0, +1);
    private bool isScrolling = true;

    [Header("Scale By Orientation")]
    [SerializeField] private float portraitScale = 0.85f;
    [SerializeField] private float landscapeScale = 1f;

    [Header("Y Position By Orientation")]
    [SerializeField] private float portraitYOffset = 5f;
    [SerializeField] private float landscapeYOffset = 0f;

    private Vector3 startPosition;

    private void OnEnable()
    {
        PlayerController.BadGameOver += StopScroll;
        FallManager.GoodGameOver += StopScroll;
        ScreenController.ScreenOrientationUpdate += UpdateByOrientation;
    }

    private void OnDisable()
    {
        PlayerController.BadGameOver -= StopScroll;
        FallManager.GoodGameOver -= StopScroll;
        ScreenController.ScreenOrientationUpdate -= UpdateByOrientation;
    }

    private void Start()
    {
        startPosition = transform.position;

        UpdateByOrientation();
    }

    void Update()
    {
        if (!isScrolling) return;

        transform.position +=
            (Vector3)(direction.normalized * FallManager.Instance.GlobalSpeed * Time.deltaTime);
    }

    private void StopScroll()
    {
        isScrolling = false;
    }

    private void UpdateByOrientation()
    {
        bool isPortrait =
            Screen.orientation == ScreenOrientation.Portrait ||
            Screen.orientation == ScreenOrientation.PortraitUpsideDown;

        // Scale
        float targetScale = isPortrait ? portraitScale : landscapeScale;
        transform.localScale = Vector3.one * targetScale;

        // Position Y 
        float yOffset = isPortrait ? portraitYOffset : landscapeYOffset;
        transform.position = new Vector3(
            transform.position.x,
            startPosition.y + yOffset,
            transform.position.z
        );
    }
}

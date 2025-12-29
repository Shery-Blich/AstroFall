using Unity.VisualScripting;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Vector2 direction = new Vector2(0, +1);
    private bool isScrolling = true;

    [Header("Scale By Orientation")]
    [SerializeField] private float portraitScale = 0.85f;
    [SerializeField] private float landscapeScale = 1f;

    private void OnEnable()
    {
        PlayerController.BadGameOver += StopScroll;
        FallManager.GoodGameOver += StopScroll;
        ScreenController.ScreenOrientationUpdate += UpdateScaleByOrientation;
    }

    private void OnDisable()
    {
        PlayerController.BadGameOver -= StopScroll;
        FallManager.GoodGameOver -= StopScroll;
        ScreenController.ScreenOrientationUpdate -= UpdateScaleByOrientation;
    }

    private void Start()
    {
        UpdateScaleByOrientation(); // חשוב: גם בתחילת סצנה
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

    private void UpdateScaleByOrientation()
    {
        bool isPortrait =
            Screen.orientation == ScreenOrientation.Portrait ||
            Screen.orientation == ScreenOrientation.PortraitUpsideDown;

        transform.localScale = isPortrait
            ? Vector3.one * portraitScale
            : Vector3.one * landscapeScale;
    }
}

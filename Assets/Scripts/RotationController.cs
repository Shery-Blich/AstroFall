using Unity.VisualScripting;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    public static RotationController Instance { get; private set; }

    public Vector2 ScreenBounds { get; private set; }


    private void Awake()
    {
        // Singleton pattern to ensure only one instance in all scenes
        if (Instance == null)
        {
            Instance = this;

            // Persist the same instance across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance exists, it means the new one is a duplicate, so we destroy it
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetRotationLock(true);
        SetScreenBounds();
        SetObstaclesCount();
    }

    public void SetScreenBounds()
    {
        this.ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    public void SetObstaclesCount()
    {
        ScreenOrientation currentOrientation = Screen.orientation;
        ObstaclesManager.Instance.SetObstaclesToOrientation(isPortrait:
            currentOrientation == ScreenOrientation.Portrait || currentOrientation == ScreenOrientation.PortraitUpsideDown);
    }

    public void SetRotationLock(bool shouldLock)
    {
        if (shouldLock)
        {
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;

            // Lock to current orientation
            Screen.orientation = Screen.orientation;
        }
        else
        {
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;

            // Listen to rotation
            Screen.orientation = ScreenOrientation.AutoRotation;
        }

        print($"Rotation lock set to: {shouldLock}");
    }


    private void OnDestroy()
    {
        SetRotationLock(false);
    }
}
using System;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    [SerializeField]
    public Camera MainCam;

    [SerializeField]
    private float portraitCameraSize = 6;

    [SerializeField]
    private float landscapeCameraSize = 2.8f;
    public static ScreenController Instance { get; private set; }

    public Vector2 ScreenBounds { get; private set; }

    public static event Action ScreenOrientationUpdate;


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
        SetScreen();
    }

    public void SetScreen()
    {
        SetRotationLock(true);
        SetCamera();
        SetScreenBounds();
        SetObstaclesCount();
        ScreenOrientationUpdate?.Invoke();
    }

    public void SetCamera()
    {
        if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            MainCam.orthographicSize = portraitCameraSize;
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            MainCam.orthographicSize = landscapeCameraSize;
        }
    }

    public void SetScreenBounds()
    {
        this.ScreenBounds = MainCam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    public void SetObstaclesCount()
    {
        ScreenOrientation currentOrientation = Screen.orientation;
        var isPortrait = currentOrientation == ScreenOrientation.Portrait || currentOrientation == ScreenOrientation.PortraitUpsideDown;
        ObstaclesManager.Instance.SetObstaclesToOrientation(isPortrait);

        print($"Set obstacles for orientation: {currentOrientation}");
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
using System;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public Camera MainCam;

    [SerializeField]
    public float portraitCameraSize = 6;

    [SerializeField]
    public float landscapeCameraSize = 2.8f;

    public static ScreenController Instance { get; private set; }

    public Vector2 ScreenBounds { get; private set; }

    public static event Action ScreenOrientationUpdate;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SetScreen();
    }

    public void SetScreen()
    {
        SetRotationLock(true);
        SetScreenAutoSleep(false);
        SetCamera();
        SetScreenBounds();

        // TODO: Move set ObstaclesCount to listen to screen orientation change event
        SetObstaclesCount();
        ScreenOrientationUpdate?.Invoke();
    }

    public void SetCamera()
    {
        Camera cam = Camera.main;

        if (IsPortrait())
        {
            MainCam.orthographicSize = portraitCameraSize;

        }
        else
        {
            MainCam.orthographicSize = landscapeCameraSize;
        }
    }

    public bool IsPortrait()
    {
        ScreenOrientation currentOrientation = Screen.orientation;
        return currentOrientation == ScreenOrientation.Portrait || currentOrientation == ScreenOrientation.PortraitUpsideDown;
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

    // Because the game is tilt only, and doesnt require the user to touch the screen
    // We need to disable auto sleep to avoid the screen turning off during gameplay
    public void SetScreenAutoSleep(bool shouldAutoSleep)
    {
        if (shouldAutoSleep)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        print($"Screen auto sleep set to: {shouldAutoSleep}");
    }
    private void OnDestroy()
    {
        SetRotationLock(false);
        SetScreenAutoSleep(true);
    }
}
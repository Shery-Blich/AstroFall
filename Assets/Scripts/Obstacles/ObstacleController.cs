using System;
using System.Drawing;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField]
    Asteroid asteroidScript;

    [SerializeField]
    Plane planeScript;

    [SerializeField]
    Trash trashScript;

    // Single Source of Truth for all types
    private ObstacleTypeEnum currentObstacleType;
    
    private void Start()
    {
        UpdateActiveScripts(currentObstacleType);
    }

    private void Update()
    {
        UpdateObstecalType();
    }

    // TODO: Use events and screen controller to get bounds and use those for checking change instead of cacling world to viewport
    private void UpdateObstecalType()
    {
        var viewportPosition = ScreenController.Instance.MainCam.WorldToViewportPoint(transform.position);

        if (viewportPosition.y < -0.1f)
        {
            UpdateActiveScripts(ObstaclesManager.Instance.CurrentObstacleStage);
        }
    }

    private void UpdateActiveScripts(ObstacleTypeEnum obstacleType)
    {
        // Since Obstacles are updated only once they are outside the screen,
        // we check every time we leave if the current stage changed
        // if it didn't we skip the update
        // otherwise we update the type
        if (!IsUpdateNeeded(obstacleType))
        {
            return;
        }

        print($"Updating Obstacle {this.gameObject.name} Type to {ObstaclesManager.Instance.CurrentObstacleStage}");
        switch (obstacleType)
        {
            case ObstacleTypeEnum.Trash:
                SetScriptActivation(false, trash: true, false);
                break;

            case ObstacleTypeEnum.Plane:
                SetScriptActivation(false, false, plane: true);
                planeScript.FreezeRotation();
                break;

            // Default to Asteroid
            default:
                SetScriptActivation(astroid: true, false, false);
                break;
        }

        currentObstacleType = obstacleType;
    }

    private void SetScriptActivation(bool astroid, bool trash, bool plane)
    {
        print($"Setting Obstacle {this.gameObject.name} Scripts - Astroid: {astroid}, Trash: {trash}, Plane: {plane}");
        asteroidScript.enabled = astroid;
        trashScript.enabled = trash;
        planeScript.enabled = plane;
    }

    private bool IsUpdateNeeded(ObstacleTypeEnum obstacleType) => obstacleType != currentObstacleType;
}

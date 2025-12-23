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

    private ObstacleType currentObstacleType;
    
    private void Start()
    {
        ChangeActiveScripts(currentObstacleType);
    }

    private void Update()
    {
        UpdateObstecalType();
    }

    private void UpdateObstecalType()
    {
        var viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPosition.y < -0.1f)
        {
            ChangeActiveScripts(ObstaclesManager.Instance.CurrentObstacleStage);
        }
    }

    private void ChangeActiveScripts(ObstacleType obstacleType)
    {
        //TODO: Change Use events
        // No change needed 
        if (obstacleType == currentObstacleType)
        {
            return;
        }

        print($"Updating Obstacle {this.gameObject.name} Type to {ObstaclesManager.Instance.CurrentObstacleStage}");
        switch (obstacleType)
        {
            case ObstacleType.Trash:
                SetScriptActivation(false, trash: true, false);
                break;

            case ObstacleType.Plane:
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
}

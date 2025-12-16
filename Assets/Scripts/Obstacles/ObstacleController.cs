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

    public ObstacleType GameStageType { get; set; }

    private ObstacleType currentObstacleType;

    private void Start()
    {
        currentObstacleType = ObstacleType.Asteroid;
        GameStageType = ObstacleType.Asteroid;
        ChangeActiveScripts(currentObstacleType);
    }

    private void Update()
    {
        UpdateObstecalType();
    }

    private void UpdateObstecalType()
    {
        var viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        if ((viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y > 1))
        {
            ChangeActiveScripts(GameStageType);
        }
    }

    private void ChangeActiveScripts(ObstacleType obstacleType)
    {
        // No change needed
        if (obstacleType == currentObstacleType)
        {
            return;
        }

        switch (obstacleType)
        {
            case ObstacleType.Trash:
                SetScriptActivation(false, trash: true, false);
                break;

            case ObstacleType.Plane:
                SetScriptActivation(false, false, plane: true);
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

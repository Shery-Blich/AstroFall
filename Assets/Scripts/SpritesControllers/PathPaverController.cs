using Unity.VisualScripting;
using UnityEngine;

public class PathPaverController : MonoBehaviour
{
    [SerializeField]
    public PolygonCollider2D PlayerCollider;

    [SerializeField]
    public BoxCollider2D PathPaverCollider;

    [SerializeField]
    public float MaxSpeedLimit = 4.0f;

    [SerializeField]
    public float MinSpeedLimit = 2.0f;

    [SerializeField]
    public float CurrentSpeed = 2.0f;

    [SerializeField]
    public int ChangeSpeedIntervalSeconds = 4;

    private Vector2 TargetPositionLeftPoint;
    private Vector2 TargetPositionRightPoint;
    private Vector2 CurrentTarget;

    private float timeSinceLastSpeedUpdate = 0.0f;

    public void OnEnable()
    {
        ScreenController.ScreenOrientationUpdate += SetPathPaverTargets;
        FallManager.OnStageChanged += DisableOnStage;
    }

    public void OnDisable()
    {
        ScreenController.ScreenOrientationUpdate -= SetPathPaverTargets;
        FallManager.OnStageChanged -= DisableOnStage;
    }

    public void Update()
    {
        timeSinceLastSpeedUpdate += Time.deltaTime; 
        if (timeSinceLastSpeedUpdate >= ChangeSpeedIntervalSeconds)
        {
            CurrentSpeed = Random.Range(MinSpeedLimit, MaxSpeedLimit);
            timeSinceLastSpeedUpdate = 0.0f;
        }
    }

    //TODO: Cleanup(break into functions), comment for entire class
    public void FixedUpdate()
    {
        Vector2 currPos = this.transform.position;


        if (currPos == TargetPositionLeftPoint)
        {
            CurrentTarget = TargetPositionRightPoint;
        }
        else if (currPos == TargetPositionRightPoint) 
        {
            CurrentTarget = TargetPositionLeftPoint;
        }

        transform.position = Vector2.MoveTowards(transform.position,
            CurrentTarget, MaxSpeedLimit * Time.deltaTime);
    }

    // We can't do it on start since we dependent on screen controller's start
    // And we need to change the targets orientation changes
    // So we use an event for it
    public void SetPathPaverTargets()
    {
        var screenBounds = ScreenController.Instance.ScreenBounds;
        // Find a better number than this magic number
        var yPos = -screenBounds.y - 1f;
        var xPoS = screenBounds.x - PathPaverCollider.size.x / 2;

        var initalTargetRandom = Random.Range(0, 1);
        TargetPositionLeftPoint = new Vector2(-xPoS, yPos);
        TargetPositionRightPoint = new Vector2(xPoS, yPos);
        this.transform.position = new Vector3 (this.transform.position.x, yPos);

        if (initalTargetRandom <= 0.5)
        {
            CurrentTarget = TargetPositionLeftPoint;
        }
        else
        {
            CurrentTarget = TargetPositionRightPoint;
        }   
    }

    public void DisableOnStage(ObstacleTypeEnum obstacleType)
    {
        if (obstacleType == ObstacleTypeEnum.Plane)
        {
            this.gameObject.SetActive(false);
        }
    }
}

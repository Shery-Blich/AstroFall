using Unity.VisualScripting;
using UnityEngine;

public class PathPaverController : MonoBehaviour
{
    [SerializeField]
    public PolygonCollider2D PlayerCollider;

    [SerializeField]
    public BoxCollider2D PathPaverCollider;

    [SerializeField]
    public float MovementSpeed = 5.0f;

    private Vector2 TargetPositionLeftPoint;
    private Vector2 TargetPositionRightPoint;
    private Vector2 CurrentTarget;

    public void OnEnable()
    {
        ScreenController.ScreenOrientationUpdate += SetPathPaverTargets;
    }

    public void OnDisable()
    {
        ScreenController.ScreenOrientationUpdate -= SetPathPaverTargets;
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

        print($"Moving to x:{CurrentTarget.x}, y: {CurrentTarget.y}");
        transform.position = Vector2.MoveTowards(transform.position,
            CurrentTarget, MovementSpeed * Time.deltaTime);

        print($"Current Screen bounds: x:{ScreenController.Instance.ScreenBounds}");
    }

    // We can't do it on start since we dependent on screen controller's start
    // And we need to change the targets orientation changes
    // So we use an event for it
    public void SetPathPaverTargets()
    {
        var screenBounds = ScreenController.Instance.ScreenBounds;
        var yPos = -screenBounds.y ;
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
}

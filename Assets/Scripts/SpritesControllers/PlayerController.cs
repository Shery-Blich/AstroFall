using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Collider2D playerColiider;

    [SerializeField]
    private float godModeDuration;

    private bool isInGodMode;
    private enum MovementDirection
    {
        Idle,
        Left,
        Right
    }

    private MovementDirection playerMovementDir;
    private float tiltDuration = 0.0f;
    private float moveSpeed = 7.0f;


    public static event Action BadGameOver;
    public const float MAX_MOVEMENT_SPEED = 15.0f;
    private const float MIN_TITLT_FOR_MOVEMENT = 0.05f;
    private const float MIN_TILT_FOR_ROTATION = 0.1f;
    private const int MAX_WARNING_COUNT = 3;
    private int warningCount = 0;

    /// For resetting - To have a reference to its starting position and rotation

    Vector3 startPos;
    Quaternion startRot;

    private void Awake()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    private void Start()
    {
        this.gameObject.SetActive(true);
        FallManager.GoodGameOver += HandelGoodGameOver;

        // We need to enable the accelerometer device explicitly for the APK to work on mobile
        if (Accelerometer.current != null)
        {
            InputSystem.EnableDevice(Accelerometer.current);
            print("Accelerometer enabled for PlayerController.");
        }
    }

    private void OnDestroy()
    {
        BadGameOver -= HandelBadGameOver;
        FallManager.GoodGameOver -= HandelGoodGameOver;
    }

    // Update is called once per frame, to make tilting smooth we use Update to make sure tilting happenes at relevant frame(no delay or jitter)
    void Update()
    {
        // We give priority to keyboard movement if any key is pressed since it's for testing purposes
        var testMoveDir = HandleKeyBoardMovment();
        TitltPlayer(testMoveDir ?? this.playerMovementDir);
    }

    // Fixed Update is called at every physics step, and because we are dependent on mobile accelerometer
    // We need to use FixedUpdate for smooth movement
    private void FixedUpdate()
    {
        HandleMobileMovement();
    }

    private void HandleMobileMovement()
    {
        try
        {
            var accelerationXVal = Accelerometer.current.acceleration.ReadValue().x;

            MoveToNewPos(accelerationXVal);
            SetTiltDirection(accelerationXVal);
        } catch (Exception e) {
            if (warningCount >= MAX_WARNING_COUNT)
            {
                return;
            }



            print($"No Accelerometer Instance found, skipping mobile movement handling. Are you running in Editor?" +
                $"\nError:");
            print(e);
            warningCount++;
        }
    }

    private void MoveToNewPos(float accelerationXVal)
    {
        if (!IsMobileTitltEnoughForMovement(accelerationXVal))
        {
            return;
        }

        float currentSpeed = Mathf.Lerp(moveSpeed, MAX_MOVEMENT_SPEED, tiltDuration / 2.0f);
        var dx = accelerationXVal * currentSpeed * Time.fixedDeltaTime;

        Vector2 position = transform.position;
        position.x += dx * 0.8f;
        transform.position = position;
    }

    private void SetTiltDirection(float accelerationXVal)
    {
        if (!IsMobileTitltEnoughForMovement(accelerationXVal))
        {
            return;
        }


        if (accelerationXVal > MIN_TILT_FOR_ROTATION)
        {
            this.playerMovementDir = MovementDirection.Right;
            tiltDuration += Time.fixedDeltaTime;

            return;
        }

        if (accelerationXVal < -MIN_TILT_FOR_ROTATION)
        {
            this.playerMovementDir = MovementDirection.Left;
            tiltDuration += Time.fixedDeltaTime;

            return;
        }

        this.playerMovementDir = MovementDirection.Idle;
        tiltDuration = 0.0f;
    }

    private void TitltPlayer(MovementDirection movementState)
    {
        switch (movementState)
        {
            case MovementDirection.Right:
                transform.rotation = Quaternion.Euler(0, 0, -15);
                break;

            case MovementDirection.Left:
                transform.rotation = Quaternion.Euler(0, 0, 15);
                break;

            default:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
    }

    private bool IsMobileTitltEnoughForMovement(float accelerationXVal) => Mathf.Abs(accelerationXVal) > MIN_TITLT_FOR_MOVEMENT;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (FallManager.Instance.isGameOver)
        {
            return;
        }

        print("Player Collided with " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Obstacle") && !isInGodMode)
        {
            HandelBadGameOver();
        }
    }

    public void StartGodMode()
    {

    }
    private void HandelBadGameOver()
    {
        print("Game Over Event Triggered - PlayerController");
        gameObject.SetActive(false);
        BadGameOver?.Invoke();
    }

    private void HandelGoodGameOver()
    {
        print("Game Over Event Triggered - PlayerController, Player won!");
    }


    // We use Keyboard for testing purposes, making it easier & faster to test for collisions and other gameplay elements
    // On general this is a mobile only game, keyboard input allows you for movement that is not allowed on mobile(like vertical movement)
    private MovementDirection? HandleKeyBoardMovment()
    {
        float speed = 100.0f;
        float horizontal = 0.0f;
        float vertical = 0.0f;
        MovementDirection? movementdir = null;

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            horizontal = -speed * Time.deltaTime;
            movementdir = MovementDirection.Left;
        }
        else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            horizontal = speed * Time.deltaTime;
            movementdir = MovementDirection.Right;
        }

        if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
        {
            vertical = -speed * Time.deltaTime;
        }
        else if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        {
            vertical = speed * Time.deltaTime;
        }

        Vector2 position = transform.position;
        position.x += 0.1f * horizontal;
        position.y += 0.1f * vertical;

        transform.position = position;

        return movementdir;
    }

    //stub
    private IEnumerator PlaceholderTimer()

    {
        yield return new WaitForSeconds(godModeDuration);
        animator.SetTrigger("PowerUpEnd");
        isInGodMode = false;
        playerColiider.enabled = true;
    }

    public void ActivatePowerUp()
    {

        if (animator != null)
        {
            isInGodMode = true;
            playerColiider.enabled = false;
            animator.SetTrigger("CollectNyanCat");
            StartCoroutine(PlaceholderTimer());
        }

        Debug.Log("You hit nyan cat");
    }
}

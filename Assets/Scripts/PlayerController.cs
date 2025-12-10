using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private enum MovementDirection
    {
        Idle,
        Left,
        Right
    }

    private MovementDirection playerMovementDir;
    private float tiltDuration = 0.0f;
    private float moveSpeed = 7.0f;


    public static event Action GameOver;
    public const float MAX_MOVEMENT_SPEED = 15.0f;

    private void Start()
    {
        this.gameObject.SetActive(true);
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
        var accelerationXVal = Input.acceleration.x;
        MoveToNewPos(accelerationXVal);
        SetTiltDirection(accelerationXVal);
    }

    private void MoveToNewPos(float accelerationXVal)
    {
        float currentSpeed = Mathf.Lerp(moveSpeed, MAX_MOVEMENT_SPEED, tiltDuration / 2.0f);
        var dx = accelerationXVal * currentSpeed * Time.fixedDeltaTime;

        Vector2 position = transform.position;
        position.x += dx * 0.8f;
        transform.position = position;
    }

    private void SetTiltDirection(float accelerationXVal)
    {
        if (accelerationXVal > 0.1f)
        {
            this.playerMovementDir = MovementDirection.Right;
            tiltDuration += Time.fixedDeltaTime;

            return;
        }

        if (accelerationXVal < -0.1f)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Player Collided with " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            HandelGameOver();
        }
    }

    private void HandelGameOver()
    {
        print("Game Over Event Triggered - PlayerController");
        gameObject.SetActive(false);
        GameOver?.Invoke();
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
}

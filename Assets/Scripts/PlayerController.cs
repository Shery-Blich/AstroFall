using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerController : MonoBehaviour
{
    public static event Action GameOver;

    void Update()
    {
        HandleKeyBoardMovment();
    }

    // We Use Keyboard for easier testing
    private void HandleKeyBoardMovment()
    {
        float speed = 100.0f;
        float horizontal = 0.0f;
        float vertical = 0.0f;

        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            print("Left Arrow Key is held down");
            horizontal = -speed * Time.deltaTime;
            TitltPlayer(-1);
        }
        else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            print("Right Arrow Key is held down");
            horizontal = speed * Time.deltaTime;
            TitltPlayer(1);
        }
        else
        {
            TitltPlayer(0);
        }

        if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
        {
            print("Down Arrow Key is held down");
            vertical = -speed * Time.deltaTime;
        }
        else if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        {
            print("Up Arrow Key is held down");
            vertical = speed * Time.deltaTime;
        }

        Vector2 position = transform.position;
        position.x += 0.1f * horizontal;
        position.y += 0.1f * vertical;

        transform.position = position;
    }

    private void TitltPlayer(float tiltValue)
    {
        switch(tiltValue)
        {
            case > 0:
                transform.rotation = Quaternion.Euler(0, 0, -15);
                break;

            case < 0:
                transform.rotation = Quaternion.Euler(0, 0, 15);
                break;

            default:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameOver.Invoke();
    }
}

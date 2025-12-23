using System.Collections;
using UnityEngine;

public class NyanCatPickUp : BasePickup
{
    protected override void OnPickup(Collider2D collision)
    {
       PlayerController playerController = collision.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.ActivatePowerUp();
        }

        Debug.Log("You hit nyan cat");

    }

}

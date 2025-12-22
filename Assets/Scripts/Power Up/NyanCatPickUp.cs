using UnityEngine;

public class NyanCatPickUp : BasePickup
{
    protected override void OnPickup(Collider2D collision)
    {
        Animator animator = collision.GetComponentInChildren<Animator>();

        if (animator != null )
        {
            animator.SetTrigger("CollectNyanCat");
        }
            
        Debug.Log("You hit nyan cat");
    }
}

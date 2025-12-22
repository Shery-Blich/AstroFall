using UnityEngine;

public abstract class BasePickup : MonoBehaviour
{
    protected abstract void OnPickup(Collider2D collision);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPickup(collision);
            Destroy(gameObject);
        }
    }
}

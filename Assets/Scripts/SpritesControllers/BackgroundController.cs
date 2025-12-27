using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundController : MonoBehaviour
{
    [SerializeField]
    private float startSpeed = 1.0f;

    public Vector2 direction = new Vector2(0, +1); 
    private bool isScrolling = true;

    private void OnEnable()
    {
        PlayerController.BadGameOver += StopScroll;
        FallManager.GoodGameOver += StopScroll;
    }

    private void OnDisable()
    {
        PlayerController.BadGameOver -= StopScroll;
        FallManager.GoodGameOver -= StopScroll;
    }

    void Update()
    {
        if (!isScrolling) return;

        transform.position += (Vector3)(direction.normalized * FallManager.Instance.GlobalSpeed * Time.deltaTime*startSpeed);
    }

    private void StopScroll()
    {
        isScrolling = false;
    }
}

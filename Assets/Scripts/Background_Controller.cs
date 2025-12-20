using UnityEngine;
using UnityEngine.UIElements;

public class Background_Controller : MonoBehaviour
{
    public Vector2 direction = new Vector2(0, +1); 
    public float speed = 1f;

    // private void OnEnable()
    //{
    //    PlayerController.BadGameOver += ShowEndScreen;
    //    FallManager.GoodGameOver += ShowEndScreen;
    //}

    //private void OnDisable()
    //{
    //    PlayerController.BadGameOver -= ShowEndScreen;
    //    FallManager.GoodGameOver -= ShowEndScreen;
    //}
   
    void Update()
    {
        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
    }
}

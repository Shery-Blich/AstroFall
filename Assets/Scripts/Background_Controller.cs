using UnityEngine;
using UnityEngine.UIElements;

public class Background_Controller : MonoBehaviour
{
    [SerializeField]
    private float startSpeed = 1.0f;


    public Vector2 direction = new Vector2(0, +1); 
    private float speed = 1f;

    public void UpdateSpeed(float speedMultiplier)
    {
        speed = speedMultiplier * startSpeed;
    }
   
    void Update()
    {
        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);
    }
}

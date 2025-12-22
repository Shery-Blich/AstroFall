using UnityEngine;

public class PathPaverController : MonoBehaviour
{
    [SerializeField]
    public PolygonCollider2D PlayerCollider;

    [SerializeField]
    public BoxCollider2D PathPaverCollider;

    [SerializeField]
    public float PathPaverMovementSpeed = 5.0f;

    public void Start()
    {
        var playerSize = PlayerCollider.bounds.size;

        PathPaverCollider.size = playerSize;
        PathPaverCollider.offset = PlayerCollider.offset;

        transform.position = new Vector3(Scre, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

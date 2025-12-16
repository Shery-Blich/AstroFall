using UnityEngine;

public class Trash : Obstacle
{
    [SerializeField]
    private float minRotationSpeed = 200f;
    private float maxRotationSpeed = 700f;


    protected override void UpdateObstcale()
    {
        base.UpdateObstcale();
        ApplyRandomSpin();
    }

    public void ApplyRandomSpin()
    {
        transform.Rotate(Vector3.forward, Random.Range(minRotationSpeed, maxRotationSpeed) * Time.deltaTime);
    }
}

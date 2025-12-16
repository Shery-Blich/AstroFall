using UnityEngine;

public class Trash : Obstacle
{
    protected override void StartObstacle()
    {
        base.StartObstacle();
        AddRandomRotation();

    }

    protected override void UpdateObstcale()
    {
        base.UpdateObstcale();
        AddRandomRotation();
    }

    private void AddRandomRotation() => this.rb.AddTorque(Random.Range(30f, 90f));
}

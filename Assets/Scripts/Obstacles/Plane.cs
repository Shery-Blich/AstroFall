using UnityEngine;

public class Plane : Obstacle
{
    public void FreezeRotation()
    {
        this.rb.rotation = 0;
        this.rb.freezeRotation = true;
    }

    protected override void AddForce(float minForce, float maxForce)
    {
        base.AddForce(minForce, maxForce);
        AddXVelocity();
    }

    public void AddXVelocity()
    {
        var xVelocity = Random.Range(-2f, 2f);
        this.rb.linearVelocityX = xVelocity;
    }
}

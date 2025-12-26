using UnityEngine;


// TODO: Allow Plane to come from the sides of the screen
public class Plane : Obstacle
{
    protected override void StartObstacle()
    {
        base.StartObstacle();
        this.spawnYLocation = 0.1f;
    }
    public void FreezeRotation()
    {
        this.rb.rotation = 0;
        this.rb.freezeRotation = true;
    }

    protected override float SetSize()
    {
        Size /= 2f;
        return base.SetSize();
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

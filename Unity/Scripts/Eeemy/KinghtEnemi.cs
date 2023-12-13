using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirections),typeof(Damageable))]
public class KinghtEnemi : Enemy_Walk
{
    public override bool ison { get; set; }
    public override bool isOnOn { get { return false; } }
    protected void newAwake()
    {
        base.Awake();
    }
    protected void newUpdate()
    {
        base.Update();
    }
    protected void newFixedUpdate()
    {
        base.FixedUpdate();
    }
    protected void newFilpDirection()
    {
        base.FilpDirection();
    }
    protected void newOnHit(int damage, Vector2 knockback)
    {
        base.OnHit(damage, knockback);
    }
    protected void newOnCliffDetected()
    {
        base.OnCliffDetected();
    }
    protected void newOnPlayerRearDetected()
    { 
        base.OnPlayerRearDetected();
    }
}

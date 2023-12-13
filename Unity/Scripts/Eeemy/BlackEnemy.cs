using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class BlackEnemy : Enemy_Walk
{

    //�~�������]�w
    public override bool ison { get 
        { 
            return base.GetBool("IsOn"); 
        } set
        {
            base._isOn = value;
            base.SetBool("IsOn", value);
        }
    }
    public override bool isOnOn { get { return true; } }
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
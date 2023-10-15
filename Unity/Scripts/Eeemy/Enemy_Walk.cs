using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


//創建一個抽象類
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public abstract class Enemy_Walk : MonoBehaviour
{
    public float walkAccleration = 3f;
    public float maxSpeed = 3f;
    public float walkSpeed = 3f;
    public float walkStopRat = 0.6f;

    public DetectionZone OnDetectionZone;
    public DetectionZone attackZone;
    public DetectionZone cliffDetetionZoon;
    public DetectionZone PlayerRearDeffectionZone;

    public Damageable playDamageble;

    Rigidbody2D rb; 
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;

    public enum WalkableDirection { Left, Right }
    private WalkableDirection _WalkDirection;
    private Vector2 WalkDirectionVector = Vector2.right;
    //方向
    public WalkableDirection WalkDirection
    {
        get { return _WalkDirection; }
        set
        {
            if (_WalkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    WalkDirectionVector = Vector2.left;
                }
                else if (value == WalkableDirection.Left)
                {
                    WalkDirectionVector = Vector2.right;
                }
            }
            _WalkDirection = value;
        }
    }
    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    { get { return animator.GetBool(AnimationStrings.canMove); } }

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.AttackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.AttackCooldown, Mathf.Max(value, 0));
        }
    }

    public bool lockIsOn;
    public bool _isOn;
    public abstract bool ison { get; set; }
    public abstract bool isOnOn { get;}


    public bool GetBool(string name)
    {
        return animator.GetBool(name);
    }
    public void SetBool(string name,bool value)
    {
        animator.SetBool(name, value);
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }
    public void Update()
    {
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
        if (playDamageble.IsAlive)
        {
            HasTarget = attackZone.detectColliders.Count > 0;
        }
        else
        {
            HasTarget = false;
        }
        if(isOnOn)
        {
            if (!lockIsOn)
            {

                ison = OnDetectionZone.detectColliders.Count > 0 || damageable.LockVelocity;
                if (ison)
                {
                    //呼叫出
                    CharacterEvents.characterText.Invoke(gameObject, "!");
                    lockIsOn = true;
                }
            }
        }
    }

    public void FixedUpdate()
    {
        if (!isOnOn)
        {
            if (touchingDirections.IsOnwall && touchingDirections.IsGrounded)
            {
                //Debug.Log("碰撞牆壁翻轉");
                FilpDirection();
            }   

            if (!damageable.LockVelocity)
            {
                if (CanMove && touchingDirections.IsGrounded)

                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (walkAccleration * WalkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed)
                        , rb.velocity.y);
                else
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRat), rb.velocity.y);
            }
        }
        else
        {
            if (ison)
            {
                if (touchingDirections.IsOnwall && touchingDirections.IsGrounded)
                {
                    FilpDirection();
                }

                if (!damageable.LockVelocity)
                {
                    if (CanMove && touchingDirections.IsGrounded)
                        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (walkAccleration * WalkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed)
                            , rb.velocity.y);
                    else
                        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRat), rb.velocity.y);
                }
            }
        }
        
    }

    public void FilpDirection()
    {
        if(!animator.GetBool("isattack"))
        {
            if (AttackCooldown == 0)
            {
                //Debug.Log("翻轉");
                if (WalkDirection == WalkableDirection.Right)
                {
                    WalkDirection = WalkableDirection.Left;
                }
                else if (WalkDirection == WalkableDirection.Left)
                {
                    WalkDirection = WalkableDirection.Right;
                }
            }
        }
        
        
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        // 玩家擊退
        // 假如原本的位置是負的也會相反
        //Debug.Log("擊退");
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);


    }
    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            //Debug.Log("下面空的翻轉");

            FilpDirection();
        }
    }
    public void OnPlayerRearDetected()
    {
        if (touchingDirections.IsGrounded && playDamageble.IsAlive)
        {
            //Debug.Log("偵測到玩家才翻轉");
            FilpDirection();
        }
    }
}


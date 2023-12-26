using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Unity.VisualScripting;
using UnityEngine;

public class Flyingeye : MonoBehaviour
{
    public float flightSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone bitedetectionZone;
    //節點列表
    public List<Transform> waypoints;
    
    Damageable Damageable;
    Animator animator;
    Rigidbody2D rb;
    
    // 追蹤的節點
    Transform nextWaypoint;
    int waypointNum = 0;


    public bool _hasTarget = false;
    //private Vector2 flinghtSped;
    //定義錯誤的變數


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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }
    void Update()
    {
        rb.velocity = new Vector2(1, 1);
        HasTarget = bitedetectionZone.detectColliders.Count > 0;
    }
    private void FixedUpdate()
    {
        if (Damageable.IsAlive)
        {
            
            if (CanMove)
            {
                
                Flight();
            }
            else
            {
                
                rb.velocity = Vector2.zero;

            }
        }
        else
        {
            rb.gravityScale = 2f;
            rb.velocity = Vector2.zero;
        }
    }
        

    private void Flight()
    {
        Vector2 directionToWaypontint =  (nextWaypoint.position - transform.position).normalized;
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);
       
        rb.velocity = directionToWaypontint * flightSpeed;
        UpdataDirection();

        if (distance <= waypointReachedDistance)
        {
            
            waypointNum++;
            if(waypointNum >= waypoints.Count)
            {
                
                waypointNum = 0;
            }
            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdataDirection()
    {
        Vector3 loocScale = transform.localScale;
        if(transform.localScale.x >0)
        {
            if(rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * loocScale.x, loocScale.y, loocScale.z);
            }
        }
        else
        {
            if(rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * loocScale.x,loocScale.y,loocScale.z );
            }
        }
    }
}

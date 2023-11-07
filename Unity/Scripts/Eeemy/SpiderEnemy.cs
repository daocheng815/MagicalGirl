
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : MonoBehaviour
{
    
    Animator animator;
    Damageable damageable;
    public DetectionZone cliffDetetionZoon;
    
    private bool _hasTarget;
    
    public bool HasTarget
    {
        get { return _hasTarget; }
        set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }
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

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();
    }
    

    void Update()
    {
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
        HasTarget = cliffDetetionZoon.detectColliders.Count > 0;
    }
 
}

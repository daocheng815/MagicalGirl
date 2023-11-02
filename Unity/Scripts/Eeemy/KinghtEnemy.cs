using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using EnemyState;
public class KinghtEnemy : MonoBehaviour
{
    //創建實例
    EnemyStateManager enemyStateManager = new EnemyStateManager();
    //巡邏點陣列
    public Transform[] patrolposition;
    private int currentPatrolIndex = 0;
    //Enemy移動速度
    public float walkAccleration = 3f;
    public float maxSpeed = 3f;
    public float walkSpeed = 3f;
    public float walkStopRat = 0.6f;
    // 搜索玩家範圍(敵人視野)
    public float pursueDistance = 12;
    // 攻擊距離
    public float attackDistance = 6;
    // 攻擊後追擊距離
    public float hitPursueDistance = 16;
    //追擊延遲距離
    public float pursueDelayDistance = 2;
    [SerializeField]private Damageable playDamageble;
    //玩家是否在敵人正面
    private bool detectionFront =>
        WalkDirection == WalkableDirection.Right ?
        enemySearchToPlayers.direction.x < 0.9 && WalkDirection == WalkableDirection.Right:
        enemySearchToPlayers.direction.x > -0.9 && WalkDirection == WalkableDirection.Left;
    public bool Front;
    
    Rigidbody2D rb; 
    TouchingDirections touchingDirections;
    /// <summary>
    /// 搜尋玩家位置
    /// </summary>
    EnemySearchToPlayers enemySearchToPlayers;
    Animator animator;
    Damageable damageable;
    //翻轉方向列舉
    public enum WalkableDirection { Left, Right }
    private WalkableDirection _WalkDirection;
    private Vector2 WalkDirectionVector = Vector2.left;
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
    private void Awake()
    {
        playDamageble = GameObject.FindWithTag("Player").GetComponent<Damageable>();
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        enemySearchToPlayers = GetComponent<EnemySearchToPlayers>();
    }
    private void Start()
    {
        enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Patrol;
    }

    private void Update()
    {
        Front = detectionFront;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
        if (playDamageble.IsAlive)
        {
            //進入攻擊距離 與 正在玩家正面 時 攻擊
            HasTarget = enemySearchToPlayers.distance < attackDistance && detectionFront;
        }
        else
        {
            HasTarget = false;
        }
        //查看當前狀態機
        //Debug.Log(enemyStateManager.GetCurrentStateString());
    }
    /// <summary>
    /// 翻轉
    /// </summary>
    public void FlipCharacter()
    {
        if(!animator.GetBool("isattack"))
        {
            if (AttackCooldown == 0)
            {
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
    private void FixedUpdate()
    {
        if (!detectionFront)
            if(enemySearchToPlayers.distance < pursueDistance)
                FlipCharacter();
        if(touchingDirections.IsOnwall && touchingDirections.IsGrounded)
            FlipCharacter();
        //============
        //巡邏狀態機
        //============
        if (enemyStateManager.IsCurrentState(ES.Patrol))
        {
            if (enemySearchToPlayers.distance < pursueDistance)
            {
                
                enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Pursue;
                return;
                
            }
            if (!damageable.LockVelocity)
            {
                if (CanMove && touchingDirections.IsGrounded)
                {
                    Vector2 directionToPatrol = (patrolposition[currentPatrolIndex].position - transform.position).normalized;
                    rb.velocity = new Vector2(directionToPatrol.x * maxSpeed, rb.velocity.y);
                    //移動方向與敵人方向藕同
                    if(rb.velocity.x > 0)
                        WalkDirection = WalkableDirection.Left;
                    if(rb.velocity.x < 0)
                        WalkDirection = WalkableDirection.Right;
                    if (touchingDirections.IsOnwall && touchingDirections.IsGrounded)
                    {
                        FlipCharacter();
                        currentPatrolIndex = (currentPatrolIndex + 1) % patrolposition.Length;
                    }
                    if (Vector2.Distance(transform.position, patrolposition[currentPatrolIndex].position) < 0.1f)
                    {
                        FlipCharacter();
                        currentPatrolIndex = (currentPatrolIndex + 1) % patrolposition.Length;
                    }
                    
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
        }
        //====================================
        //追擊狀態機 與 受傷害後的追擊狀態機
        //====================================
        if (enemyStateManager.IsCurrentState(ES.Pursue) || enemyStateManager.IsCurrentState(ES.Hit))
        {
            
            switch (enemyStateManager.CurrentState)
            {
                case EnemyStateManager.EnemyState.Pursue:
                    //普通追擊跳出距離
                    if (enemySearchToPlayers.distance > pursueDistance)
                    {
                        enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Patrol;
                        return;
                    }
                    break;
                case EnemyStateManager.EnemyState.Hit:
                    //受傷追擊跳出距離
                    if (enemySearchToPlayers.distance > hitPursueDistance)
                    {
                        enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Patrol;
                        return;
                    }
                    break;
            }
            
            if (!damageable.LockVelocity)
            {
                if (CanMove && touchingDirections.IsGrounded)
                {
                    //敵人需要超出追擊玩家點一定的距離這樣才有時間攻擊
                    Vector2 playWorldLocation = new Vector2(enemySearchToPlayers.PlayerWorldLocation.x , transform.position.y);
                    if(WalkDirection == WalkableDirection.Left)
                        playWorldLocation = new Vector2(enemySearchToPlayers.PlayerWorldLocation.x + pursueDelayDistance , transform.position.y);
                    if(WalkDirection == WalkableDirection.Right)
                        playWorldLocation = new Vector2(enemySearchToPlayers.PlayerWorldLocation.x - pursueDelayDistance , transform.position.y);
                    rb.velocity = (playWorldLocation - (Vector2)transform.position).normalized * maxSpeed;
                    if(rb.velocity.x > 0)
                        WalkDirection = WalkableDirection.Left;
                    if(rb.velocity.x < 0)
                        WalkDirection = WalkableDirection.Right;
                }
                else
                {
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRat), rb.velocity.y);
                }
            }
        }
        //============
        //距離修正
        //============
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        // 玩家擊退，假如原本的位置是負的也會相反
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        //將狀態機設定為受傷
        enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Hit;
    }
}

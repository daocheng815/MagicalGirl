using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyState;
using Unity.VisualScripting;

public class HeadlessAssassin : MonoBehaviour
{
    // 創建實例
    EnemyStateManager enemyStateManager = new EnemyStateManager();
    // 巡邏點陣列
    public Transform[] patrolposition;
    private int currentPatrolIndex = 0;
    // Enemy移動速度
    public float walkAccleration = 3f;
    public float maxSpeed = 3f;
    public float walkSpeed = 3f;
    public float walkStopRat = 0.6f;
    // 等待狀態時間
    public float idleDelayTime = 0.5f;
    public float randomIdleDelayTime = 2f;
    // 搜索玩家範圍(敵人視野)
    public float pursueDistance = 12;
    // 攻擊距離
    public float attackDistance = 6;
    // 攻擊後追擊距離
    public float hitPursueDistance = 16;
    // 追擊延遲距離
    public float pursueDelayDistance = 2;
    [SerializeField]private Damageable playDamageble;
    // 玩家是否在敵人正面
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
    Detection detection;
    Animator animator;
    Damageable damageable;
    
    //翻轉方向列舉
    public enum WalkableDirection {Right,Left}
    private WalkableDirection _WalkDirection;
    private Vector2 WalkDirectionVector = Vector2.right;
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
        detection = GetComponent<Detection>();
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
        Debug.Log("翻轉");
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

    private bool isIdleDelayTime = false;
    private int jumpcount = 0;
    private bool isjump = false;
    public float jumpY = 6.5f;
    public float jumpX = 6.5f;
    private float jump_x;
    public float jumpDelayTime = 0.3f;
    
    /// <summary>
    /// 跳躍
    /// </summary>
    /// <param name="DirectionX">傳遞X軸方向</param>
    private void Jump()
    {
        isjump = false;
        jumpcount = 0;
        jump_x = Mathf.Sign(rb.velocity.x) * 1f;
        enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Jump;
    }

    private void  Junpdelaytime()
    {
        isjump = true;
    }
    private void FixedUpdate()
    {
        Debug.Log(enemyStateManager.GetCurrentStateString());
        //如果玩家不在正面且在視野範圍內翻轉
        if (!detectionFront)
            if(enemySearchToPlayers.distance < pursueDistance)
                FlipCharacter();
        //if(touchingDirections.IsOnwall && touchingDirections.IsGrounded && detection.isSlope)
        //    FlipCharacter();
        //============
        //等待狀態機
        //============
        if (enemyStateManager.IsCurrentState((ES.Idle)))
        {
            animator.SetBool(AnimationStrings.isMoving,false);
            rb.velocity = Vector2.zero;
            var delayTime = UnityEngine.Random.Range(idleDelayTime,randomIdleDelayTime);
            if (!isIdleDelayTime)
                StartCoroutine(IdleDelayTime(delayTime, EnemyStateManager.EnemyState.Patrol));
            //等待狀態機協程
            IEnumerator IdleDelayTime(float delayTime,EnemyStateManager.EnemyState EnemyState)
            {
                //正在等待
                isIdleDelayTime = true;
                //計時器
                float timer = 0f;
                
                while (timer < delayTime)
                {
                    timer += Time.deltaTime;
                    //等待時間內若是靠近就進入追擊
                    if (enemySearchToPlayers.distance < pursueDistance)
                    {
                        enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Pursue;
                        break;
                    }
                    yield return null;
                }
                //回復運動狀態
                animator.SetBool(AnimationStrings.isMoving,true);
                //等待結束
                isIdleDelayTime = false;
                //設定下一個狀態
                if (!(enemySearchToPlayers.distance < pursueDistance))
                    enemyStateManager.CurrentState = EnemyState;

            }
        }
        //============
        //跳躍狀態機
        //============
        if (enemyStateManager.IsCurrentState(ES.Jump))
        {
            animator.SetBool(AnimationStrings.isMoving,false);
            Debug.Log("跳躍");
            if (jumpcount == 0)
            {
                //rb.velocity = new Vector2(jump_x * maxSpeed * jumpX, jumpY);
                rb.velocity = new Vector2(jump_x * maxSpeed , jumpY);
                Debug.Log(rb.velocity);
                if(!isjump)
                    Invoke("Junpdelaytime",0.3f);
                jumpcount++;
                
            }
           
            else if (jumpcount != 0 && isjump)
            {
                animator.SetBool(AnimationStrings.isMoving,true);
                enemyStateManager.CurrentState = enemyStateManager.previousState;
            }
            //if (CanMove && touchingDirections.IsGrounded && jumpcount == 0)
            //{
            //    jumpcount += 1;
            //    rb.velocity = new Vector2( jumpX, jumpY);

            //}
        }
        //============
        //巡邏狀態機
        //============
        if (enemyStateManager.IsCurrentState(ES.Patrol))
        {
            //狀態機判定 === 判定如果距離在視野內進行追擊
            if (enemySearchToPlayers.distance < pursueDistance)
            {
                enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Pursue;
                return;
            }
            //狀態邏輯
            if (!damageable.LockVelocity)
            {
                if (CanMove && touchingDirections.IsGrounded)
                {
                    Vector2 directionToPatrol = (patrolposition[currentPatrolIndex].position - transform.position).normalized;
                    rb.velocity = new Vector2(directionToPatrol.x * maxSpeed, rb.velocity.y);
                    //如果撞到牆壁且不在斜坡上
                    if (detection.isWall && !detection.isSlope)
                    {
                        //rb.velocity = new Vector2(rb.velocity.x + 6.5f, rb.velocity.y  + 6.5f);
                        Jump();
                    }
                    //移動方向與敵人方向藕同
                    if(rb.velocity.x > 0)
                        WalkDirection = WalkableDirection.Left;
                    if(rb.velocity.x < 0)
                        WalkDirection = WalkableDirection.Right;
                    //if (touchingDirections.IsOnwall && touchingDirections.IsGrounded)
                    //{
                    //    FlipCharacter();
                    //    currentPatrolIndex = (currentPatrolIndex + 1) % patrolposition.Length;
                    //}
                    if(Mathf.Abs(transform.position.x - patrolposition[currentPatrolIndex].position.x) < 0.1f)
                    //if (Vector2.Distance(transform.position, patrolposition[currentPatrolIndex].position) < 0.1f)
                    {
                        //FlipCharacter();
                        currentPatrolIndex = (currentPatrolIndex + 1) % patrolposition.Length;
                        Debug.Log("到達");
                        enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Idle;
                        
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
            //狀態機判定
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
            //狀態邏輯
            if (!damageable.LockVelocity)
            {
                if (CanMove && touchingDirections.IsGrounded)
                {
                    
                    //敵人需要超出追擊玩家點一定的距離這樣才有時間攻擊，做個距離修正
                    Vector2 playWorldLocation = new Vector2(enemySearchToPlayers.PlayerWorldLocation.x , transform.position.y);
                    if(WalkDirection == WalkableDirection.Left)
                        playWorldLocation = new Vector2(enemySearchToPlayers.PlayerWorldLocation.x + pursueDelayDistance , transform.position.y);
                    if(WalkDirection == WalkableDirection.Right)
                        playWorldLocation = new Vector2(enemySearchToPlayers.PlayerWorldLocation.x - pursueDelayDistance , transform.position.y);
                    rb.velocity = (playWorldLocation - (Vector2)transform.position).normalized * maxSpeed;
                    //如果撞到牆壁且不在斜坡上
                    if (detection.isWall && !detection.isSlope)
                    {
                        //rb.velocity = new Vector2(rb.velocity.x + 6.5f, rb.velocity.y  + 6.5f);
                        Jump();
                    }
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
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        // 玩家擊退，假如原本的位置是負的也會相反
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        //將狀態機設定為受傷
        enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Hit;
    }
}

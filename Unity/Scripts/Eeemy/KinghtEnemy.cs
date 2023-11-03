using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyState;


public class KinghtEnemy : MonoBehaviour
{
    // �Ыع��
    EnemyStateManager enemyStateManager = new EnemyStateManager();
    // �����I�}�C
    public Transform[] patrolposition;
    private int currentPatrolIndex = 0;
    // Enemy���ʳt��
    public float walkAccleration = 3f;
    public float maxSpeed = 3f;
    public float walkSpeed = 3f;
    public float walkStopRat = 0.6f;
    // ���ݪ��A�ɶ�
    public float idleDelayTime = 0.5f;
    public float randomIdleDelayTime = 2f;
    // �j�����a�d��(�ĤH����)
    public float pursueDistance = 12;
    // �����Z��
    public float attackDistance = 6;
    // ������l���Z��
    public float hitPursueDistance = 16;
    // �l������Z��
    public float pursueDelayDistance = 2;
    [SerializeField]private Damageable playDamageble;
    // ���a�O�_�b�ĤH����
    private bool detectionFront =>
        WalkDirection == WalkableDirection.Right ?
        enemySearchToPlayers.direction.x < 0.9 && WalkDirection == WalkableDirection.Right:
        enemySearchToPlayers.direction.x > -0.9 && WalkDirection == WalkableDirection.Left;
    public bool Front;
    
    Rigidbody2D rb; 
    TouchingDirections touchingDirections;
    /// <summary>
    /// �j�M���a��m
    /// </summary>
    EnemySearchToPlayers enemySearchToPlayers;
    Animator animator;
    Damageable damageable;
    //½���V�C�|
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
            //�i�J�����Z�� �P ���b���a���� �� ����
            HasTarget = enemySearchToPlayers.distance < attackDistance && detectionFront;
        }
        else
        {
            HasTarget = false;
        }
        //�d�ݷ�e���A��
        //Debug.Log(enemyStateManager.GetCurrentStateString());
    }
    /// <summary>
    /// ½��
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

    private bool isIdleDelayTime = false;
    private void FixedUpdate()
    {
        if (!detectionFront)
            if(enemySearchToPlayers.distance < pursueDistance)
                FlipCharacter();
        if(touchingDirections.IsOnwall && touchingDirections.IsGrounded)
            FlipCharacter();
        //============
        //���ݪ��A��
        //============
        if (enemyStateManager.IsCurrentState((ES.Idle)))
        {
            animator.SetBool(AnimationStrings.isMoving,false);
            rb.velocity = Vector2.zero;
            var delayTime = UnityEngine.Random.Range(idleDelayTime,randomIdleDelayTime);
            if (!isIdleDelayTime)
                StartCoroutine(IdleDelayTime(delayTime, EnemyStateManager.EnemyState.Patrol));
            //���ݪ��A����{
            IEnumerator IdleDelayTime(float delayTime,EnemyStateManager.EnemyState EnemyState)
            {
                //���b����
                isIdleDelayTime = true;
                //�p�ɾ�
                float timer = 0f;
                while (timer < delayTime)
                {
                    timer += Time.deltaTime;
                    //���ݮɶ����Y�O�a��N�i�J�l��
                    if (enemySearchToPlayers.distance < pursueDistance)
                    {
                        enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Pursue;
                        break;
                    }
                    yield return null;
                }
                //�^�_�B�ʪ��A
                animator.SetBool(AnimationStrings.isMoving,true);
                //���ݵ���
                isIdleDelayTime = false;
                //�]�w�U�@�Ӫ��A
                if (!(enemySearchToPlayers.distance < pursueDistance))
                    enemyStateManager.CurrentState = EnemyState;

            }
        }
        //============
        //���ު��A��
        //============
        if (enemyStateManager.IsCurrentState(ES.Patrol))
        {
            //���A���P�w === �P�w�p�G�Z���b�������i��l��
            if (enemySearchToPlayers.distance < pursueDistance)
            {
                
                enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Pursue;
                return;
                
            }
            //���A�޿�
            if (!damageable.LockVelocity)
            {
                if (CanMove && touchingDirections.IsGrounded)
                {
                    Vector2 directionToPatrol = (patrolposition[currentPatrolIndex].position - transform.position).normalized;
                    rb.velocity = new Vector2(directionToPatrol.x * maxSpeed, rb.velocity.y);
                    //���ʤ�V�P�ĤH��V�¦P
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
                        //FlipCharacter();
                        currentPatrolIndex = (currentPatrolIndex + 1) % patrolposition.Length;
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
        //�l�����A�� �P ���ˮ`�᪺�l�����A��
        //====================================
        if (enemyStateManager.IsCurrentState(ES.Pursue) || enemyStateManager.IsCurrentState(ES.Hit))
        {
            //���A���P�w
            switch (enemyStateManager.CurrentState)
            {
                case EnemyStateManager.EnemyState.Pursue:
                    //���q�l�����X�Z��
                    if (enemySearchToPlayers.distance > pursueDistance)
                    {
                        enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Patrol;
                        return;
                    }
                    break;
                case EnemyStateManager.EnemyState.Hit:
                    //���˰l�����X�Z��
                    if (enemySearchToPlayers.distance > hitPursueDistance)
                    {
                        enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Patrol;
                        return;
                    }
                    break;
            }
            //���A�޿�
            if (!damageable.LockVelocity)
            {
                if (CanMove && touchingDirections.IsGrounded)
                {
                    //�ĤH�ݭn�W�X�l�����a�I�@�w���Z���o�ˤ~���ɶ������A���ӶZ���ץ�
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
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        // ���a���h�A���p�쥻����m�O�t���]�|�ۤ�
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        //�N���A���]�w������
        enemyStateManager.CurrentState = EnemyStateManager.EnemyState.Hit;
    }
}

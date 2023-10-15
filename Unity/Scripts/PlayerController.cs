using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


// 玩家行為控制

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public PMagic pmagic;
    public float walkSpeed = 5f ;
    public float runspeed = 8f;
    public float airWalkSpeed = 3f ;
    public float jumpImpulse = 10f ;
    public static bool lockplay = false;

    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;

    //玩家速度
    public float CurrentMoveSpeed{
        get
        {
            if(canMove)
            {
                // 防止卡牆(相反的 卡牆 可以當作一個抓住牆面的技能)
                if (IsMoving)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runspeed ;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airWalkSpeed ;
                    }
                    
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }}
    
    

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving { get
        {
            
            return _isMoving;
        }
        set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    public bool isJump {  get {return animator.GetBool(AnimationStrings.jumpTrigger); }  }


    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning{ get 
        { 
            return _isRunning;
        }
        set 
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }


    [SerializeField]
    private bool _isSlide = false;
    public bool IsSlide
    {
        get { return _isSlide;}
        set
        {
            _isSlide = value;
            animator.SetBool(AnimationStrings.Slide, value);
        }
    }


    public bool _isFacingRight = true;

    public bool IsFacingRight { get 
        { 
            return _isFacingRight; 
        } 
        set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight=value;
        } 
    }

    public bool canMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        } }
    
    public bool IsAlive { 
        get
        {
            return animator.GetBool(AnimationStrings.IsAlive);
        } 
    }


    
    Rigidbody2D rb;
    Animator animator;

    // Start is called before the first frame update
    private void Awake()
    {
        Transform gameObject = transform.Find("setMenu");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        
    }

    void Start()
    {

        if (ScreenSetting.GameLoadNum == 0)
            invventoryManger.deltbag();

       
    }

    private void FixedUpdate()
    {
        if (!lockplay)
        {
            if (!IsSlide)
            {
                if (!damageable.LockVelocity)
                {

                    rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
                    //Debug.Log(rb.velocity);

                }
            }
            else if (!damageable.LockVelocity && touchingDirections.IsGrounded)
            {
                
                if (IsFacingRight)
                { 
                    Vector2 Np = rb.position + new Vector2(0.4f, moveInput.y);
                    rb.MovePosition(Np);
                }
                else
                {
                    Vector2 Np = rb.position + new Vector2(-0.4f, moveInput.y);
                    rb.MovePosition(Np);
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
     
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        animator.SetFloat(AnimationStrings.xVelocity, rb.velocity.x);


    }

    private int MoveCount = 0;

    public void ReMoveCount(){MoveCount = 0;}

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!lockplay)
        {
            if (context.canceled)
            {
                IsRunning = false;
            }
            if (context.started)
            {
                switch(MoveCount)
                {
                    case 0:
                        //Debug.Log("走路");
                        Invoke("ReMoveCount", 0.3f);
                        break;
                    case 1:
                        //Debug.Log("跑步");
                        IsRunning = true;
                        break;
                    case 2:
                        MoveCount = 0;
                        break;

                }
                MoveCount++;
            }

            moveInput = context.ReadValue<Vector2>();
            if (IsAlive)
            {
                IsMoving = moveInput != Vector2.zero;
                SetFacingDirection(moveInput);
            }
            else
            {
                IsMoving = false;
                
            }
        }
    }

    
    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x >0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }else if (moveInput.x <0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }
    public void OnRun(InputAction.CallbackContext context) 
    {

        if (!lockplay)
        {
            if (context.started)
            {
                IsRunning = true;
            }
            else if (context.canceled)
            {
                IsRunning = false;
            }
        }
            
        
    }
    private int jumpCount = 0;
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!lockplay)
            if (IsAlive && canMove)
            {
                if (context.started)
                {
                    if (touchingDirections.IsGrounded)
                    {
                    
                        jumpCount = 1;
                        PerformJump();
                    }
                    else if (jumpCount < 2)
                    {
                    
                        jumpCount = 2;
                        PerformJump();
                    }
               
                }
            }
    }

    private void PerformJump()
    {
    
        animator.SetTrigger(AnimationStrings.jumpTrigger);
        rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (touchingDirections.IsGrounded)
        {
            animator.ResetTrigger(AnimationStrings.jumpTrigger);
            jumpCount = 0; 
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!lockplay)
            if (context.started)
            {
                animator.SetTrigger(AnimationStrings.attackTrigger);
            }
    }

    public void OnRangeAttack(InputAction.CallbackContext context)
    {
        if (!lockplay && pmagic.IsMagic(20))
            if (context.started)
            {
               
                animator.SetTrigger(AnimationStrings.rangeaAttackTrigger);
            }
    }


    public void OnHit(int damage, Vector2 knockback)
    {
        if (!lockplay)
            damageable.LockVelocity = true;
            // 玩家擊退

            rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    // 閃避

    private bool iss = false;
    public void OnSlide(InputAction.CallbackContext context)
    {
            
        // 未完成 打算再做一條用來表示體力 與顯示冷卻時間

        //if (context.started && !iss && touchingDirections.IsGrounded &&IsAlive && !lockplay && pmagic.IsMagic(20))
        if (context.started && !iss && IsAlive && !lockplay && pmagic.IsMagic(20))
        {
            iss = true;
            IsSlide = true;
            pmagic.OnMagic(20);
            StartCoroutine(slide(0.3f));
            
            //Debug.Log("閃避");
        }
        //協程方式不太好，之後記得改成Invoke方法。
        IEnumerator slide(float time)
        {
            yield return new WaitForSeconds(time);
            
            IsSlide = false;
            //Debug.Log("閃避完成");
            StartCoroutine(FadeSlide(1f));
        }
        //冷卻時間
        IEnumerator FadeSlide(float time)
        {
            yield return new WaitForSeconds(time);
            iss = false;
        }
    }

    

    // 牆跳
    private bool IsScratch;
    public void OnScratch(InputAction.CallbackContext context)
    {
        if(!lockplay&&pmagic.IsMagic(45))
            if (context.started && touchingDirections.IsOnwall && IsScratch )
            {   
                if (!(touchingDirections.IsGrounded && touchingDirections.IsOnwall))
                {
                    pmagic.OnMagic(45);
                    Debug.Log("牆跳");
                    rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
                    IsScratch = false;
                }
            }
            else
            {
                IsScratch = true;
            }
    }


    // 牆跳 結束
    // 玩家身上的UI
    //魔法選單

    public Transform MagicUI_launchPoint;
    public GameObject MagicUI_projectilePrefad;
    

    private bool ISMagicUI = false;
    public void OnMagicUI(InputAction.CallbackContext context)
    {   
        
        Transform SK = gameObject.transform.Find("SKILL");

        Transform SKU = SK.gameObject.transform.Find("U").transform;
        Transform SKD = SK.gameObject.transform.Find("D").transform;
        Transform SKR = SK.gameObject.transform.Find("R").transform;
        Transform SKL = SK.gameObject.transform.Find("L").transform;

        SpriteRenderer SKUU = SKU.GetComponent<SpriteRenderer>();
        SpriteRenderer SKUD = SKD.GetComponent<SpriteRenderer>();
        SpriteRenderer SKUR = SKR.GetComponent<SpriteRenderer>();
        SpriteRenderer SKUL = SKL.GetComponent<SpriteRenderer>();
        
        GameObject SKObject = SK.gameObject;

        
        void FAN(int i)
        {   
            Vector3 A = new Vector3(1f, 1f, 0f);
            Vector3 B = new Vector3(1.2f, 1.2f, 0f);
            Color CA = new Color(0.1f, 0.1f, 0.1f, 1);
            Color CB = new Color(1, 1, 1, 1); 
            SKU.localScale = A;
            SKD.localScale = A;
            SKR.localScale = A;    
            SKL.localScale = A;
            SKUU.color = CA;
            SKUD.color = CA;
            SKUR.color = CA;
            SKUL.color = CA;
            switch(i){
                case 1:
                    SKU.localScale = B;
                    SKUU.color = CB;
                    break;
                 
                case 2: 
                    SKD.localScale = B;
                    SKUD.color = CB;
                    break;
                 
                case 3:
                    SKL.localScale = B;
                    SKUL.color = CB;
                    break;
                
                case 4: 
                    SKR.localScale = B;
                    SKUR.color = CB;
                    break;
            }
        }

        if (!ISMagicUI && IsAlive && pmagic.IsMagic(10))
        {

            Time.timeScale = 0.2f;

            IsMoving = false;
            IsRunning = false;
            rb.velocity = Vector2.zero;

            FAN(0);

            // 如果打開了這個介面就要完整地等待完時間才能再次打開這個UI
            // 物件組件 SK 取得 某個物件

            if (context.started)
            {
                
                lockplay = true;
                Debug.Log("yes");
            }
            if (context.canceled)
            {

                SKObject.SetActive(true);
                lockplay = true;
                StartCoroutine(ExecuteAfterDelay(0.5f));
            }
        }
            
        
        // 協程
        IEnumerator ExecuteAfterDelay(float delayInSeconds)
        {
            ISMagicUI = true;
            float timer = 0f;
            int C = 0;
            while (timer < delayInSeconds)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    FAN(1);
                    if (C == 1 && pmagic.IsMagic(55)) 
                    {   
                        pmagic.OnMagic(55);

                        //GameObject projectlie = Instantiate(MagicUI_projectilePrefad, MagicUI_launchPoint.position, MagicUI_projectilePrefad.transform.rotation);
                        Debug.Log("確定上");
                        Invoke("OnMagic_A", 0.3f);
                        break; 
                    }
                    C = 1;
                    if (C == 0) { Debug.Log("上"); }
                }
                if (Input.GetKeyDown(KeyCode.S))
                {

                    FAN(2);
                    if (C == 2 && pmagic.IsMagic(55)) { Debug.Log("確定下");
                        pmagic.OnMagic(55);
                        //GameObject projectlie = Instantiate(MagicUI_projectilePrefad, MagicUI_launchPoint.position, MagicUI_projectilePrefad.transform.rotation);
                        Invoke("OnMagic_B", 0.3f);
                        break; }
                    C = 2;
                    if (C == 0) { Debug.Log("下"); }
                }

                if (_isFacingRight) // 翻轉
                {
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        FAN(3);
                        if (C == 3) { Debug.Log("確定右");
                            
                            break; }
                        C = 3;
                        if (C == 0) { Debug.Log("右"); }
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {

                        FAN(4);
                        if (C == 4) { Debug.Log("確定左"); break; }
                        C = 4;
                        if (C == 0) { Debug.Log("左"); }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        FAN(4);
                        if (C == 3) { Debug.Log("確定右");
                            
                            break; }
                        C = 3;
                        if (C == 0) { Debug.Log("右"); }
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {

                        FAN(3);
                        if (C == 4) { Debug.Log("確定左"); break; }
                        C = 4;
                        if (C == 0) { Debug.Log("左"); }
                    }
                }

                
                timer += Time.deltaTime;
                yield return null;
            }

            ISMagicUI = false;
            SKObject.SetActive(false);
            lockplay = false;
            Debug.Log("2秒後");
            Time.timeScale = 1f;
        }
    }


    public GameObject BPrefad;
    public void OnMagic_A()
    {
        int i = 0;
        while (i< 360)
        {
            GameObject Magic_A = Instantiate(BPrefad, MagicUI_launchPoint.position, Quaternion.Euler(0,0,i));
            Rigidbody2D Mrb = Magic_A.GetComponent<Rigidbody2D>();
            
            float rd = Mathf.Deg2Rad * i;
            float sd = 5.0f;
            Vector2 v2 = new Vector2(Mathf.Cos(rd) * sd, Mathf.Sin(rd) *sd);
            Mrb.velocity = v2;
            i += 45;
        }
    }
    public void OnMagic_B()
    {
        int i = 0;

        while (i < 360)
        {
           
            float maxRadius = 5.0f;
            int angleIncrement = 10; 
            
            float currentRadius = i * 0.1f;
            if (currentRadius > maxRadius)
            {
                currentRadius = maxRadius;
            }

            
            Vector2 spawnPosition = MagicUI_launchPoint.position + new Vector3(Mathf.Cos(Mathf.Deg2Rad * i) * currentRadius, Mathf.Sin(Mathf.Deg2Rad * i) * currentRadius);

            GameObject Magic_A = Instantiate(BPrefad, spawnPosition, Quaternion.Euler(0, 0, i));
            Rigidbody2D Mrb = Magic_A.GetComponent<Rigidbody2D>();

            float rd = Mathf.Deg2Rad * i;
            float speed = 5.0f; 

            
            Vector2 velocity = new Vector2(Mathf.Cos(rd), Mathf.Sin(rd)) * speed;

            Mrb.velocity = velocity;
            i += angleIncrement;
        }
    }

    private bool SYD_on = false;
    private void Update()
    {
        if (lockplay)
        {
            IsRunning = false;
            IsMoving = false;
        }
        //UI_Test.Instance.text = rb.velocity.y.ToString() +"   "+　VC2C.Instance.CFT.m_YDamping.ToShortString();
        
        if (!VC2C.Instance.SYD_IE)
        {
            // 墜落距離大於10
            if (rb.velocity.y < -10 && !SYD_on)
            {
                
                VC2C.Instance.SYD(true, 2f, 2f, 0.25f);
                SYD_on = true;
            }
            else if(rb.velocity.y < -10 && SYD_on)
            {
                VC2C.Instance.CFT.m_YDamping = 0.25f;
            }
            else if(touchingDirections.IsGrounded && SYD_on)
            {
                SYD_on = false;
                VC2C.Instance.SYD(false, 2f, 2f, 0.25f);
            }
        }
    }
}


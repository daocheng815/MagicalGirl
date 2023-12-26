using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using Events;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
// ���a�欰����
[RequireComponent(typeof(Detection))]
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public ParticleSystem dast;
    public ParticleSystem Slide_R;
    public ParticleSystem Slide_L;
    
    public PMagic pmagic;
    public float walkSpeed = 5f ;
    public float runspeed = 8f;
    public float airWalkSpeed = 3f ;
    public float airrunkSpeed = 5f;
    public float jumpImpulse = 10f ;
    public float jumpImpulse2 = 11f;
    public static bool lockplay = false;

    private float currentSpeed = 0f;
    public float acceleration = 5.0f; //�[�t��
    public float deceleration = 10.0f; //

    public bool slide_wall;
    public DetectionZone slide_wall_DetectionZone;

    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Detection detection;
    Damageable damageable;

    public Rigidbody2D rb;
    Animator animator;

    //���a�t��
    public float CurrentMoveSpeed{
        get
        {
            if(canMove)
            {
                // ����d��(�ۤϪ� �d�� �i�H��@�@�ӧ���𭱪��ޯ�)
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
                        if (IsRunning)
                        {
                            return airrunkSpeed;
                        }
                        else
                        {
                            return airWalkSpeed;
                        }
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
    [SerializeField]private bool _isMoving = false;
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
    public bool isJump {  get {return animator.GetBool(AnimationStrings.IsJump); }  }
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
    [SerializeField]private bool isSlideAir;
    [SerializeField]private bool _isSlide = false;
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
                VC2C.Instance.CameraOffset(value ? 0.5f:-0.5f,0.3f);
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
    // Start is called before the first frame update
    private void Awake()
    {
        //Transform gameObject = transform.Find("setMenu");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        detection = GetComponent<Detection>();
        damageable = GetComponent<Damageable>();
        
    }
    void Start()
    {

        if (Persistence.GameLoadNum == 0)
        {
            invventoryManger.Instance.deltbag( 0);
            invventoryManger.Instance.deltbag( 1);
        }
        
    }
    private bool is_Slide_up;
    private Vector2 PlayerSlidePosition;
    private Vector2 targetPosition;
    private void FixedUpdate()
    {
        if (!lockplay)
        {
            if (!IsSlide)
            {
                if (!damageable.LockVelocity)
                {
                    //�[�t�׻P��t��
                    bool hasInput = moveInput.x != 0;
                    currentSpeed = Mathf.MoveTowards(currentSpeed, CurrentMoveSpeed, acceleration * Time.deltaTime);
                    if (!hasInput) { currentSpeed = Mathf.MoveTowards(3f, 0f, deceleration * Time.deltaTime); }

                    rb.velocity = new Vector2(moveInput.x * currentSpeed, rb.velocity.y);
                }
            }
            //IsSlide �Ʀ�ʧ@Slide �Ʀ�ʧ@���i������b�a���W�A�P�b�Ť����A�o�̻ݭn���}�ӧP�w
            //���O�y���P�w���~�w�ä��O���S���i�J�� �ӬO MovePosition �����y����BUG
            //else if (!damageable.LockVelocity && touchingDirections.IsGrounded)
            //���b�Ť�isSlideAir
            if (IsSlide && !damageable.LockVelocity)
            {
                if (!touchingDirections.IsOnwall)
                {
                    is_Slide_up = false;
                    Vector2 np = rb.position + new Vector2(IsFacingRight ? 0.4f : -0.4f, moveInput.y);
                    
                    rb.MovePosition(np);
                    //is_Slide_up = false;
                    //float horizontalInput = IsFacingRight ? 1.0f : -1.0f; // ���u�¦V�̩w������V
                    //targetPosition = rb.position + new Vector2(horizontalInput, moveInput.y) * 25f * Time.deltaTime;
                }
                //rb.position = Vector2.Lerp(rb.position, targetPosition, 0.5f);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        animator.SetFloat(AnimationStrings.xVelocity, rb.velocity.x);
        //�p�G���a���`�B�b�a���W�N�N�t�קאּ0
        if (!IsAlive && touchingDirections.IsGrounded)
            rb.velocity = Vector2.zero;
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
                        //Debug.Log("����");
                        Invoke("ReMoveCount", 0.3f);
                        break;
                    case 1:
                        //Debug.Log("�]�B");
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
            CreateDust();
            IsFacingRight = true;
        }else if (moveInput.x <0 && IsFacingRight)
        {
            CreateDust();
            IsFacingRight = false;
        }
    }
    void CreateDust()
    {
        dast.Play();
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
    [SerializeField]private int jumpCount = 0;
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
        CreateDust();
        animator.SetTrigger(AnimationStrings.jumpTrigger);
        rb.velocity = new Vector2(rb.velocity.x, WallClimbIdleCountDelay ? jumpImpulse2 : jumpImpulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (touchingDirections.IsGrounded && !touchingDirections.IsOnwall)
        {
            animator.ResetTrigger(AnimationStrings.jumpTrigger);
            jumpCount = 0; 
        }
    }
//����
    [SerializeField]private bool WallClimbIdleCountDelay = false;
    [SerializeField]private bool WallClimbIdleCount = false;
    public void OnWallClimbIdle(InputAction.CallbackContext context)
    {
        if (!lockplay && detection.isWall)
        {
            if (context.started)
            {
                WallClimbIdleCountDelay = !WallClimbIdleCount;
                WallClimbIdleCount = !WallClimbIdleCount;
                animator.SetBool(AnimationStrings.IsWallClimb,WallClimbIdleCount);
            }
            //else if (context.canceled)
            //{
            //    animator.SetBool(AnimationStrings.IsWallClimb,false);
            //   Debug.Log("��}");
            //}
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!lockplay)
            if (context.started)
            {
                animator.SetTrigger(AnimationStrings.attackTrigger);
            }
        //�I�s��v������_�ʰʵe
        VC2C.Instance.StartCoroutine(VC2C.Instance.CameraSize_Num());
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
            // ���a���h
            rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        VC2C.Instance.StartCoroutine(VC2C.Instance.CameraShock_Num());
        //VC2C.Instance.CameraShock(0.1f, 1f, true, false);
    }
    // �{��
    private bool iss = false;
    public void OnSlide(InputAction.CallbackContext context)
    {
        // ������ ����A���@���ΨӪ����O �P��ܧN�o�ɶ�
        //if (context.started && !iss && touchingDirections.IsGrounded &&IsAlive && !lockplay && pmagic.IsMagic(20))
        if (context.started && !iss && IsAlive && !lockplay && pmagic.IsMagic(20) && !animator.GetBool(AnimationStrings.IsRangeAttack) && !detection.isWall)
        {
            //PlayerSlidePosition = rb.position;
            //Debug.Log(PlayerSlidePosition);
            //�P�w�}�l�Ʀ�ɬO�_�b�Ť�
            isSlideAir = !touchingDirections.IsGrounded;
            //�}�ҰʺA�ҽk
            GlobalVolumeManger.Instance.motionBlurSt(1f);
            //�}�Ҳɤl�ĪG
            CreateDust();
            if(IsFacingRight)
                Slide_R.Play();
            if(!IsFacingRight)
                Slide_L.Play();
            iss = true;
            IsSlide = true;
            pmagic.OnMagic(20);
            StartCoroutine(slide(0.3f));
            //Debug.Log("�{��");
        }
        //��{�覡���Ӧn�A����O�o�令Invoke��k�C
        IEnumerator slide(float time)
        {
            CoolingUI.Instance.CoolingTime(0,0.6f,0.3f);
            yield return new WaitForSeconds(time);
            
            IsSlide = false;
            //Debug.Log("�{�ק���");
            GlobalVolumeManger.Instance.motionBlurSt(0f);
            
            StartCoroutine(FadeSlide(0.3f));
        }
        //�N�o�ɶ�
        IEnumerator FadeSlide(float time)
        {
            yield return new WaitForSeconds(time);
            iss = false;
        }
    }
    // ���
    private bool IsScratch;
    public void OnScratch(InputAction.CallbackContext context)
    {
        if(!lockplay&&pmagic.IsMagic(45)&& false)
            if (context.started && touchingDirections.IsOnwall && IsScratch )
            {   
                if (!(touchingDirections.IsGrounded && touchingDirections.IsOnwall))
                {
                    pmagic.OnMagic(45);
                    Debug.Log("���");
                    rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
                    IsScratch = false;
                }
            }
            else
            {
                IsScratch = true;
            }
    }
    // ��� ����
    // ���a���W��UI
    //�]�k���
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

            // �p�G���}�F�o�Ӥ����N�n����a���ݧ��ɶ��~��A�����}�o��UI
            // ����ե� SK ���o �Y�Ӫ���

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
            
        
        // ��{
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
                        Debug.Log("�T�w�W");
                        Invoke("OnMagic_A", 0.3f);
                        break; 
                    }
                    C = 1;
                    if (C == 0) { Debug.Log("�W"); }
                }
                if (Input.GetKeyDown(KeyCode.S))
                {

                    FAN(2);
                    if (C == 2 && pmagic.IsMagic(55)) { Debug.Log("�T�w�U");
                        pmagic.OnMagic(55);
                        //GameObject projectlie = Instantiate(MagicUI_projectilePrefad, MagicUI_launchPoint.position, MagicUI_projectilePrefad.transform.rotation);
                        Invoke("OnMagic_B", 0.3f);
                        break; }
                    C = 2;
                    if (C == 0) { Debug.Log("�U"); }
                }

                if (_isFacingRight) // ½��
                {
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        FAN(3);
                        if (C == 3) { Debug.Log("�T�w�k");
                            
                            break; }
                        C = 3;
                        if (C == 0) { Debug.Log("�k"); }
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {

                        FAN(4);
                        if (C == 4) { Debug.Log("�T�w��"); break; }
                        C = 4;
                        if (C == 0) { Debug.Log("��"); }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        FAN(4);
                        if (C == 3) { Debug.Log("�T�w�k");
                            
                            break; }
                        C = 3;
                        if (C == 0) { Debug.Log("�k"); }
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {

                        FAN(3);
                        if (C == 4) { Debug.Log("�T�w��"); break; }
                        C = 4;
                        if (C == 0) { Debug.Log("��"); }
                    }
                }

                
                timer += Time.deltaTime;
                yield return null;
            }

            ISMagicUI = false;
            SKObject.SetActive(false);
            lockplay = false;
            Debug.Log("2���");
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
    
    private IEnumerator WallClimbIdleCountDelaytime(float deltime)
    {
        yield return new WaitForSecondsRealtime(deltime);
        WallClimbIdleCountDelay = false;
    }
    private void Update()
    {
        animator.SetBool("lockPlayer",lockplay);
        slide_wall = slide_wall_DetectionZone.detectColliders.Count > 0;
        //�վ�Y���ɭ��O
        rb.gravityScale = !isJump && !touchingDirections.IsGrounded ?  1.2f : 1f;
        if (lockplay)
        {
            IsRunning = false;
            IsMoving = false;
        }
        // ��b����ɪ����NIsSlide���A����
        IsSlide = IsSlide ? !detection.isWall : IsSlide;
        // �ѩ󰱤�ʧ@�A�ҥH�����ɤl�ĪG
        if (!IsSlide && detection.isWall)
        {
            Slide_R.Stop();
            Slide_L.Stop();
        }
        // �p�G���b��W�άO�b�a���N���X���𪺰ʧ@
        if (!detection.isWall)
        {
            animator.SetBool(AnimationStrings.IsWallClimb,false);
            WallClimbIdleCount = false;
        }
        // ���U��a���ɪ����A
        if (detection.isWall && touchingDirections.IsGrounded)
        {
            animator.SetBool(AnimationStrings.IsWallClimb,false);
            WallClimbIdleCount = false;
        }
        if (!detection.isWall && WallClimbIdleCountDelay)
            StartCoroutine(WallClimbIdleCountDelaytime(0.5f));
        if (detection.isWall && touchingDirections.IsGrounded && WallClimbIdleCountDelay)
            StartCoroutine(WallClimbIdleCountDelaytime(0.5f));
        //
        if (WallClimbIdleCount)
        {
            jumpCount = 0;
            rb.velocity = Vector2.zero;
        }
        //UI_Test.Instance.text = rb.velocity.y.ToString() +"   "+�@VC2C.Instance.CFT.m_YDamping.ToShortString();
        if (!VC2C.Instance.SYD_IE)
        {
            // �Y���Z���j��10
            if (rb.velocity.y < -3 && !SYD_on)
            {
                
                VC2C.Instance.SYD(true, 2f, 2f, 0.25f);
                SYD_on = true;
            }
            else if(rb.velocity.y < -3 && SYD_on)
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


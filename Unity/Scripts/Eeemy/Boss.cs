using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
public class Boss : MonoBehaviour
{
    //狀態機
    public enum AnimatorStates
    {
        Idle,
        Attack,
        Dead,
    }
    
    public enum States
    {
        Idle,//等待
        Patrol,//巡邏
        Pursue,//追擊玩家
        Dalay,//停滯
        AttackBroken,
    }
    
    // 參數獲取
    private AnimatorStates _state;
    [SerializeField]private States _myState = States.Idle;
    private States MyState
    {
        get => _myState;
        set
        {
            _myState = value;
            //狀態判斷
            switch (value)
            {
                case States.Idle:
                    Idle();
                    moveDirection = Vector2.zero;
                    break;
                case States.Pursue:
                    Vector3 end = (Vector3)SetAstartTileMap.Instance.RandomNotStopNode(PlayerT, 4, 3);
                    FindPath(transform.position, end);
                    break;
                case States.Dalay:
                    break;
                case States.Patrol:
                    break;
                case States.AttackBroken:
                    Attack_broken();
                    break;
            }
        }
    }
    private Animator _animator;
    private Damageable _damageable;
    private EnemySearchToPlayers _est;
    private Rigidbody2D _rb;
    private bool isAlive => _damageable.IsAlive;
    private Vector3 PlayerT => _est.playerT;
    private float playerToDistance => _est.distance;

    //翻轉
    public enum WalkableDirection { Left, Right }
    private WalkableDirection _walkDirection;
    private Vector2 _walkDirectionVector = Vector2.left;
    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                var localScale = gameObject.transform.localScale;
                localScale = new Vector2(localScale.x * -1, localScale.y);
                gameObject.transform.localScale = localScale;
                if (value == WalkableDirection.Right)
                {
                    _walkDirectionVector = Vector2.left;
                }
                else if (value == WalkableDirection.Left)
                {
                    _walkDirectionVector = Vector2.right;
                }
            }
            _walkDirection = value;
        }
    }
    
    //移動
    public float speed = 3f;
    private Vector2 moveDirection = Vector2.zero;
    //視野
    public float eyeDistanceMax = 12;
    public float eyeDistanceMin = 1;
    //巡路判定時間
    public float pathRunTime = 0.1f;
    //結束巡路距離
    public float endPathDistance = 0.1f;
    //broken
    public GameObject broken;
    private int _brokenDelayTime;
    
    public void Awake()
    {
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _est = GetComponent<EnemySearchToPlayers>();
        _rb = GetComponent<Rigidbody2D>();
        ChangeState(AnimatorStates.Idle);
    }
    /// <summary>
    /// 翻轉方向
    /// </summary>
    private void FilpDirection()
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
    /// <summary>
    /// 轉換動畫狀態機，並且回傳動畫時間
    /// </summary>
    /// <param name="state"></param>
    private float? ChangeState(AnimatorStates state)
    {
        _state = state;
        _animator.Play(_state.ToString());
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName(state.ToString()))
        {
            return stateInfo.normalizedTime * stateInfo.length;
        }
        return null;
    }
    private int _filpTime;
    private int _isMoveNum;
    [SerializeField]private bool isMove;
    private Vector3 _orgTransform;
    public void FixedUpdate()
    {
        _brokenDelayTime++;
        //判定是否正在移動，(rb就算有向量，也有可能卡住)
        if (_isMoveNum == 0)
            _orgTransform = transform.position;    
        _isMoveNum ++;
        if (_isMoveNum > 20)
        {
            isMove = _orgTransform != transform.position;
            _isMoveNum = 0;
        }
        //前進方向等於朝向
        _filpTime ++;
        if (_filpTime > 5)
        {
            if(_rb.velocity.x > 0)
                WalkDirection = WalkableDirection.Left;
            if(_rb.velocity.x < 0)
                WalkDirection = WalkableDirection.Right;
            _filpTime = 0;
        }
        //狀態判斷
        if (MyState != States.Dalay)
        {
            if (playerToDistance < eyeDistanceMax && playerToDistance > eyeDistanceMin)
                MyState = States.Pursue;
            else if (_brokenDelayTime > 100 && playerToDistance < eyeDistanceMin)
            {
                MyState = States.AttackBroken;
                _brokenDelayTime = 0;
            }
            else
                MyState = States.Idle;
        }
    }
    public void Update()
    {
        //移動
        _rb.velocity = moveDirection * speed;
        // 死亡判斷
        if(!isAlive)
            ChangeState(AnimatorStates.Dead);
    }

    private void Attack_broken()
    {
        var r = Random.Range(-4, 4);
        for (int i = -4; i <= 4; i++)
        {
            var y = Random.Range(2, 5);
            var g = Random.Range(4.5f, 6.5f);
            var t = Random.Range(0.3f, 1.3f);
            if(i == r)
                continue;
            GameObject brokene = Instantiate(broken, PlayerT + new Vector3(i, y, 0), quaternion.identity);
            brokene.GetComponent<broken>().BrokenAttack(g,t);
        }
        MyState = States.Idle;
    }
    /// <summary>
    /// 待機狀態邏輯
    /// </summary>
    private void Idle()
    {
        Debug.Log("idle");
        if (_filpTime > 4)
            if (playerToDistance < eyeDistanceMin && !_est.distanceBools)
            {
                FilpDirection();
            }
    }
    /// <summary>
    /// 隨機前往周圍可移動的節點
    /// </summary>
    /// <param name="pos">目前的座標</param>
    /// <param name="zMax">搜尋最大範圍</param>
    /// <param name="zMin">搜尋最小範圍</param>
    private void SinglePath(Vector3 pos ,int zMax = 1 ,int zMin = 0)
    {
        if (!_isSinglePath)
        {
            Vector3? node = SetAstartTileMap.Instance.RandomNotStopNode(pos,zMax,zMin);
            if (node != null)
                StartCoroutine(SinglePath((Vector2)node));
        }
    }
    private bool _isSinglePath;
    private IEnumerator SinglePath(Vector2 node)
    {
        _isSinglePath = true;
        while (Vector2.Distance(gameObject.transform.position, node) > endPathDistance)
        {
            Debug.DrawRay(transform.position,(Vector3)node- transform.position,Color.red);
            moveDirection = (node - (Vector2)transform.position).normalized;
            yield return null;
            if (!isMove)
            {
                SinglePath(transform.position);
                _isSinglePath = false;
                break;
            }
        }
        _isSinglePath = false;
    }
    /// <summary>
    /// 巡路判斷
    /// </summary>
    private void FindPath(Vector3 start, Vector3 end)
    {
        if (!_isPath && !_isSinglePath)
        {
            List<AstartNode> nodes = SetAstartTileMap.Instance.FindPath(start, end);
            if (nodes != null)
            {
                var node = SetAstartTileMap.Instance.ChengerNodesToW(nodes);
                StartCoroutine(Path(node));
            }
            else
            {
                Debug.Log("巡路失敗");
                SinglePath(transform.position);
            }
        }
    }
#if UNITY_EDITOR
    /// <summary>
    /// 顯示巡路
    /// </summary>
    /// <param name="nodes"></param>
    private void TestRaw(List<Vector2> nodes)
    {
        for (int i = 0; i < nodes.Count-1; i++)
        {
            Vector3 direction = nodes[i + 1] - nodes[i];
            Debug.DrawRay(nodes[i],direction,Color.red);
        }
    }
#endif
    private bool _isPath;
    /// <summary>
    /// 從節點尋找路徑，並逐一前進
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    private IEnumerator Path(List<Vector2> nodes)
    {
        _isPath = true;
        int i = 0;
        while (i < nodes.Count -1)
        {
#if UNITY_EDITOR
            if(SetAstartTileMap.Instance.isTestInfo)
                TestRaw(nodes);
#endif
            moveDirection = (nodes[i] - (Vector2)transform.position).normalized;
            yield return null;
            if (Vector2.Distance(gameObject.transform.position, nodes[i]) < endPathDistance)
            {
                i++;
                if(i == nodes.Count -2)
                    break;
            }
            if(!_est.distanceBool && i > 1)
                break;
            if (!isMove)
            {
                SinglePath(transform.position);
                break;
            }
        }
#if UNITY_EDITOR
        if(SetAstartTileMap.Instance.isTestInfo)
            SetAstartTileMap.Instance.DelTextInfo(0.1f);
#endif
        moveDirection = Vector2.zero;
        _isPath = false;
    }
}

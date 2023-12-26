using UnityEngine;
public class 
    Detection : MonoBehaviour
{
    [SerializeField]private bool rayShow = true;

    [SerializeField]private float scaleXOffset = 1f;
    [SerializeField]private float rayLengthRight = 1.0f;
    [SerializeField]private float rayLengthDown = 2.0f;
    [SerializeField]private float VerticalrayLengthDown = 1.0f;
    [SerializeField]private LayerMask wallLayer; 
    
    //取得翻轉方向
    private float scaleX => transform.localScale.x;

    //碰撞射線的向量
    private Vector2 _rayDirectionRight ;
    private Vector2 _rayDirectionDown ;
    //垂直於地面的向量
    private Vector2 _rayDirectionDownVertical;
    //水平向量
    private Vector2 VectorHorizontal => new Vector2(0, 1);
    //2D碰撞
    private RaycastHit2D _hitRight;
    private RaycastHit2D _hitDown;
    
    //狀態
    public bool isSlope;
    public bool isWall;
    
    private void Awake()
    {
        _rayDirectionDown = new Vector2(0, -1f);
    }

    private void FixedUpdate()
    {
        //牆壁
        _rayDirectionRight = new Vector2(scaleX, 0f);
        var position = new Vector2(transform.position.x  ,transform.position.y+ scaleXOffset) ;
        if (rayShow)
            Debug.DrawRay(position, _rayDirectionRight * rayLengthRight, Color.blue);
        _hitRight = Physics2D.Raycast(position, _rayDirectionRight, rayLengthRight, wallLayer);
        if (_hitRight.collider != null)
        {
            //if(rayShow)
                //Debug.Log("碰撞到牆壁！");
        }
        //判定是碰撞到牆壁
        isWall = _hitRight.collider;
        //玩垂直地面的位置
        if (rayShow)
            Debug.DrawRay(position, _rayDirectionDown * rayLengthDown, Color.red);
        _hitDown = Physics2D.Raycast(position, _rayDirectionDown, rayLengthDown, wallLayer);
        //碰撞的話抓取對象物件的法線(normal)向量，也剛好等於對目前的角色底下的斜率。
        if (_hitDown.collider != null)
        {
            _rayDirectionDownVertical = _hitDown.normal;
        }
        var newPosition = new Vector2(transform.position.x , transform.position.y + VerticalrayLengthDown) ;
        if (rayShow)
            Debug.DrawRay(newPosition, _rayDirectionDownVertical * rayLengthDown, Color.yellow);
        //判定是否垂直於地面上
        isSlope = !(_rayDirectionDownVertical == VectorHorizontal);
        
        // 這個判定向量的方式可以利用在許多的地方，例如:斜坡時移動的向量，但目前還沒製作。
    }
    
}


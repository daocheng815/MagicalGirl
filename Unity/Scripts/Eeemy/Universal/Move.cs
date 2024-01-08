
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 移動腳本
/// </summary>
public class Move : MonoBehaviour
{
   //獲取元件
   private Rigidbody2D rb;
   //移動
   private float _speed = 3f;
   /// <summary>
   /// 速度
   /// </summary>
   public float Speed { get { return _speed; } set { _speed = value; } }
   private Vector2 _moveDirection = new Vector2(0,0);
   private float _detectionTime = 0.3f;
   public float DetectionTime
   {
      get => _detectionTime;
      set => _detectionTime = value;
   }
   private float _detectionDelayTime = 0.3f;
   private Vector3 orgPos;
   [SerializeField]private bool stop;
   public bool Stop { get => stop; }

   /// <summary>
   /// 移動方向
   /// </summary>
   public Vector2 MoveDirection { get { return _moveDirection;} set { _moveDirection = value; }}
   //翻轉方向
   public enum WalkableDirection { Left, Right }
   private WalkableDirection _walkDirection;
   public WalkableDirection WalkDirection
   {
      get { return _walkDirection; }
      set
      {
         if (_walkDirection != value)
         {
            transform.localScale = FlipHorizontal(transform.localScale);
         }
         _walkDirection = value;
      }
   }
   public Vector3 FlipHorizontal(Vector3 localScale)
   {
      return new Vector3(-localScale.x, localScale.y, localScale.z);
   }
   [ContextMenu("翻轉")]
   public void FilpDirection()
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

   public void RightToLeft()
   {
      if (WalkDirection == WalkableDirection.Right)
      {
         WalkDirection = WalkableDirection.Left;
      }
   }
   public void LeftToRight()
   {
      if (WalkDirection == WalkableDirection.Left)
      {
         WalkDirection = WalkableDirection.Right;
      }
   }
   //視野
   public float eyeDistanceMax = 12;
   public float eyeDistanceMin = 1;

   public Vector3 GetPos()
   {
      return transform.position;
   }
   private void Awake()
   {
      rb = GetComponent<Rigidbody2D>();
   }
   
   public void Update()
   {
      rb.velocity = Speed * MoveDirection;
      Stoped();
   }

   private void Stoped()
   {
      if(rb.velocity != Vector2.zero)
      {
         if (_detectionDelayTime == 0)
            orgPos = GetPos();
         _detectionDelayTime += Time.deltaTime;
         if (_detectionDelayTime >= _detectionTime)
         {
            stop = orgPos == GetPos();
            _detectionDelayTime = 0;
         }
      }
   }
   public List<Vector2> FindPath(Vector3 end)
   {
      return SetAstartTileMap.Instance.ChengerNodesToW(SetAstartTileMap.Instance.FindPath(transform.position, end));
   }
   
}


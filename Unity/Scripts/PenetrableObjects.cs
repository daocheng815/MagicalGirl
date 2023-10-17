using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//穿越物件
public class PenetrableObjects : MonoBehaviour
{
    TilemapCollider2D col;
    public TouchingDirections touchingDirections;
    public PlayerController playerController;

    public bool UP_isTrigeer;
    public float UP_delayTime = 0.5f;
    public float _UP_delayTime = 0f;

    public bool DO_isTrigeer;
    public float DO_delayTime = 0.5f;
    public float _DO_delayTime = 0f;

    public bool isColliding;
    [SerializeField]
    private bool _isColliding = false;

    private void Awake()
    {
        col = GetComponent<TilemapCollider2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
    
        _isColliding = true;
        foreach (ContactPoint2D contact in collision.contacts)
        {
            
            if (contact.normal.y > 0.5f)
            {
                if(!DO_isTrigeer)
                    UP_isTrigeer = true;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        
        _isColliding = false;
    }
    private void FixedUpdate()
    {
       
        isColliding = _isColliding ? _isColliding : !_isColliding;
        Debug.Log(isColliding);
    }
    void Update()
    {
        play_UP();
        play_DO();
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            DO_isTrigeer = true;
        }
    }
    
    
    void play_UP()
    {
        if (UP_isTrigeer)
        {
            playerController.rb.gravityScale = 0.8f;
            _UP_delayTime += Time.deltaTime;
            col.enabled = false;
            if (_UP_delayTime >= UP_delayTime)
            {
                _UP_delayTime = 0f;
                col.enabled = true;
                UP_isTrigeer = false;
                playerController.rb.gravityScale = 1f;
            }
        }
    }
    void play_DO()
    {
        //條大重力使掉落速度加快
        if (DO_isTrigeer)
        {
            playerController.rb.gravityScale = 3f;
            _DO_delayTime += Time.deltaTime;
            col.enabled = false;
            if (_DO_delayTime >= DO_delayTime)
            {
                _DO_delayTime = 0f;
                col.enabled = true;
                DO_isTrigeer = false;
                playerController.rb.gravityScale = 1f;
            }
        }
        
    }
}

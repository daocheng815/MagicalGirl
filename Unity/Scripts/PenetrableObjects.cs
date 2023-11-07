using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
//¬ï¶Vª«¥ó
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

    [SerializeField]
    private bool _isColliding = false;

    public LayerMask layerMask;

    private void Awake()
    {
        col = GetComponent<TilemapCollider2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if ((layerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f )
                {
                    if(!DO_isTrigeer)
                        UP_isTrigeer = true;
                }
                if (contact.normal.y < 0.5f)
                {
                    _isColliding = true;
                }
            }
        }
        
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if ((layerMask.value & (1 << collision.gameObject.layer)) != 0)
            _isColliding = false;
    }
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && _isColliding)
        {
            DO_isTrigeer = true;
        }
        if (UP_isTrigeer)
            Invoke("play_UP", 0f);
        if (DO_isTrigeer)
            Invoke("play_DO", 0f);
    }
    void play_UP()
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
    void play_DO()
    {
        
        playerController.rb.gravityScale = 2.5f;
        _DO_delayTime += Time.deltaTime;
        col.enabled = false;
        if (_DO_delayTime >= DO_delayTime)
        {
            if (touchingDirections.IsGrounded)
                _DO_delayTime = DO_delayTime;
            _DO_delayTime = 0f;
            col.enabled = true;
            DO_isTrigeer = false;
            playerController.rb.gravityScale = 1f;
        }
    }
}

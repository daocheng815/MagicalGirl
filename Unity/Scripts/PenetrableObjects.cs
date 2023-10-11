using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PenetrableObjects : MonoBehaviour
{
    TilemapCollider2D col;

    public bool UP_isTrigeer;
    public float UP_delayTime = 0.5f;
    public float _UP_delayTime = 0f;

    public bool DO_isTrigeer;
    public float DO_delayTime = 0.5f;
    public float _DO_delayTime = 0f;

    private void Awake()
    {
        col = GetComponent<TilemapCollider2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            
            if (contact.normal.y > 0.5f)
            {
                if(!DO_isTrigeer)
                    UP_isTrigeer = true;
            }
        }
    }
    void Update()
    {
        play_UP();
        play_DO();
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.PageDown))
        {
            DO_isTrigeer = true;
        }
    }
    void play_UP()
    {
        if (UP_isTrigeer)
        {
            _UP_delayTime += Time.deltaTime;
            col.enabled = false;
            if (_UP_delayTime >= UP_delayTime)
            {
                _UP_delayTime = 0f;
                col.enabled = true;
                UP_isTrigeer = false;
            }
        }
    }
    void play_DO()
    {
        if (DO_isTrigeer)
        {
            _DO_delayTime += Time.deltaTime;
            col.enabled = false;
            if (_DO_delayTime >= DO_delayTime)
            {
                _DO_delayTime = 0f;
                col.enabled = true;
                DO_isTrigeer = false;
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoad : MonoBehaviour
{
    private Transform _player;
    private Vector2 sp => transform.position;

    [SerializeField]private Vector2 vet;
    void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    Vector2 R (Vector2? e = null)
    {
        var ep = e ?? _player.position;
        var vt = ep - sp;
        vt = new Vector2(Mathf.Sign(vt.x), Mathf.Sign(vt.y));
        
        Debug.DrawRay(sp, vt, Color.red);
        RaycastHit2D hitDown = Physics2D.Raycast(sp, vt, 1, 8);
        bool isNull = hitDown;
        Debug.Log(isNull);
        return vt;
    }
    void Update()
    {
        vet = R();
    }
}

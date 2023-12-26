using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemySearchToPlayers : MonoBehaviour
{
    [SerializeField]private GameObject Player;
    [SerializeField]private bool isOnPlayer;
    [SerializeField]private bool enableDrawRay; 
    public Vector3 PlayerWorldLocation => Player.transform.position;
    public float distance;
    public Vector3 direction;

    public bool distanceBool = true;
    public bool distanceBools = true;
    
    public bool pl;
    public bool ml;
    public bool c;
    public Vector3 playerT => Player.transform.position;
    public Vector3 playerL => Player.transform.localScale;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //獲取標籤為玩家的物件
        Player = GameObject.FindWithTag("Player");
        isOnPlayer = Player != null ?  true : false;
    }
    private List<bool> bList = new List<bool>();
    private int bi = 0;
    private void FixedUpdate()
    {
        c = playerT.x - transform.position.x >= 0;
        pl = playerL.x >= 1;
        ml = transform.localScale.x >= 1 ;

        distanceBools = (ml && pl && c) || (!ml && !pl && !c) || (!ml && pl && !c) || (ml && !pl && c);
        
        bList.Add(distanceBools);

        if (bList.Count >= 20)
        {
            foreach (var a in bList)
            {
                if (a)
                    bi++;
            }
            distanceBool = bi >= bList.Count / 2;
            bi = 0;
            bList.Clear();
        }
        
        distance = Vector3.Distance(PlayerWorldLocation, transform.position);
        direction = (PlayerWorldLocation - transform.position).normalized;
        if(enableDrawRay)
            Debug.DrawRay(transform.position,PlayerWorldLocation - transform.position, Color.red);
    }
}

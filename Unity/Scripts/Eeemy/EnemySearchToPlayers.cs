using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemySearchToPlayers : MonoBehaviour
{
    [SerializeField]private GameObject Player;
    [SerializeField]private bool isOnPlayer;
    [SerializeField]private bool enableDrawRay; 
    public Vector3 PlayerWorldLocation => Player.transform.position;
    public float distance;
    public Vector3 direction;
    private void Awake()
    {
        //獲取標籤為玩家的物件
        Player = GameObject.FindWithTag("Player");
        isOnPlayer = Player != null ?  true : false;
    }

    private void FixedUpdate()
    {
        distance = Vector3.Distance(PlayerWorldLocation, transform.position);
        direction = (PlayerWorldLocation - transform.position).normalized;
        if(enableDrawRay)
            Debug.DrawRay(transform.position,PlayerWorldLocation - transform.position, Color.red);
    }
}

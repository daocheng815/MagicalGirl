using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour
{

    Rigidbody2D rb;
    public GameObject play;
    public PlayerController playerController;

    public List<Transform> moveposition;
    public bool ison;
    public bool isonTruee;
    public bool isplayon;
    public float objSpeed = 1f;
    public float waypointReachedDistance = 0.1f;
    private float playtPosition_x;
    private float playtPosition_y;
    public LayerMask LayerMask;

    Transform nextWaypoint;
    int waypointNum = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextWaypoint = moveposition[waypointNum];
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((LayerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("觸發目標:"+ collision.gameObject.layer);
            playtPosition_x = playerController.transform.position.x - transform.position.x;
            isonTruee = true; 
            ison = true;
        }
       
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((LayerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            isonTruee = false;
            ison = false;
        }
        
    }
    void Update()
    {
        playmoveObj();
        moveobj();
    }
    private void moveobj()
    {
        Vector2 playtPosition = (nextWaypoint.position - gameObject.transform.position).normalized;

        float distance = Vector2.Distance(nextWaypoint.position, transform.position);
        //Debug.Log(playtPosition + "   " + distance);
        rb.velocity = playtPosition * objSpeed;
        if (distance <= waypointReachedDistance)
        {
            waypointNum++;
            if (waypointNum >= moveposition.Count)
            {

                waypointNum = 0;
            }
            nextWaypoint = moveposition[waypointNum];
        }
    }
    void playmoveObj()
    {
        //持續修改玩家位置
        if (ison && isonTruee)
        {
            play.transform.position = new Vector3(gameObject.transform.position.x + playtPosition_x, play.transform.position.y, play.transform.position.z);
            if (playerController.isJump || playerController.IsMoving)
                ison = false;
        }
        else
        {
            if (!playerController.isJump && !playerController.IsMoving)
            {
                playtPosition_x = playerController.transform.position.x - transform.position.x;
                ison = true;
                //Debug.Log("重設位置");
            }
        }
    }
}

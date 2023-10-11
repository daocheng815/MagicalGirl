using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public int damage = 10;
    public Vector2 moveSpeed = new Vector2(3f,0);
    public Vector2 knockback = new Vector2 (0,0);
    public float maxDistance = 10f;


    Rigidbody2D rb;
    private Vector2 initialPosition;

    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }


    void Start()
    {
        rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
            
    }

    private void Update()
    {
        
        float distance = Vector2.Distance(initialPosition, transform.position);

        
        if (distance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null)
        {
            //擊退時翻轉方向(x and -x)
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

            //hit the traget
            bool gotHit = damageable.Hit(damage, deliveredKnockback,false);

            if (gotHit) { 
               
                Debug.Log(collision.name + "hit for" + damage);
                Destroy(gameObject);
            }
        }
    }
}

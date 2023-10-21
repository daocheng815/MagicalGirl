using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public int damage = 10;
    public Vector2 moveSpeed = new Vector2(3f,0);
    public Vector2 knockback = new Vector2 (0,0);
    public float maxDistance = 10f;
    private int attackNum = 0;

    public string targetLayerName = "Ground";
    public ParticleSystem Explode;
    public ParticleSystem Explode2;
    public ParticleSystem bullet;
    public ParticleSystem Trajectory;

    SpriteRenderer sr;
    Rigidbody2D rb;
    private Vector2 initialPosition;

    // Start is called before the first frame update

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
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
            startParticle();
        }
    }
    private void startParticle()
    {
        attackNum += 1;
        bullet.Stop();
        Trajectory.Stop();
        Explode2.Play();
        Explode.Play();
        sr.color = new Vector4(1, 1, 1, 0);
        if (!Explode.isPlaying)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.transform.name);
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            if (attackNum == 0)
            {
                attackNum += 1;
                startParticle();
            }
        }
        if (collision.transform.gameObject.transform.tag == "Enemy" && attackNum == 0)
        {
            attackNum += 1;
            Damageable damageable = collision.GetComponent<Damageable>();
            if (damageable != null)
            {
                //擊退時翻轉方向(x and -x)
                Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);

                //hit the traget
                bool gotHit = damageable.Hit(damage, deliveredKnockback, false);

                if (gotHit)
                {
                    Explode.Play();
                    Debug.Log(collision.name + "hit for" + damage);
                    //呼叫攝影機控制的震動動畫
                    VC2C.Instance.StartCoroutine(VC2C.Instance.CameraSize_Num());
                    startParticle();
                }
            }
        }
        
    }
}

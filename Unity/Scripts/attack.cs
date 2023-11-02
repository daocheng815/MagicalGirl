using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    public bool MY = false;
    public bool You = false;
    public int attackDamage = 10;
    public int minFloat = 0;
    public int maxFloat = 0;
    public float critChance = 0f;
    public int critDamage = 0;
    private int orgdamage;
    private bool IsCrit = false;
    public Vector2 knockback = Vector2.zero;
    
    //  傷害方式就是傷害本身是個碰撞體，當傷害本身碰撞到帶有damageable腳本的物體時會去使用該物體上的hit，並且將傷害腳本的值傳遞給被碰撞物體的damageable上。
    
    private void OnParticleCollision(GameObject other)
    {
        orgdamage = attackDamage;

        int damageFloat = Random.Range(minFloat, maxFloat);
        attackDamage += damageFloat;

        float randomProbability = Random.Range(0f, 1f);
        float critChanceProbability = critChance / 100f;
        if (randomProbability <= critChanceProbability)
        {
            attackDamage = attackDamage * ((critDamage + 100) / 100);
            Debug.Log("爆擊");
            IsCrit = true;
        }
        else
        {
            IsCrit = false;
            Debug.Log("普通攻擊");
        }

        Damageable damageable = other.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback;
            //擊退時翻轉方向(x and -x)
            if (You)
            {
                //判定碰撞到的物件的朝向
                deliveredKnockback = other.transform.gameObject.transform.localScale.x < 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            }
            else if(MY)
            {
                deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            }
            else
            {
                deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            }
            //hit the traget
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback, IsCrit);
            if (gotHit) { Debug.Log(other.name + "hit for" + attackDamage); }


        }
        attackDamage = orgdamage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        orgdamage = attackDamage;

        int damageFloat = Random.Range(minFloat, maxFloat);
        attackDamage += damageFloat;

        float randomProbability = Random.Range(0f, 1f);
        float critChanceProbability = critChance / 100f;
        if (randomProbability <= critChanceProbability)
        {
            attackDamage = attackDamage * ((critDamage + 100) / 100); 
            //Debug.Log("爆擊");
            IsCrit = true;
        }
        else
        {
            IsCrit = false;
            //Debug.Log("普通攻擊");
        }

        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback;
            //擊退時翻轉方向(x and -x)
            if (MY)
            {
                deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y); 
            }
            else
            { 
                deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y); 
            }


            //hit the traget
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback, IsCrit);
            if (gotHit) { Debug.Log(collision.name + "hit for" + attackDamage); }

            
        }
        attackDamage = orgdamage;
    }
}

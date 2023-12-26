using Events;
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
    
    public float critMiss = 0f;
    private bool IsMiss = false;
    
    public Vector2 knockback = Vector2.zero;
    
    //  �ˮ`�覡�N�O�ˮ`�����O�ӸI����A��ˮ`�����I����a��damageable�}��������ɷ|�h�ϥθӪ���W��hit�A�åB�N�ˮ`�}�����ȶǻ����Q�I�����骺damageable�W�C
    
    private void OnParticleCollision(GameObject other)
    {
        orgdamage = attackDamage;

        int damageFloat = Random.Range(minFloat, maxFloat);
        attackDamage += damageFloat;

        float randomCritMiss = Random.Range(0f, 100f);
        IsMiss = randomCritMiss <= critMiss;
        
        
        float randomProbability = Random.Range(0f, 1f);
        float critChanceProbability = critChance / 100f;
        if (randomProbability <= critChanceProbability)
        {
            attackDamage = attackDamage * ((critDamage + 100) / 100);
            Debug.Log("�z��");
            IsCrit = true;
        }
        else
        {
            IsCrit = false;
            Debug.Log("���q����");
        }

        Damageable damageable = other.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback;
            //���h��½���V(x and -x)
            if (You)
            {
                //�P�w�I���쪺���󪺴¦V
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

        float randomCritMiss = Random.Range(0f, 100f);
        IsMiss = randomCritMiss <= critMiss;
        
        float randomProbability = Random.Range(0f, 1f);
        float critChanceProbability = critChance / 100f;
        if (randomProbability <= critChanceProbability)
        {
            attackDamage = attackDamage * ((critDamage + 100) / 100); 
            //Debug.Log("�z��");
            IsCrit = true;
        }
        else
        {
            IsCrit = false;
            //Debug.Log("���q����");
        }

        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 deliveredKnockback;
            //���h��½���V(x and -x)
            if (MY)
            {
                deliveredKnockback = transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y); 
            }
            else
            { 
                deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y); 
            }

            if (!IsMiss)
            {
                //hit the traget
                bool gotHit = damageable.Hit(attackDamage, deliveredKnockback, IsCrit);
                if (gotHit) { Debug.Log(collision.name + "hit for" + attackDamage); }
            }
            else
            {
                Debug.Log("MISS");
                CharacterEvents.characterTextW.Invoke(collision.gameObject, "miss");
            }

            if (IsCrit && !IsMiss)
            {
                CharacterEvents.characterCrit.Invoke(collision.gameObject, "�z��");
            }

            if (!IsMiss)
            {
                //���V��
                int shuDyeingVar = 0;
                if (attackDamage < 50)
                {
                    shuDyeingVar = 1;
                }
                else if (attackDamage <= 80)
                {
                    shuDyeingVar = 2;
                }
                else if (attackDamage <= 120)
                {
                    shuDyeingVar = 3;
                }
                else if (attackDamage <= 150)
                {
                    shuDyeingVar = 4;
                }
                else
                {
                    shuDyeingVar = 5;
                }

                if (gameObject.layer != 11)
                {
                    if (!IsCrit)
                    {
                        PlayerVar.Instance.shuDyeingVar.FilthyAdd(shuDyeingVar);
                        CharacterEvents.characterShuDyeing.Invoke(collision.gameObject, "���V");
                    }
                    else
                    {
                        PlayerVar.Instance.shuDyeingVar.FilthyAdd(shuDyeingVar*2);
                        CharacterEvents.characterShuDyeing.Invoke(collision.gameObject, "�Y�����V");
                    }
                }
                
            }

        }
        attackDamage = orgdamage;
    }
}

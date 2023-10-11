using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damagableHit;

    Animator animator;

    [SerializeField]
    private int _MaxHealth;
    public int MaxHealth 
    {   get 
        {
            return _MaxHealth;
        } 
        private set {
            _MaxHealth = value;
        } 
    }
    [SerializeField]
    private int _health = 100;

    public int health
    {
        get { return _health; }
        set 
        {
            _health = value; 
            if (health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvincible = false;

   

    private float timeSinceHit = 0f;
    public float isInvinciblutyTime = 0.25f;

    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive =value;
            animator.SetBool(AnimationStrings.IsAlive, value);
            Debug.Log("IsAlive set:" + value);

        }
    }

    // The velocity shoild not be changed while this is true but needs to be respected by other physics components like
    // the player controller
    // �b�Y�ر��p�U�A���骺�t�פ����ӧ��ܡA����L�P���z�������ե�]�p���a����^���M�ݭn�Ҽ{�o�ӳt�סC���y�ܻ��A���骺�t�צb�Y�ӯS�w���p�U���ӫO�����ܡA����L���z�欰���Ӯھڳo�ӳt�רӶi��p��M�T���C�o�i��A�Ψ�T�άY�Ǫ��z�欰�ίS��B�z�ӫO���t�פ��ܡC


    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (isInvincible)
        {
            if(timeSinceHit > isInvinciblutyTime)
            {
                isInvincible=false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }

        
        if (gameObject.tag == "Player")
        {
            HealthBarW.HeaithMax = MaxHealth;
            HealthBarW.HeaithCurrent = health;
            HealthBar.HeaithMax = MaxHealth;
            HealthBar.HeaithCurrent = health;
        }

    }

    public  bool Hit(int damage,Vector2 knockback,bool crit)
    {
        if(IsAlive && !isInvincible)
        {
            health -= damage;
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true; 
            damagableHit?.Invoke(damage, knockback);
            if (crit)
            { CharacterEvents.characterCritDamaged.Invoke(gameObject, damage); Debug.Log("����"); }
            else
            { CharacterEvents.characterDamaged.Invoke(gameObject, damage); }


            return true;      
        }
        return false;
    }
    public bool Heal(int healthRestore) 
    {
        if(IsAlive && health <MaxHealth)
        {
            
            int maxHeal = Mathf.Max(MaxHealth - health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            health += actualHeal;
            CharacterEvents.characterHealed.Invoke(gameObject, actualHeal);
            return true;
        }
        return false;
    }

   
}

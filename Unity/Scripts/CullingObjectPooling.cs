using System;
using System.Collections.Generic;
using Eeemy;
using UnityEngine;

/// <summary>
/// 優化遊戲
/// </summary>
public class CullingObjectPooling : MonoBehaviour
{
    [SerializeField]private List<GameObject> enemyObjectPooling = new List<GameObject>();

    private List<Type> _typeList = new List<Type>() 
        {typeof(Transform), typeof(SpriteRenderer), typeof(Animator), typeof(Rigidbody2D), typeof(CapsuleCollider2D)};

    public List<Type> NotTypeList = new List<Type>() {typeof(Sairesuu_boss_plot)};
    private const string EnemyTag = "Enemy";
    
    private BoxCollider2D boxCollider;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(EnemyTag);
        if (enemies.Length > 0)
        {
            foreach (var g in enemies)
            {
                enemyObjectPooling.Add(g);
                AC(g, false);
            }
        }
        boxCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(EnemyTag))
        {
            if (!enemyObjectPooling.Contains(other.gameObject))
            {
                bool s = false;
                var ac = other.GetComponents(typeof(Component));
                foreach (var c in ac)
                {
                    var type = c.GetType();
                    if (!NotTypeList.Contains(type))
                        s = true;
                }
                if(!s)
                    enemyObjectPooling.Add(other.gameObject);
            }
            else
            {
                AC(other.gameObject,true);
            }
        }
    }

    public void AddPool(GameObject other)
    {
        if (other.gameObject.CompareTag(EnemyTag))
        {
            if (!enemyObjectPooling.Contains(other.gameObject))
            {
                enemyObjectPooling.Add(other.gameObject);
                AC(other.gameObject,false);
            }
            else
            {
                AC(other.gameObject,false);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(EnemyTag))
        {
            if (enemyObjectPooling.Contains(other.gameObject))
            {
                AC(other.gameObject,false);
            }
        }
    }

    private void AC(GameObject g,bool setActive)
    {
        var ac = g.GetComponents(typeof(Component));
        foreach (var c in ac)
        {
            var type = c.GetType();
            if (!_typeList.Contains(type))
                ((Behaviour)c).enabled = setActive;
        }
    }
}
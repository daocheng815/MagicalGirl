using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 優化遊戲
/// </summary>
public class CullingObjectPooling : MonoBehaviour
{
    [SerializeField]private List<GameObject> enemyObjectPooling = new List<GameObject>();

    private List<Type> _typeList = new List<Type>() 
        {typeof(Transform), typeof(SpriteRenderer), typeof(Animator), typeof(Rigidbody2D), typeof(CapsuleCollider2D)};

    private BoxCollider2D boxCollider;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var g in enemies)
        {
            enemyObjectPooling.Add(g);
            AC(g, false);
        }
        boxCollider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!enemyObjectPooling.Contains(other.gameObject))
            {
                enemyObjectPooling.Add(other.gameObject);
            }
            else
            {
                AC(other.gameObject,true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
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
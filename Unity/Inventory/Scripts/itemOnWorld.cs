using System.Collections;
using DG.Tweening;
using Events;
using UnityEngine;

public class itemOnWorld : MonoBehaviour
{
    public item thisItem;
    [Range(1,100)]
    public int itemAddNum = 1;
    public CircleCollider2D col;
    private SpriteRenderer sr;
    private bool hasTriggered = false;
    private bool isPlayerInside = false;
    public float delayTime = 0.3f;
    public float FadeTime = 0.2f;
    public float destroyTime = 0.2f;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !hasTriggered)
        {
            isPlayerInside = true;
            StartCoroutine(WaitAndTrigger(delayTime));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
    private IEnumerator WaitAndTrigger(float time)
    {
        float timer = 0f;
        while (timer < time)
        {
            if(CheckCollisionWithGround())
                timer += Time.deltaTime;
            if (!isPlayerInside)
            {
                break;
            }
            yield return null;
        }
        if (isPlayerInside)
        {
            hasTriggered = true;
            //AddNewItem();
            if(invventoryManger.Instance.AddNewItem(thisItem.itemID, 0,itemAddNum))
                sr.DOFade(0f, FadeTime).OnComplete(() => Destroy(gameObject, destroyTime));
        }
    }

    private bool CheckCollisionWithGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(col.transform.position, col.radius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                return true; // 如果碰撞到地面，返回true
            }
            if (collider.gameObject.layer == LayerMask.NameToLayer("item"))
            {
                return true; // 如果碰撞到地面，返回true
            }
        }

        return false; // 如果?有碰撞到地面，返回false
    }
}

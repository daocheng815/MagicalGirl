using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Events;
public class itemOnWorld : MonoBehaviour
{
    public item thisItem;
    public Inventory[] playinventory;
    private SpriteRenderer sr;
    private bool hasTriggered = false;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            AddNewItem();
            sr.DOFade(0f, 0.3f).OnComplete((() => Destroy(gameObject,0.3f)));
        }
    }
    public void AddNewItem()
    {
        //檢查背包陣列中是否有這個物品了
        bool isItem = false;
        int isitem = 0;
        for (int i = 0; i < playinventory.Length; i++)
        {
            isItem = playinventory[i].itemList.Contains(thisItem);
            if (isItem)
                isitem = i;
        }
        //如果背包陣列中有
        if (isItem)
        {
            thisItem.itemHeld += 1;
        }
        else
        {
            if (!playinventory[0].itemList.Contains(thisItem))
            {
                //判定在背包的18格中是否有空位
                for (int i = 0; i < playinventory[0].itemList.Count;i++)
                {
                    if (playinventory[0].itemList[i] == null)
                    {
                        playinventory[0].itemList[i] = thisItem;
                        break;
                    }
                }
            }
            else
            {
                thisItem.itemHeld += 1;
            }
        }
        //更新背包容易卡頓，替換成如果某個間距時間內只會更新一次。
        //invventoryManger.Instance.RefreshItem(1);
        //invventoryManger.Instance.RefreshItem(0);
        //紀錄獲取物品
        ItemDropsEvents.DropsSuccess();
    }
}

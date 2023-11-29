using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
public class ItemList : Singleton<ItemList>
{
    public item[] Nitem;
    public List<item> Item = new List<item>();
    
    //重製狀態很重要，這會影響到接下來要使用ItemList的事件觸發。
    private void OnEnable()
    {
        //重製狀態
        itemUpDate.itemListUpDate = false;
    }

    private void Start()
    {
        //Debug.Log(itemUpDate.itemListUpDate);
        foreach (item item in Nitem)
        {
            Item.Add(item);
        }
        // 創建IID
        for (int i = 0 ; i < Item.Count ; i++)
        {
            Item[i].itemID = i;
        }
        itemUpDate.itemListUpDate = true;
    }
}

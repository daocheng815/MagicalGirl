using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
public class ItemList : Singleton<ItemList>
{
    public item[] Nitem;
    public List<item> Item = new List<item>();
    
    //���s���A�ܭ��n�A�o�|�v�T�챵�U�ӭn�ϥ�ItemList���ƥ�Ĳ�o�C
    private void OnEnable()
    {
        //���s���A
        itemUpDate.itemListUpDate = false;
    }

    private void Start()
    {
        //Debug.Log(itemUpDate.itemListUpDate);
        foreach (item item in Nitem)
        {
            Item.Add(item);
        }
        // �Ы�IID
        for (int i = 0 ; i < Item.Count ; i++)
        {
            Item[i].itemID = i;
        }
        itemUpDate.itemListUpDate = true;
    }
}

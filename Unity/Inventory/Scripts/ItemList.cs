using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : Singleton<ItemList>
{
    public item[] Nitem;
    public List<item> Item = new List<item>();

    private void Start()
    {
        foreach (item item in Nitem)
        {
            Item.Add(item);
        }
        // ³Ð«ØIID
        for (int i = 0 ; i < Item.Count ; i++)
        {
            Item[i].itemID = i;
        }
    }
}

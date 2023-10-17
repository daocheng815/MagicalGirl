using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : Singleton<ItemList>
{
    [SerializeField]
    private item[] Nitem;
    public List<item> Item = new List<item>();

    private void Start()
    {
        foreach (item item in Nitem)
        {
            Item.Add(item);
        }
    }
}

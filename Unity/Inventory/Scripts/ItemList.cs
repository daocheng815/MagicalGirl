using System.Collections.Generic;
using Events;
using UnityEngine;

public class ItemList : Singleton<ItemList>
{
    public item[] OrgItem;
    public List<item> Item = new List<item>();
    
    //���s���A�ܭ��n�A�o�|�v�T�챵�U�ӭn�ϥ�ItemList���ƥ�Ĳ�o�C
    private void OnEnable()
    {
        ItemUpDate.ItemListUpDate = false;
        Debug.Log("�}�l��s");
        foreach (var i in OrgItem)
        {
            Item.Add(i);
        }
        
        // �Ы�IID
        for (int i = 0 ; i < Item.Count ; i++)
        {
            Item[i].itemID = i;
        }
        Debug.Log("������s");
        ItemUpDate.ItemListUpDate = true;
    }
}

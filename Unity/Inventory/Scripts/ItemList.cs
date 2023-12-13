using System.Collections.Generic;
using Events;

public class ItemList : Singleton<ItemList>
{
    public item[] OrgItem;
    public List<item> Item = new List<item>();
    
    //重製狀態很重要，這會影響到接下來要使用ItemList的事件觸發。
    private void OnEnable()
    {
        ItemUpDate.ItemListUpDate = false;
        foreach (var i in OrgItem)
        {
            Item.Add(i);
        }
        
        // 創建IID
        for (int i = 0 ; i < Item.Count ; i++)
        {
            Item[i].itemID = i;
        }
        ItemUpDate.ItemListUpDate = true;
    }
}

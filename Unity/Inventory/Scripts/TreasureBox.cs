using Events;
using SaveLord;
using UnityEngine;

public class TreasureBox : MonoBehaviour,ILordInterface
{
    [Header("箱子編號")]
    public int boxNum;
    [Header("物品ID")]
    public int itemID;
    [Header("x,y為掉落初始位置偏移(Vector2)， z值是掉落延遲時間。")]
    public Vector3 v3;

    private bool box
    {
        get => EventRecordManger.Instance.GetBoolVal("box_"+boxNum,true);
        set => EventRecordManger.Instance.SetBoolVal("box_"+boxNum, value);
    }
    
    public int itemNum = 1;
    public bool isAll = true;
    public bool des = true;
    public void OnBox()
    {
        if(isAll)
            ItemEvents.ItemDropsTheWorld.Invoke(transform.position,new Vector2(100,itemNum),itemID,v3);
        else
        {
            for (int i = 0; i < itemNum; i++)
            {
                ItemEvents.ItemDropsTheWorld.Invoke(transform.position,new Vector2(100,1),itemID,v3);
            }
        }

        if (des)
        {
            box = false;
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        if (!box)
        {
            Destroy(gameObject);
        }
    }
}

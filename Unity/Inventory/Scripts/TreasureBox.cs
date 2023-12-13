using Events;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    [Header("物品ID")]
    public int itemID;
    [Header("x,y為掉落初始位置偏移(Vector2)， z值是掉落延遲時間。")]
    public Vector3 v3;
    public void OnBox()
    {
        ItemEvents.ItemDropsWorld.Invoke(transform.position,new Vector2(100,1),itemID,v3);
    }
}

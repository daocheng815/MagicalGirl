using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    public abstract class ItemEvents
    {
        /// <summary>
        /// Vector3 : 掉落對象GameObject的位置。
        /// Vector2 :  x掉落機率，0~100，y獲取物品數量。
        /// int : 掉落物品ID。
        /// Vector3 : x,y為掉落初始位置偏移(Vector2)， z值是掉落延遲時間。
        /// </summary>
        public static UnityAction<Vector3, Vector2, int, Vector3> ItemDropsTheWorld;
        public static UnityAction<int,item> ItemDropsTheSuccess;
    }
    public abstract class ItemUpDate
    {
        public static bool ItemListUpDate = false;
        public static UnityAction BagUpData;
    }
    public abstract class BagFuncMenu
    {
        public static UnityAction<int, item, Inventory ,Vector2> ItemOnClicked;
        public static UnityAction ItemOnAction;
        public static UnityAction<bool> IsItemOnDrag;
    }
}

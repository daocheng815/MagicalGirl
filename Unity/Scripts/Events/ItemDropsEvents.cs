using UnityEngine;
using UnityEngine.Events;
using System.Collections;
namespace Events
{
    public class ItemDropsEvents
    {
        /// <summary>
        /// Vector3 : 掉落對象GameObject的位置。
        /// float : 掉落機率，0~100。
        /// int : 掉落物品ID。
        /// Vector3 : x,y為掉落初始位置偏移(Vector2)， z值是掉落延遲時間。
        /// </summary>
        public static UnityAction<Vector3, float, int, Vector3> itemDropsWorld;
        public static UnityAction DropsSuccess;
    }

    public class itemUpDate
    {
        public static bool itemListUpDate = false;
    }
}

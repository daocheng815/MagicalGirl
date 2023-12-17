using UnityEngine;
using UnityEngine.Events;
namespace Events
{
    public abstract class BuffEvents
    {
        /// <summary>
        /// BUFF動畫開始
        /// </summary>
        public static UnityAction<GameObject, int> AddBuff;
        /// <summary>
        /// 創建玩家BUFF UI
        /// </summary>
        public static UnityAction<Buff,GameObject> PlayerCreateBuffPrefab;
        /// <summary>
        /// 移除BUFF
        /// </summary>
        public static UnityAction<GameObject, int> DelBuff;

        public static UnityAction<string,Vector2> BuffUIFloatingWindowOn;
        public static UnityAction BuffUIFloatingWindowOff;
    }
}
using System;
using SaveLord;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayAction
{
    public class GetItemDrops : MonoBehaviour,IPlayAction,ILordInterface
    {
        [Header("物品ID")]
        public int itemID = 8;
        [Header("x,y為掉落初始位置偏移(Vector2)， z值是掉落延遲時間。")]
        public Vector3 v3;

        public int itemNum = 1;
        private bool _isGet;
        private bool isGet
        {
            get => EventRecordManger.Instance.GetBoolVal(isGetName,false);
            set => EventRecordManger.Instance.SetBoolVal(isGetName, value);
        }
        
        [SerializeField] private string isGetName;
        //取得當前interFace
        private INpcBubbleTeble[] _iNpcBubble;

        private void Awake() { _iNpcBubble = gameObject.GetComponents<INpcBubbleTeble>(); }

        public void Init()
        {
            if(isGet)
                foreach (INpcBubbleTeble I in _iNpcBubble)
                {
                    I.IsBubbleEnable = false;
                }
        }
        public bool LockAction { get; set; }
        public int ActionCount { get; set; }
        public void Action()
        {
            if (!isGet)
            {
                foreach (INpcBubbleTeble I in _iNpcBubble)
                {
                    I.IsBubbleEnable = false;
                }
                isGet = true;
                Debug.Log("獲取物品");
                Events.ItemEvents.ItemDropsTheWorld(transform.position, new Vector2(100, itemNum), itemID, v3);
            }
        }
    }
}
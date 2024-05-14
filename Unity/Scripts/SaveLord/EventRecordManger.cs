using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveLord
{
    /// <summary>
    /// 全局資料管理
    /// </summary>
    public class EventRecordManger : CSingleton<EventRecordManger>
    {
        public bool EventRecordEnable = false;
        
        public Dictionary<string, bool> TriggerStateBool
        {
            get => _triggerStateBool;
            set => _triggerStateBool = value;
        }

        public Dictionary<string, int> TriggerStateINT
        {
            get => _triggerStateINT;
            set => _triggerStateINT = value;
        }
        
        //某些事情的觸發狀態，以布林儲存
        private Dictionary<string, bool> _triggerStateBool = new Dictionary<string, bool>();
        
        //某些事情的觸發狀態，以數字儲存
        private Dictionary<string,int> _triggerStateINT = new Dictionary<string,int>();

        public void ReSet()
        {
            Debug.Log("更新");
            TriggerStateBool = new Dictionary<string, bool>();
            TriggerStateINT = new Dictionary<string, int>();
        }
        /// <summary>
        /// 嘗試獲取trigger_state_bool字典的布林值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="initBool"></param>
        /// <returns></returns>
        public bool GetBoolVal(string key , bool initBool)
        {
            if (_triggerStateBool.TryGetValue(key, out var storedBool))
            {
                // 如果鍵存在，則返回存儲的布林值
                return storedBool;
            }
            // 如果鍵不存在，將initBool加入字典並返回initBool
            _triggerStateBool.Add(key,initBool);
            return initBool;
        }
        
        /// <summary>
        /// 設定trigger_state_bool字典的布林值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newBool"></param>
        /// <returns></returns>
        public void SetBoolVal(string key, bool newBool)
        {
            _triggerStateBool[key] = newBool;
        }
        
        /// <summary>
        /// 嘗試獲取trigger_state_bool字典的布林值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="initBool"></param>
        /// <returns></returns>
        public int GetIntVal(string key , int initBool)
        {
            if (_triggerStateINT.TryGetValue(key, out var storedBool))
            {
                // 如果鍵存在，則返回存儲的布林值
                return storedBool;
            }
            // 如果鍵不存在，將initBool加入字典並返回initBool
            _triggerStateINT.Add(key,initBool);
            return initBool;
        }
        
        /// <summary>
        /// 設定trigger_state_bool字典的布林值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newBool"></param>
        /// <returns></returns>
        public void SetIntVal(string key, int newBool)
        {
            _triggerStateINT[key] = newBool;
        }
    }
}
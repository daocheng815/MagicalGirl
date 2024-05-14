using System.Collections.Generic;
using currentLevel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace SaveLord
{
    [System.Serializable]
    public class KeyBoolValue
    {
        public string key;
        public bool value;
    }
    [System.Serializable]
    public class KeyIntValue
    {
        public string key;
        public int value;
    }
    [System.Serializable]
    public class KeyVector2Value
    {
        public string key;
        public Vector2 value;
    }
    [System.Serializable]
    public class PlayerData
    {
        public string saveIsTime;
        public LevelState.LevelStateEnum myLevelState;
        public int magicVar;
        public int healthVar;
        public float playerTransformX;
        public float playerTransformY;
        public List<int> mybag_item_ID = new List<int>();
        public List<int> mybag_item_itemHeld = new List<int>();
        public List<int> shortcutbag_item_ID= new List<int>();
        public List<int> shortcutbag_item_itemHeld= new List<int>();
        
        //玩家攝影機
        public int isCameraNum;
        
        //某些事情的觸發狀態，以布林儲存
        public List<KeyBoolValue> trigger_state_bool = new List<KeyBoolValue>();
        //某些事情的觸發狀態，以數字儲存
        public  List<KeyIntValue> trigger_state_int= new  List<KeyIntValue>();
    }
}
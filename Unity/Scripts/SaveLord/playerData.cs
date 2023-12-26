using System.Collections.Generic;
using currentLevel;

namespace SaveLord
{
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
    }
}
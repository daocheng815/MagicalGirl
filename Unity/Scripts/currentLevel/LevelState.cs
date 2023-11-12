using System.Collections.Generic;

namespace currentLevel
{
    public class LevelState
    {
        //地圖名稱
        public Dictionary<LevelStateEnum, string> LevelName = new Dictionary<LevelStateEnum, string>()
        {
            {LevelStateEnum.Ruinesremains,"廢棄廢墟"},
            {LevelStateEnum.EnchantedForest,"魔法之森"},
            {LevelStateEnum.UndergroundCave, "地下洞窟"}
        };
        
        //地圖狀態
        public enum LevelStateEnum
        {
            Ruinesremains,
            EnchantedForest,
            UndergroundCave,
        }
        
        //初始化狀態機
        private LevelStateEnum currentState = LevelStateEnum.Ruinesremains;
        public LevelStateEnum previousState = LevelStateEnum.Ruinesremains;

        //設定狀態機
        public LevelStateEnum CurrentState
        {
            get { return currentState; }
            set
            {
                previousState = currentState;
                currentState = value;
            }
        }
        
        //初始化狀態
        public LevelState()
        {
            CurrentState = LevelStateEnum.Ruinesremains;
        }
    }
}
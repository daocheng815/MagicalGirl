using System.Collections.Generic;
using currentLevel;

namespace Events
{
    internal class LevelName
    {
        //地圖名稱
        public static Dictionary<LevelState.LevelStateEnum, string> LevelNames = new
            Dictionary<LevelState.LevelStateEnum, string>()
            {
                { LevelState.LevelStateEnum.Ruinesremains, "廢棄廢墟" },
                { LevelState.LevelStateEnum.EnchantedForest, "魔法之森" },
                { LevelState.LevelStateEnum.UndergroundCave, "地下洞窟" }
            };
    }
}

namespace currentLevel
{
    public class LevelState
    {
        
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
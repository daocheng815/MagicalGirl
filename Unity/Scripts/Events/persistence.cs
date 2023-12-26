namespace Events
{
    using currentLevel;
    public static class Persistence
    {
        /// <summary>
        /// 讀檔序列號，0為不進行存檔，1~3為存檔
        /// </summary>
        public static int GameLoadNum;
        /// <summary>
        /// 當前地圖名稱
        /// </summary>
        public static LevelState.LevelStateEnum IsLevel;
    }
}

using Events;
using System.IO;
using JetBrains.Annotations;
namespace SaveLord
{
    using UnityEngine;
    public class ArchiveSystemManager : DontDestroySingleton<ArchiveSystemManager>
    {
        /// <summary>
        /// 保存數據
        /// </summary>
        /// <param name="playerData">保存資料</param>
        /// <param name="gameLoadNum">保存資料欄位</param>
        public void Save(PlayerData playerData,int gameLoadNum = 1)
        {
            string json = JsonUtility.ToJson(playerData,true);

            string filePath = Path.Combine(Application.dataPath, "playerDataSave_"+gameLoadNum+".json");
            // 将 JSON 字符串保存到文件
            File.WriteAllText(filePath, json);
        }
        /// <summary>
        /// 讀取存檔資料
        /// </summary>
        /// <param name="gameLoadNum">讀檔資料欄位，0為不獨檔，1之後才算</param>
        /// <returns></returns>
        [CanBeNull]
        public PlayerData Lord(int gameLoadNum = 1)
        {
            PlayerData loadedData = null;
            string filePath = Path.Combine(Application.dataPath, "playerDataSave_" + gameLoadNum + ".json");

            // 检查文件是否存在
            if (File.Exists(filePath))
            {
                // 从文件中读取JSON字符串
                string json = File.ReadAllText(filePath);

                // 将JSON字符串转换为PlayerData对象
                loadedData = JsonUtility.FromJson<PlayerData>(json);
            }
            return loadedData;
        }
    }
}
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SaveLord;

public class FilleManager : MonoBehaviour
{
    private PlayerData[] playerData = new PlayerData[4];
    
    public GameObject play;

    public Damageable damageable;

    public PMagic pMagic;

    public AVGSystem AVGSystem;

    public Inventory mybag;
    public Inventory shortcutbag;

    public string SaveNumSuffix = "_SL";
    //土炮物品處理
    //public item RecoveryWater1;
    //public item RecoveryWater2;
    
    private void Update()
    {   
        /*
        //PlayerPrefs 讀取存儲系統
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
            
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Lord();
        }
        */
        if(ScreenSetting.GameLoadNum == 1)
        {
            ScreenSetting.GameLoadNum = 0;
            Lord(1);
        }
        if(ScreenSetting.GameLoadNum == 2)
        {
            ScreenSetting.GameLoadNum = 0;
            Lord(2);
        }
        if(ScreenSetting.GameLoadNum == 3)
        {
            ScreenSetting.GameLoadNum = 0;
            Lord(3);
        }
    }


    [ContextMenu("存檔")]
    public void Save(int Num = 1)
    {
        //儲存背包數據
        List<int> mybag_item_ID = new List<int>();
        List<int> mybag_item_itemHeld = new List<int>();
        List<int> shortcutbag_item_ID = new List<int>();
        List<int> shortcutbag_item_itemHeld = new List<int>();
        mybag_item_ID.Clear();
        mybag_item_itemHeld.Clear();
        shortcutbag_item_ID.Clear();
        shortcutbag_item_itemHeld.Clear();
        for (int i = 0; i < mybag.itemList.Count; i++)
        {
            if (mybag.itemList[i] != null)
            {
                mybag_item_ID.Add(mybag.itemList[i].itemID);
                mybag_item_itemHeld.Add(mybag.itemList[i].itemHeld);
            }
            else
            {
                mybag_item_ID.Add(114514);
                mybag_item_itemHeld.Add(1);
            }
        }
        for (int i = 0; i < shortcutbag.itemList.Count; i++)
        {
            if (shortcutbag.itemList[i] != null)
            {
                shortcutbag_item_ID.Add(shortcutbag.itemList[i].itemID);
                shortcutbag_item_itemHeld.Add(shortcutbag.itemList[i].itemHeld);
            }
            else
            {
                shortcutbag_item_ID.Add(114514);
                shortcutbag_item_itemHeld.Add(1);
            }
        }
        
        playerData[Num] = new PlayerData()
        {
            magicVar = pMagic.P_Magic,
            healthVar =  damageable.health,
            playerTransformX = play.transform.position.x,
            playerTransformY = play.transform.position.y,
            mybag_item_ID = new List<int>(mybag_item_ID),
            mybag_item_itemHeld = new List<int>(mybag_item_itemHeld),
            shortcutbag_item_ID = new List<int>(shortcutbag_item_ID),
            shortcutbag_item_itemHeld = new List<int>(shortcutbag_item_itemHeld),
        };
        
        string json = JsonUtility.ToJson(playerData[Num],true);

        string filePath = Path.Combine(Application.dataPath, "playerDataSave_"+Num+".json");
        // 将 JSON 字符串保存到文件
        System.IO.File.WriteAllText(filePath, json);
        
        
        /*
        FileStream fs = new FileStream(Application.dataPath + "/Save.sav", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine(damageable.health);
        sw.WriteLine(player.transform.position.x);
        sw.WriteLine(player.transform.position.y);
        sw.Close();
        fs.Close();
        */
        
        /*
        for (int i = 0; i < mybag.itemList.Count; i++)
        {
            PlayerPrefs.SetString(Num + SaveNumSuffix + "mybag_" + i + "itemName", "");
            PlayerPrefs.SetInt(Num + SaveNumSuffix + "mybag_" + i + "itemHeld", 1);
            if (mybag.itemList[i] != null)
            {
                PlayerPrefs.SetString(Num + SaveNumSuffix + "mybag_" + i + "itemName", mybag.itemList[i].itemName);
                PlayerPrefs.SetInt(Num + SaveNumSuffix + "mybag_" + i + "itemHeld", mybag.itemList[i].itemHeld);
            }
        }
        for (int i = 0; i < shortcutbag.itemList.Count; i++)
        {
            PlayerPrefs.SetString(Num + SaveNumSuffix + "shortcutbag_" + i + "itemName", "");
            PlayerPrefs.SetInt(Num + SaveNumSuffix + "shortcutbag_" + i + "itemHeld", 1);
            if (shortcutbag.itemList[i] != null)
            {
                PlayerPrefs.SetString(Num + SaveNumSuffix + "shortcutbag_" + i + "itemName", shortcutbag.itemList[i].itemName);
                PlayerPrefs.SetInt(Num + SaveNumSuffix + "shortcutbag_" + i + "itemHeld", shortcutbag.itemList[i].itemHeld);
            }
        }
        PlayerPrefs.SetInt(Num + SaveNumSuffix + "MagicVar", pMagic.P_Magic);
        PlayerPrefs.SetInt(Num + SaveNumSuffix + "health", damageable.health);
        PlayerPrefs.SetFloat(Num + SaveNumSuffix + "playerTransformX", play.transform.position.x);
        PlayerPrefs.SetFloat(Num + SaveNumSuffix + "playerTransformY", play.transform.position.y);
        Debug.Log("存檔成功");
        */
    }

    [ContextMenu("讀檔")]
    public void Lord(int Num = 1)
    {
        string filePath = Path.Combine(Application.dataPath, "playerDataSave_" + Num + ".json");

        // 检查文件是否存在
        if (File.Exists(filePath))
        {
            // 从文件中读取JSON字符串
            string json = File.ReadAllText(filePath);

            // 将JSON字符串转换为PlayerData对象
            PlayerData loadedData = JsonUtility.FromJson<PlayerData>(json);

            // 现在你可以使用loadedData中的数据
            Debug.Log("Magic Var: " + loadedData.magicVar);
            Debug.Log("Health Var: " + loadedData.healthVar);
            Debug.Log("mybag_item_ID Var: " + loadedData.mybag_item_ID[0]);
            Debug.Log("mybag_item_itemHeld Var: " + loadedData.mybag_item_itemHeld[0]);
            // 以及其他属性...


            play.transform.position = new Vector3(loadedData.playerTransformX,loadedData.playerTransformY,play.transform.position.z);
            damageable.health = loadedData.healthVar;
            pMagic.P_Magic = loadedData.magicVar;

            var mybagListCount = mybag.itemList.Count;
            mybag.itemList.Clear();
            for (int i = 0; i < mybagListCount; i++)
            {
                mybag.itemList.Add(null);
            }
            for (int i = 0; i < mybag.itemList.Count; i++)
            {
                //數值為114514時是空值
                if (loadedData.mybag_item_ID[i] != 114514)
                {
                    mybag.itemList[i] = ItemList.Instance.Item[loadedData.mybag_item_ID[i]];
                    mybag.itemList[i].itemHeld = loadedData.mybag_item_itemHeld[i];
                }
                else
                {
                    mybag.itemList[i] = null;
                }
            }

            var shortcutbagListCount = shortcutbag.itemList.Count;
            shortcutbag.itemList.Clear();
            for (int i = 0; i < shortcutbagListCount; i++)
            {
                shortcutbag.itemList.Add(null);
            }
            for (int i = 0; i < shortcutbag.itemList.Count; i++)
            {
                //數值為114514時是空值
                if (loadedData.shortcutbag_item_ID[i] != 114514)
                {
                    shortcutbag.itemList[i] = ItemList.Instance.Item[loadedData.shortcutbag_item_ID[i]];
                    shortcutbag.itemList[i].itemHeld = loadedData.shortcutbag_item_itemHeld[i];
                }
                else
                {
                    shortcutbag.itemList[i] = null;
                }
            }
        }
        else
        {
            Debug.LogWarning("File does not exist: " + filePath);
        }
        
        invventoryManger.Instance.RefreshItem(0);
        invventoryManger.Instance.RefreshItem(1);
       



        /*
        play.transform.position = new Vector3(PlayerPrefs.GetFloat(Num + SaveNumSuffix + "playerTransformX"), PlayerPrefs.GetFloat(Num + SaveNumSuffix + "playerTransformY"), play.transform.position.z);
        damageable.health = PlayerPrefs.GetInt(Num + SaveNumSuffix + "health");
        pMagic.P_Magic = PlayerPrefs.GetInt(Num + SaveNumSuffix + "MagicVar");
        */
        /*
        FileStream fs = new FileStream(Application.dataPath + "/save.sav", FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        damageable.health = int.Parse(sr.ReadLine());
        player.transform.position = new Vector3(float.Parse(sr.ReadLine()), float.Parse(sr.ReadLine()), player.transform.position.z);
        sr.Close();
        fs.Close();
        */
        
        //清除背包List 並重新生成
        
        /*
        var mybagOrgItemListNum = mybag.itemList.Count;
        mybag.itemList.Clear();
        for (int i = 0; i < mybagOrgItemListNum; i++)
        {
            mybag.itemList.Add(null);
        }
        //遍歷背包List的內容
        for (int i = 0; i < mybag.itemList.Count; i++)
        {
            //PlayerPrefs.Haskey 這是判定如果當前的key存在的話
            if (PlayerPrefs.HasKey(Num + SaveNumSuffix + "mybag_" + i + "itemName"))
            {
                string item = PlayerPrefs.GetString(Num + SaveNumSuffix + "mybag_" + i + "itemName");
                for(int j = 0; j < ItemList.Instance.Item.Count; j++ )
                {
                    if(item == ItemList.Instance.Item[j].itemName.ToString())
                    {
                        mybag.itemList[j] = ItemList.Instance.Item[j];
                        mybag.itemList[j].itemHeld = PlayerPrefs.GetInt(Num + SaveNumSuffix + "mybag_" + j + "itemHeld");
                    }
                }
            }
        }

        var shortcutbagOrgItemListNum = shortcutbag.itemList.Count;
        shortcutbag.itemList.Clear();
        for (int i = 0; i < shortcutbagOrgItemListNum; i++)
        {
            shortcutbag.itemList.Add(null);
        }
        for (int i = 0; i < shortcutbag.itemList.Count; i++)
        {
            //PlayerPrefs.Haskey 這是判定如果當前的key存在的話
            if (PlayerPrefs.HasKey(Num + SaveNumSuffix + "shortcutbag_" + i + "itemName"))
            {
                //Debug.Log(PlayerPrefs.GetString(Num + SaveNumSuffix + "shortcutbag_" + i + "itemName"));
                for(int j = 0; j < ItemList.Instance.Item.Count; j++ )
                {
                    //if(PlayerPrefs.GetString(Num + SaveNumSuffix + "shortcutbag_" + i + "itemName") == ItemList.Instance.Item[j].itemName.ToString())
                    //{
                    //    shortcutbag.itemList[j] = ItemList.Instance.Item[j];
                    //    shortcutbag.itemList[j].itemHeld = PlayerPrefs.GetInt(Num + SaveNumSuffix + "shortcutbag_" + j + "itemHeld");
                    //}
                }
            }
        }
        /*
         
        /*
        var shortcutbagOrgItemListNum = shortcutbag.itemList.Count;
        shortcutbag.itemList.Clear();
        for (int i = 0; i < shortcutbagOrgItemListNum; i++)
        {
            shortcutbag.itemList.Add(null);
        }
        //遍歷背包List的內容
        for (int i = 0; i < shortcutbag.itemList.Count; i++)
        {
            //PlayerPrefs.Haskey 這是判定如果當前的key存在的話
            if (PlayerPrefs.HasKey(Num + SaveNumSuffix + "shortcutbag_" + i + "itemName"))
            {
                string itemra = PlayerPrefs.GetString(Num + SaveNumSuffix + "shortcutbag_" + i + "itemName");
                for(int j = 0; j < ItemList.Instance.Item.Count; j++ )
                {
                    if(itemra == ItemList.Instance.Item[j].itemName.ToString())
                    {
                        shortcutbag.itemList[j] = ItemList.Instance.Item[j];
                        shortcutbag.itemList[j].itemHeld = PlayerPrefs.GetInt(Num + SaveNumSuffix + "shortcutbag_" + j + "itemHeld");
                    }
                }
            }
            else
            {
                Debug.LogError("不存在");
            }
        }
        */
        
        
        /*
        for (int i = 0; i < mybag.itemList.Count; i++)
        {
            //PlayerPrefs.Haskey 這是判定如果當前的key存在的話
            if (PlayerPrefs.HasKey("mybag_" + i + "itemName"))
            {
                string item = PlayerPrefs.GetString("mybag_" + i + "itemName");
                switch (item)
                {
                    //判斷物品是哪個
                    case "RecoveryWater1":
                        mybag.itemList[i] = RecoveryWater1;
                        mybag.itemList[i].itemHeld = PlayerPrefs.GetInt("mybag_" + i + "itemHeld");
                        break;
                    case "RecoveryWater2":
                        mybag.itemList[i] = RecoveryWater2;
                        mybag.itemList[i].itemHeld = PlayerPrefs.GetInt("mybag_" + i + "itemHeld");
                        break;
                    case "":
                        break;
                }
            }
        }*/
        
    }
}

//[CustomEditor(typeof(FilleManager))]
//public class FilleManager_Editor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        FilleManager script = (FilleManager)target;

//        GUILayout.Space(20);
//        if (GUILayout.Button("存檔"))
//        {
//            script.Save();
//        }
//        if (GUILayout.Button("讀檔"))
//        {
//            script.Lord();
//        }
//    }
//}

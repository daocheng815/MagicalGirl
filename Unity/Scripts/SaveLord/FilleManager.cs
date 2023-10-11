using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FilleManager : MonoBehaviour
{
    public GameObject play;

    public Damageable damageable;

    public PMagic pMagic;

    public Inventory mybag;

    public item RecoveryWater1;
    public item RecoveryWater2;

    private void Update()
    {
        // PlayerPrefs 讀取存儲系統
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
            
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Lord();
        }
        if(ScreenSetting.GameLoadNum == 1)
        {
            ScreenSetting.GameLoadNum = 0;
            Lord();
        }
    }

    [ContextMenu("存檔")]
    public void Save()
    {
        /*
        FileStream fs = new FileStream(Application.dataPath + "/Save.sav", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);
        sw.WriteLine(damageable.health);
        sw.WriteLine(player.transform.position.x);
        sw.WriteLine(player.transform.position.y);
        sw.Close();
        fs.Close();
        */
        for (int i = 0; i < mybag.itemList.Count; i++)
        {
            PlayerPrefs.SetString("mybag_" + i + "itemName", "");
            PlayerPrefs.SetInt("mybag_" + i + "itemHeld", 1);
            if (mybag.itemList[i] != null)
            {
                PlayerPrefs.SetString("mybag_" + i + "itemName", mybag.itemList[i].itemName);
                PlayerPrefs.SetInt("mybag_" + i + "itemHeld", mybag.itemList[i].itemHeld);
            }
        }

        PlayerPrefs.SetInt("MagicVar", pMagic.P_Magic);
        PlayerPrefs.SetInt("health", damageable.health);
        PlayerPrefs.SetFloat("playerTransformX", play.transform.position.x);
        PlayerPrefs.SetFloat("playerTransformY", play.transform.position.y);
        Debug.Log("存檔成功");
    }

    [ContextMenu("讀檔")]
    public void Lord()
    {
        /*
        FileStream fs = new FileStream(Application.dataPath + "/save.sav", FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        damageable.health = int.Parse(sr.ReadLine());
        player.transform.position = new Vector3(float.Parse(sr.ReadLine()), float.Parse(sr.ReadLine()), player.transform.position.z);
        sr.Close();
        fs.Close();
        */

        //清除背包List 並重新生成

        mybag.itemList.Clear();
        for (int i = 0; i < 18; i++)
        {
            mybag.itemList.Add(null);
        }
        //遍歷List的內容
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
        }

        play.transform.position = new Vector3(PlayerPrefs.GetFloat("playerTransformX"), PlayerPrefs.GetFloat("playerTransformY"), play.transform.position.z);
        damageable.health = PlayerPrefs.GetInt("health");
        pMagic.P_Magic = PlayerPrefs.GetInt("MagicVar");
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

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

    public AVGSystem AVGSystem;

    public Inventory mybag;

    public string SaveNumSuffix = "_SL";
    //�g�����~�B�z
    //public item RecoveryWater1;
    //public item RecoveryWater2;

    private void Update()
    {   
        /*
        //PlayerPrefs Ū���s�x�t��
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
            Lord();
        }
    }


    [ContextMenu("�s��")]
    public void Save(int Num = 1)
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
            PlayerPrefs.SetString(Num + SaveNumSuffix + "mybag_" + i + "itemName", "");
            PlayerPrefs.SetInt(Num + SaveNumSuffix + "mybag_" + i + "itemHeld", 1);
            if (mybag.itemList[i] != null)
            {
                PlayerPrefs.SetString(Num + SaveNumSuffix + "mybag_" + i + "itemName", mybag.itemList[i].itemName);
                PlayerPrefs.SetInt(Num + SaveNumSuffix + "mybag_" + i + "itemHeld", mybag.itemList[i].itemHeld);
            }
        }

        PlayerPrefs.SetInt(Num + SaveNumSuffix + "MagicVar", pMagic.P_Magic);
        PlayerPrefs.SetInt(Num + SaveNumSuffix + "health", damageable.health);
        PlayerPrefs.SetFloat(Num + SaveNumSuffix + "playerTransformX", play.transform.position.x);
        PlayerPrefs.SetFloat(Num + SaveNumSuffix + "playerTransformY", play.transform.position.y);
        Debug.Log("�s�ɦ��\");
    }

    [ContextMenu("Ū��")]
    public void Lord(int Num = 1)
    {
        /*
        FileStream fs = new FileStream(Application.dataPath + "/save.sav", FileMode.Open);
        StreamReader sr = new StreamReader(fs);
        damageable.health = int.Parse(sr.ReadLine());
        player.transform.position = new Vector3(float.Parse(sr.ReadLine()), float.Parse(sr.ReadLine()), player.transform.position.z);
        sr.Close();
        fs.Close();
        */
        AVGSystem.RemoveDialog_C();
        //�M���I�]List �í��s�ͦ�

        mybag.itemList.Clear();
        for (int i = 0; i < 18; i++)
        {
            mybag.itemList.Add(null);
        }
        //�M���I�]List�����e
        for (int i = 0; i < mybag.itemList.Count; i++)
        {
            //PlayerPrefs.Haskey �o�O�P�w�p�G��e��key�s�b����
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
        /*
        for (int i = 0; i < mybag.itemList.Count; i++)
        {
            //PlayerPrefs.Haskey �o�O�P�w�p�G��e��key�s�b����
            if (PlayerPrefs.HasKey("mybag_" + i + "itemName"))
            {
                string item = PlayerPrefs.GetString("mybag_" + i + "itemName");
                switch (item)
                {
                    //�P�_���~�O����
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
        play.transform.position = new Vector3(PlayerPrefs.GetFloat(Num + SaveNumSuffix + "playerTransformX"), PlayerPrefs.GetFloat(Num + SaveNumSuffix + "playerTransformY"), play.transform.position.z);
        damageable.health = PlayerPrefs.GetInt(Num + SaveNumSuffix + "health");
        pMagic.P_Magic = PlayerPrefs.GetInt(Num + SaveNumSuffix + "MagicVar");
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
//        if (GUILayout.Button("�s��"))
//        {
//            script.Save();
//        }
//        if (GUILayout.Button("Ū��"))
//        {
//            script.Lord();
//        }
//    }
//}


using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using SaveLord;
using Events;
using PlayAction;
using UnityEngine.Serialization;

public class FilleManager : MonoBehaviour
{
    private PlayerData[] playerData = new PlayerData[4];
    
    public GameObject play;

    public Damageable damageable;

    public PMagic pMagic;

    public Inventory mybag;
    public Inventory shortcutbag;
    
    //開始載入時沒有正確載入內容，但有正確執行進入載入階段
    private void Start()
    {
        if (Persistence.GameLoadNum == 0)
        {
            //重製存檔字典資料
            EventRecordManger.Instance.ReSet();
            //遊戲開始時開銷毀背包
            invventoryManger.Instance.deltbag( 0);
            invventoryManger.Instance.deltbag( 1);
        }
        Debug.Log($"Persistence.GameLoadNum : {Persistence.GameLoadNum}");
        switch (Persistence.GameLoadNum)
        {
            case 1:
                Persistence.GameLoadNum = 0;
                Debug.Log("載入1");
                Lord(1);
                break;
            case 2:
                Persistence.GameLoadNum = 0;
                Lord(2);
                break;
            case 3:
                Persistence.GameLoadNum = 0;
                Lord(3);
                break;
            case 0:
                Debug.Log("新的開始，無須讀檔");
                break;
        }
        //更新所有的物件
        ILordInterface[] lordInterfaces = FindObjectsOfType<MonoBehaviour>().OfType<ILordInterface>().ToArray();
        foreach (var I in lordInterfaces)
        {
            I.Init();
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
            isCameraNum = VC2C.Instance.isCameraNum,
            myLevelState = Persistence.IsLevel,
            saveIsTime= System.DateTime.Now.ToString(CultureInfo.InvariantCulture),
            magicVar = pMagic.P_Magic,
            healthVar =  damageable.health,
            playerTransformX = play.transform.position.x,
            playerTransformY = play.transform.position.y,
            mybag_item_ID = new List<int>(mybag_item_ID),
            mybag_item_itemHeld = new List<int>(mybag_item_itemHeld),
            shortcutbag_item_ID = new List<int>(shortcutbag_item_ID),
            shortcutbag_item_itemHeld = new List<int>(shortcutbag_item_itemHeld),
            
            trigger_state_bool = new List<KeyBoolValue>(),
            trigger_state_int = new List<KeyIntValue>(),
        }; 
        //將字典轉換成可序列化的數據再保存
        playerData[Num].trigger_state_bool = EventRecordManger.Instance.TriggerStateBool.Select(kv => new KeyBoolValue { key = kv.Key, value = kv.Value }).ToList();
        playerData[Num].trigger_state_int = EventRecordManger.Instance.TriggerStateINT.Select(kv => new KeyIntValue() { key = kv.Key, value = kv.Value }).ToList();
        //最後再把數據傳入
        ArchiveSystemManager.Instance.Save(playerData[Num],Num);
    }
    [ContextMenu("讀檔")]
    public void Lord(int num = 1)
    {
        PlayerData loadedDates = ArchiveSystemManager.Instance.Lord(num);
        if (loadedDates != null)
        {
            //重製資料
            EventRecordManger.Instance.ReSet();
            //讀取時把字典轉換回來
            EventRecordManger.Instance.TriggerStateBool = loadedDates.trigger_state_bool.ToDictionary(kv => kv.key, kv => kv.value);
            EventRecordManger.Instance.TriggerStateINT = loadedDates.trigger_state_int.ToDictionary(kv => kv.key, kv => kv.value);
            
            //更新攝影機位置
            //線判定定攝影機是否是原先的
            if(VC2C.Instance.CV.gameObject != VC2C.Instance.cvGroup[loadedDates.isCameraNum].gameObject)
                VC2C.Instance.CV.gameObject.SetActive(false);
            VC2C.Instance.cvGroup[loadedDates.isCameraNum].gameObject.SetActive(true);
            VC2C.Instance.CV = VC2C.Instance.cvGroup[loadedDates.isCameraNum];
            Debug.Log(loadedDates.isCameraNum);
            
            VC2C.Instance.UpdateCameraNum();
            //更新玩家位置
            play.transform.position = new Vector3(loadedDates.playerTransformX,loadedDates.playerTransformY,play.transform.position.z);
            damageable.health = loadedDates.healthVar;
            pMagic.P_Magic = loadedDates.magicVar;

            var mybagListCount = mybag.itemList.Count;
            mybag.itemList.Clear();
            for (int i = 0; i < mybagListCount; i++)
            {
                mybag.itemList.Add(null);
            }
            for (int i = 0; i < mybag.itemList.Count; i++)
            {
                //數值為114514時是空值
                if (loadedDates.mybag_item_ID[i] != 114514)
                {
                    mybag.itemList[i] = ItemList.Instance.Item[loadedDates.mybag_item_ID[i]];
                    mybag.itemList[i].itemHeld = loadedDates.mybag_item_itemHeld[i];
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
                if (loadedDates.shortcutbag_item_ID[i] != 114514)
                {
                    shortcutbag.itemList[i] = ItemList.Instance.Item[loadedDates.shortcutbag_item_ID[i]];
                    shortcutbag.itemList[i].itemHeld = loadedDates.shortcutbag_item_itemHeld[i];
                }
                else
                {
                    shortcutbag.itemList[i] = null;
                }
            }
            
            invventoryManger.Instance.RefreshItem(0);
            invventoryManger.Instance.RefreshItem(1);
            
            //更新所有初始更新事件
            ILordInterface[] lordInterfaces = FindObjectsOfType<MonoBehaviour>().OfType<ILordInterface>().ToArray();
            foreach (var I in lordInterfaces)
            {
                I.Init();
            }
            
        }
        else
        {
            Debug.Log("存檔不存在");
        }
        
        
    }
}


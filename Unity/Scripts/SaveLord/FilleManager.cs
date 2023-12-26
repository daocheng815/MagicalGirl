using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using SaveLord;
using Events;
public class FilleManager : MonoBehaviour
{
    private PlayerData[] playerData = new PlayerData[4];
    
    public GameObject play;

    public Damageable damageable;

    public PMagic pMagic;

    public Inventory mybag;
    public Inventory shortcutbag;
    
    private void Update()
    {   
        if(Persistence.GameLoadNum == 1)
        {
            Persistence.GameLoadNum = 0;
            Lord(1);
        }
        if(Persistence.GameLoadNum == 2)
        {
            Persistence.GameLoadNum = 0;
            Lord(2);
        }
        if(Persistence.GameLoadNum == 3)
        {
            Persistence.GameLoadNum = 0;
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
        };
        
        ArchiveSystemManager.Instance.Save(playerData[Num],Num);
    }
    [ContextMenu("讀檔")]
    public void Lord(int num = 1)
    {
        
        PlayerData loadedDates = ArchiveSystemManager.Instance.Lord(num);
        if (loadedDates != null)
        {
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
        }
        else
        {
            Debug.Log("存檔不存在");
        }
        
        
    }
}


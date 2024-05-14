using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using TMPro;
using UnityEngine;
using ItemTypeEnum;

public class invventoryManger : Singleton<invventoryManger>
{
    
    [Header("背包")]
    public Inventory[] Bag;
    public GameObject[] slotGrid;
    public GameObject[] emptySlot;
    //public slot slotPrefab;
    //showInfromation
    public TextMeshProUGUI[] itemInfromation;
    
    //public List<GameObject> slots_0 = new List<GameObject>();
    //public List<GameObject> slots_1 = new List<GameObject>();
    //利用字典儲存List，這是一個比較複雜的二重泛型，要注意
    public Dictionary<string, List<GameObject>> slotsDictionary = new Dictionary<string, List<GameObject>>()
    {
        { "slots_0",  new List<GameObject>() },
        { "slots_1",  new List<GameObject>() }
    };
    
    private void OnEnable()
    {
        itemInfromation[0].text = "";
        PlayerController.lockplay = true;
    }
    private void OnDisable()
    {
        PlayerController.lockplay = false;
    }

    /// <summary>
    /// 更新物品描述
    /// </summary>
    public void UpdateItemInfo(string itemDescription)
    {
        //slotGrid.SetActive(false);
        itemInfromation[0].text = itemDescription;
    }
    /// <summary>
    /// 移除全部的物品
    /// </summary>
    public void RemoveAllItem(int bagNum)
    {
        for (int i = 0; i < Bag[bagNum].itemList.Count; i++)
        {
            if(Bag[bagNum].itemList[i] != null)
            {
                Bag[bagNum].itemList[i].itemHeld = 1;
                Bag[bagNum].itemList[i] = null;
            }
        }
    }
    /// <summary>
    /// 新增物品
    /// </summary>
    public bool AddNewItem(int itemID,int bagNum,int addNum)
    {
        bool index = true;
        item item = ItemList.Instance.Item[itemID];
        if (ItemExistenceCheckerAllBag(itemID))
        {
            item.itemHeld += addNum;
            ItemEvents.ItemDropsTheSuccess(addNum,item);
        }
        else
        {
            int? nullBag = CheckerBagItem(bagNum);
            if (nullBag != null)
            {
                Debug.Log(nullBag);
                Bag[bagNum].itemList[(int)nullBag] = item;
                item.itemHeld += addNum - 1 ;
                ItemEvents.ItemDropsTheSuccess(addNum,item);
            }
            else
            {
                Debug.Log("背包中沒有可添加的空位，無法添加物品");
                GameMessageEvents.AddMessage("背包中沒有可添加的空位，無法獲取物品",3f);
                index = false;
            }
        }
        return index;
    }
    /// <summary>
    /// 使用KEY
    /// </summary>
    /// <param name="bagnum"></param>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public bool UsingKey(int bagnum,int itemID)
    {
        var item = ItemList.Instance.Item[itemID];
        if (item.itemType != ItemType.Key)
            return false;
        if (ItemExistenceCheckerBag(bagnum,itemID))
        {
            var i = CheckerItemIsABagToSlotID(bagnum,itemID);
            Deleteitems(i, bagnum);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 移除物品。
    /// </summary>
    public void Deleteitems(int slotID,int bagNum)
    {
        if (Bag[bagNum].itemList[slotID].itemHeld > 1)
            Bag[bagNum].itemList[slotID].itemHeld -= 1;
        else
        {
            if(bagNum != 1)
                itemInfromation[bagNum].text = "";
            Bag[bagNum].itemList[slotID] = null;
        }
        RefreshItem(bagNum);
    }

    /// <summary>
    /// 檢查某個背包是否存在某個物品
    /// </summary>
    /// <param name="b"></param>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public bool ItemExistenceCheckerBag(int b ,int itemID)
    {
        bool index = false;
        item item = ItemList.Instance.Item[itemID];
        index = Bag[b].itemList.Contains(item);
        return index;
    }
    /// <summary>
    /// 檢查某個物品在背包中的SLOT
    /// </summary>
    /// <param name="b"></param>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public int CheckerItemIsABagToSlotID(int b ,int itemID)
    {
        int index = 0;
        for (int i = 0; i < Bag[b].itemList.Count; i++)
        {
            if (Bag[b].itemList[i].itemID == itemID)
            {
                index = i;
                break; 
            }
        }
        return index;
    }
    /// <summary>
    /// 檢查所有背包是否存在某個物品
    /// </summary>
    /// <param name="itemID"></para m>
    /// <returns></returns>
    public bool ItemExistenceCheckerAllBag(int itemID)
    {
        bool index = false;
        item item = ItemList.Instance.Item[itemID];
        foreach (var bag in Bag)
        {
            index = bag.itemList.Contains(item);
            if(index)
                break;
        }
        return index;
    }
    /// <summary>
    /// 檢查所有背包是否存在某個物品，且是否有足夠的物品，如妥有就扣除
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public bool ItemExistenceCheckerAllBagNumDel(int itemID,int num)
    {
        bool index = false;
        if (ItemExistenceCheckerAllBag(itemID))
        {
            if (ItemList.Instance.Item[itemID].itemHeld >= num)
            {
                ItemList.Instance.Item[itemID].itemHeld -= num;
                index = true;
            }
        }
        return index;
    }
    /// <summary>
    /// 檢查某個背包的儲存格是否存在物品
    /// </summary>
    /// <param name="bagNum">背包</param>
    /// <param name="slotID">儲存格ID</param>
    /// <returns></returns>
    public bool ItemExistenceChecker(int bagNum, int slotID)
    {
            return Bag[bagNum].itemList[slotID] != null;
    }
    /// <summary>
    /// 檢查某個背包是否有空位，並回傳最近的空位，若是沒有回傳null
    /// </summary>
    /// <param name="bagNum"></param>
    /// <returns></returns>
    public int? CheckerBagItem(int bagNum)
    {
        int? index = null;
        //驗證bagNum是否正確
        if (bagNum > 0 || bagNum < Bag.Length)
        {
            index = Bag[bagNum].itemList.FindIndex(item => item == null);
        }
        return index;
    }
    /// <summary>
    /// 處理BUFF物品 ，未完成
    /// </summary>
    /// <param name="playerObject">BUFF對象</param>
    /// <param name="slotID">物品所在的槽位 ID。</param>
    /// <param name="bagNum">目前背包</param>
    /// <returns></returns>
    public bool Buff(GameObject playerObject, int slotID, int bagNum)
    {
        bool index = false;
        if (!ItemExistenceChecker(bagNum,slotID))
            return false;
        if (Bag[bagNum].itemList[slotID].itemType == ItemType.Buff)
        {
            if (Bag[bagNum].itemList[slotID].available)
            {
                //物件身上是否有某個BUFF
                if (BuffManger.Instance.ExamineBuff(playerObject, Bag[bagNum].itemList[slotID].buffID))
                {
                    index = false;
                }
                else
                {
                    //判定物品目前是否還可以疊加使用
                    if (BuffManger.Instance.ExamineBuffOverlay(playerObject, Bag[bagNum].itemList[slotID].buffID))
                    {
                        BuffEvents.AddBuff(playerObject, Bag[bagNum].itemList[slotID].buffID);
                        Deleteitems(slotID,bagNum);
                        RefreshItem(bagNum);
                        index = true;
                    }
                    else
                    {
                        index = false;
                        GameMessageEvents.AddMessage("效果已疊滿",3f);
                    }
                    
                }
            }
        }
        return index;
    }
    /// <summary>
    /// 這個方法用於處理淨化藥水的使用。
    /// </summary>
    /// <param name="playerObject">藥水回復對象。</param>
    /// <param name="slotID">物品所在的槽位 ID。</param>
    /// <param name="bagNum">目前背包</param>
    public bool Purify(GameObject playerObject,int slotID,int bagNum)
    {
        bool index = false;
        if (!ItemExistenceChecker(bagNum,slotID))
            return false;
        if (Bag[bagNum].itemList[slotID].itemType == ItemType.Purify)
        {
            if (Bag[bagNum].itemList[slotID].available)
            {
                if (PlayerVar.Instance.shuDyeingVar.TestFilthyVar(0))
                {
                    Debug.Log("當前沒有汙染值");
                    GameMessageEvents.AddMessage("汙染值為零，無須使用淨化。",3f);
                }
                else
                {
                    PlayerVar.Instance.shuDyeingVar.FilthySub(Bag[bagNum].itemList[slotID].purifyNum);
                    Deleteitems(slotID,bagNum);
                    RefreshItem(bagNum);
                    index = true;
                }
            }
        }
        return index;
    }
    
    /// <summary>
    /// <summary>
    /// 這個方法用於處理藥水的使用。
    /// </summary>
    /// <param name="playerObject">藥水回復對象。</param>
    /// <param name="slotID">物品所在的槽位 ID。</param>
    /// <param name="bag">目前背包</param>
    public bool Potion(GameObject playerObject,int slotID,int bagNum)
    {
        bool index = false;
        if (!ItemExistenceChecker(bagNum,slotID))
            return false;
        if (Bag[bagNum].itemList[slotID].itemType == ItemType.Potion)
        {
            if (Bag[bagNum].itemList[slotID].available )
            {
                bool isHeal = playerObject.GetComponent<Damageable>().Heal(Bag[bagNum].itemList[slotID].Healnum);
                if (!isHeal)
                {
                    Debug.Log("當前血量已滿");
                    GameMessageEvents.AddMessage("血量已滿",3f);
                }
                else
                {
                    index = true;
                    Debug.Log(Bag[bagNum].itemList[slotID].coolingTime);
                    //如果是最後一個物品也返回否，為了冷卻時間的UI正常運作
                    if (Bag[bagNum].itemList[slotID].itemHeld == 1)
                        index = false;
                    Deleteitems(slotID,bagNum);
                    RefreshItem(bagNum);
                }
            }
            Debug.Log(playerObject.GetComponent<Damageable>().health);
        }

        return index;
    }
    /// <summary>
    /// 獲取某背包儲存格數量
    /// </summary>
    /// <param name="bagNum"></param>
    /// <returns></returns>
    public int BagCount(int bagNum)
    {
        bagNum = Mathf.Clamp(bagNum, 0, Bag.Length);
        return Bag[bagNum].itemList.Count;
    }
    /// <summary>
    /// 刪除背包，並且重新生成
    /// </summary>
    public void RefreshItem(int bagNum)
    {
        //銷毀背包物件並重新生成
        for (int i=0 ; i < slotGrid[bagNum].transform.childCount ; i++)
        {
            if (slotGrid[bagNum].transform.childCount == 0)
                break;
            Destroy(slotGrid[bagNum].transform.GetChild(i).gameObject);
        }
        slotsDictionary["slots_"+bagNum].Clear();
        for (int i = 0; i < Bag[bagNum].itemList.Count; i++)
        {
            
            //CreateNewItem(instance.myBag.itemList[i]);
            slotsDictionary["slots_"+bagNum].Add(Instantiate(emptySlot[bagNum]));
            slotsDictionary["slots_"+bagNum][i].transform.SetParent(slotGrid[bagNum].transform);
            slotsDictionary["slots_"+bagNum][i].GetComponent<slot>().slotID = i;
            //slotsDictionary["slots_" + bagNum][i].GetComponent<slot>().slotItem = Bag[bagNum].itemList[i];
            slotsDictionary["slots_"+bagNum][i].GetComponent<slot>().SetupSlot(Bag[bagNum].itemList[i]);
            //將背包列表中的物件傳遞回來
            slotsDictionary["slots_"+bagNum][i].GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            //當我在生成快捷背包時
            if (Bag[bagNum].name == "shortcut")
            {
                slotsDictionary["slots_" + bagNum][i].GetComponent<slot>().tmp.text = (i+1).ToString();
            }
        }
        Debug.Log("更新背包完成");
        if (bagNum == 1)
        {
            ItemUpDate.BagUpData.Invoke();
        }
    }
    /// <summary>
    /// 刪除背包List，並且重新生成List
    /// </summary>
    public void resetbag(int bagNum) 
    {
        for(int i = 0;i < Bag[bagNum].itemList.Count;i++)
        {
            if(Bag[bagNum].itemList[i] != null)
            {
                Bag[bagNum].itemList[i].itemHeld = 1;
            }
        }

        var orgItemListNum = Bag[bagNum].itemList.Count;
        Bag[bagNum].itemList.Clear();
        for (int i = 0; i <orgItemListNum; i++)
        {
            Bag[bagNum].itemList.Add(null);
        }
    }

    [ContextMenu("銷毀背包")]
    public void deltbag(int bagNum)
    {
        resetbag(bagNum);
        RefreshItem(bagNum);
    }
    
    /// <summary>
    /// 搜尋背包內物品
    /// </summary>
    /// <param name="bag">背包</param>
    /// <param name="slot">格子</param>
    /// <returns></returns>
    public static item ReturnBagSlotItem(int bag,int slot)
    {
        if (Instance.Bag[bag].itemList[slot] == null)
        {
            return null;
        }
        return Instance.Bag[bag].itemList[slot];
    }
}



//[CustomEditor(typeof(invventoryManger))]
//public class invventorymanger_editor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        invventoryManger script = (invventoryManger)target;

//        GUILayout.Space(20);
//        if(GUILayout.Button("銷毀背包"))
//        {
//            invventoryManger.resetbag();
//            invventoryManger.RefreshItem();
//        }
//    }
//}
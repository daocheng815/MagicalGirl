

using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

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
    
    public Dictionary<string, List<GameObject>> slotsDictionary = new Dictionary<string, List<GameObject>>()
    {
        { "slots_0",  new List<GameObject>() },
        { "slots_1",  new List<GameObject>() }
    };
    
    
    private void OnEnable()
    {
        RefreshItem(0);
        RefreshItem(1);
        itemInfromation[0].text = "";
        PlayerController.lockplay = true;
    }
    private void OnDisable()
    {
        PlayerController.lockplay = false;
    }
    private void Start()
    {
        
        //
        RefreshItem(0);
        RefreshItem(1);
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
    public void AddNewItem(item thisItem,int bagNum)
    {
        // 判斷這個列表(背包)中是否已經沒有這個物品
        if (!Bag[bagNum].itemList.Contains(thisItem))
        {
            //判定在背包的18格中是否有空位
            for (int i = 0; i < Bag[bagNum].itemList.Count; i++)
            {
                if (Bag[bagNum].itemList[i] == null)
                {
                    Bag[bagNum].itemList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {
            thisItem.itemHeld += 1;
        }
        RefreshItem(0);
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
        RefreshItem(0);
    }
    /// <summary>
    /// 這個方法用於處理淨化藥水的使用。
    /// </summary>
    /// <param name="playerObject">藥水回復對象。</param>
    /// <param name="slotID">物品所在的槽位 ID。</param>
    /// <param name="bag">目前背包</param>
    public void Purify(GameObject playerObject,int slotID,int bagNum)
    {
        if (Bag[bagNum].itemList[slotID].available)
        {
            if (!PlayerVar.Instance.shuDyeingVar.TestFilthyVar(0))
            {
                PlayerVar.Instance.shuDyeingVar.FilthySub(Bag[bagNum].itemList[slotID].purifyNum);
                Deleteitems(slotID,bagNum);
                RefreshItem(bagNum);
            }
        }
    }
    /// <summary>
    /// <summary>
    /// 這個方法用於處理藥水的使用。
    /// </summary>
    /// <param name="playerObject">藥水回復對象。</param>
    /// <param name="slotID">物品所在的槽位 ID。</param>
    /// <param name="bag">目前背包</param>
    public void Potion(GameObject playerObject,int slotID,int bagNum)
    {
        if (Bag[bagNum].itemList[slotID].available)
        {
            bool isHeal = playerObject.GetComponent<Damageable>().Heal(Bag[bagNum].itemList[slotID].Healnum);
            if (isHeal)
            {
                Deleteitems(slotID,bagNum);
                RefreshItem(bagNum);
            }
        }
        Debug.Log(playerObject.GetComponent<Damageable>().health);
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
            slotsDictionary["slots_"+bagNum].Clear();
        }
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
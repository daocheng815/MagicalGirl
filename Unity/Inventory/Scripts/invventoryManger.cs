
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class invventoryManger : MonoBehaviour
{
    static invventoryManger instance;

    public Inventory myBag;
    public GameObject slotGrid;
    public GameObject emptySlot;
    //public slot slotPrefab;
    public TextMeshProUGUI itemInfromation;
    public List<GameObject> slots = new List<GameObject>();
    

    void Awake()
    {
        if (instance!=null)
            Destroy(this);
        instance = this;
    }

    private void OnEnable()
    {
        RefreshItem();
        instance.itemInfromation.text = "";
        PlayerController.lockplay = true;
        
    }
    private void OnDisable()
    {
        PlayerController.lockplay = false;
    }
    private void Start()
    {
        RefreshItem();
    }
    /// <summary>
    /// 更新物品描述
    /// </summary>
    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemInfromation.text = itemDescription;
    }
    /// <summary>
    /// 移除全部的物品
    /// </summary>
    public static void RemoveAllItem()
    {
        for (int i = 0; i < instance.myBag.itemList.Count; i++)
        {
            if(instance.myBag.itemList[i] != null)
            {
                instance.myBag.itemList[i].itemHeld = 1;
                instance.myBag.itemList[i] = null;
            }
        }
    }
    /// <summary>
    /// 新增物品
    /// </summary>
    public static void AddNewItem(item thisItem)
    {
        // 判斷這個列表(背包)中是否已經沒有這個物品
        if (!instance.myBag.itemList.Contains(thisItem))
        {
            //判定在背包的18格中是否有空位
            for (int i = 0; i < instance.myBag.itemList.Count; i++)
            {
                if (instance.myBag.itemList[i] == null)
                {
                    instance.myBag.itemList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {
            thisItem.itemHeld += 1;
        }
        RefreshItem();
    }
    /// <summary>
    /// 移除物品。
    /// </summary>
    public static void Deleteitems(int slotID)
    {
        if (instance.myBag.itemList[slotID].itemHeld > 1)
            instance.myBag.itemList[slotID].itemHeld -= 1;
        else
        {
            instance.itemInfromation.text = "";
            instance.myBag.itemList[slotID] = null;
        }
        RefreshItem();
    }

    /// <summary>
    /// 這個方法用於處理藥水的使用。
    /// </summary>
    /// <param name="playerObject">藥水回復對象。</param>
    /// <param name="slotID">物品所在的槽位 ID。</param>
    public static void potion(GameObject playerObject,int slotID)
    {
        if (instance.myBag.itemList[slotID].available)
        {
            bool isHeal = playerObject.GetComponent<Damageable>().Heal(instance.myBag.itemList[slotID].Healnum);
            if (isHeal)
            {
                Deleteitems(slotID);
            }
        }
        Debug.Log(playerObject.GetComponent<Damageable>().health);
    }
    /// <summary>
    /// 刪除背包，並且重新生成
    /// </summary>
    public static void RefreshItem()
    {
        //銷毀背包物件並重新生成
        for (int i=0; i< instance.slotGrid.transform.childCount;i++)
        {
            if (instance.slotGrid.transform.childCount == 0)
                break;
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }
        for (int i = 0; i<instance.myBag.itemList.Count;i++)
        {
            //CreateNewItem(instance.myBag.itemList[i]);
            instance.slots.Add(Instantiate(instance.emptySlot));
            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            instance.slots[i].GetComponent<slot>().slotID = i;
            instance.slots[i].GetComponent<slot>().SetupSlot(instance.myBag.itemList[i]);
            //將背包列表中的物件傳遞回來
        }
    }
    /// <summary>
    /// 刪除背包List，並且重新生成List
    /// </summary>
    public static void resetbag() 
    {
        for(int i = 0;i < instance.myBag.itemList.Count;i++)
        {
            if(instance.myBag.itemList[i] != null)
            {
                instance.myBag.itemList[i].itemHeld = 1;
            }
        }
        instance.myBag.itemList.Clear();
        for (int i = 0; i <18; i++)
        {
            instance.myBag.itemList.Add(null);
        }
    }

    [ContextMenu("銷毀背包")]
    public static void deltbag()
    {
        invventoryManger.resetbag();
        invventoryManger.RefreshItem();
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
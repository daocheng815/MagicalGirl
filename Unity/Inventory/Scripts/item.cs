using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ItemTypeEnum;

[CreateAssetMenu(fileName = "New Item",menuName = "Inventory/New Item")]
// ScriptableObject 是Unity面板中新增物件的方法
public class item : ScriptableObject
{
    /// <summary>
    /// 獨一無二的ID
    /// </summary>
    [Header("物品ID")]
    public int itemID;
    [Header("物品類別")] 
    public ItemType itemType = new ItemType();
    [Header("物品名稱")]
    public string itemName;
    [Header("物品圖示")]
    public Sprite itemImaage;
    [Header("物品數量")]
    public int itemHeld;
    [Header("物品描述")]
    [TextArea]
    public string iteminfo;
    [Header("可使用物品")]
    public bool available;
    [Header("是否有冷卻時間")]
    public bool isCoolingTime;
    [Header("冷卻時間")]
    public float coolingTime;
    [Header("回血血量")]
    public int Healnum;
}


using System.Collections;
using System.Collections.Generic;
using Events;
using ItemTypeEnum;
using UnityEngine;

public class shortcutKeyManger : MonoBehaviour
{
    public GameObject player;

    public List<coolingItmeUI> coolingItemUIList = new List<coolingItmeUI>();

    public bool isUpData = false;
    private void OnEnable()
    {
        ItemUpDate.BagUpData += UpDates;
    }

    private void OnDisable()
    {
        ItemUpDate.BagUpData -= UpDates;
    }

    void UpDates()
    { 
        coolingItemUIList.Clear();
        
        GameObject Grid = GameObject.Find("Grid");

        coolingItmeUI[] coolingItmeUis = Grid.GetComponentsInChildren<coolingItmeUI>();
        foreach (coolingItmeUI coolingItemUI in coolingItmeUis)
        {
            coolingItemUIList.Add(coolingItemUI);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Action(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Action(1, 1);
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Action(1, 2);
        }
        
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            Action(1, 3);
        }
    }

    
    void Action(int bag , int slot)
    {
        var item = invventoryManger.ReturnBagSlotItem(bag, slot);
        if (item != null)
        {
            
            Debug.Log("1  :" + item.itemNameNbt);
            ItemType tpye = item.itemType;
            if (tpye == ItemType.Purify)
            {
                //給使用物品一個回傳值(這個值用來表示是否真的使用)
                bool index = invventoryManger.Instance.Purify(player,slot, bag);
                if(index)
                    type(tpye,item);
            }
            if (tpye == ItemType.Potion)
            { 
                bool index =  invventoryManger.Instance.Potion(player,slot, bag);
                if(index)
                    type(tpye,item);
            }
            if (tpye == ItemType.Buff)
            { 
                bool index =  invventoryManger.Instance.Buff(player,slot, bag);
                if(index)
                    type(tpye,item);
            }
        }
        else
        {
            Debug.Log("當前沒有物品");
        }
    }
    void type(ItemType type , item item)
    {
        foreach (var i in coolingItemUIList)
        {
            if (i != null)
            {
                if (i.slot.slotItem != null)
                {
                    if (i.slot.slotItem.itemType == type)
                    {
                        i.Fade(item.coolingTime);
                    }
                }
            }
        }
    }
}

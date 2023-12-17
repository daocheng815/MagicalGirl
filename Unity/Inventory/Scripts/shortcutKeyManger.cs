using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using ItemTypeEnum;
using UnityEngine;

public class shortcutKeyManger : MonoBehaviour
{
    public GameObject player;
    public GameObject grid;

    public GameObject shortcutPrefab;
    public GameObject sGrid;
    public List<coolingItmeFadeUI> coolingItmeFadeUIList = new List<coolingItmeFadeUI>();
    public bool[] delay;
    
    
    public List<coolingItmeUI> coolingItemUIList = new List<coolingItmeUI>();

    public bool isUpData = false;

    private void Start()
    {
        for (int i = 0; i < invventoryManger.Instance.BagCount(1); i++)
        {
            GameObject prefab =  Instantiate(shortcutPrefab, transform.position, Quaternion.identity);
            prefab.transform.SetParent(sGrid.transform);
            coolingItmeFadeUI c = prefab.gameObject.GetComponent<coolingItmeFadeUI>();
            c.slotID = i;
            coolingItmeFadeUIList.Add(c);
            prefab.gameObject.GetComponent<RectTransform>().localScale = Vector2.one;
        }
        delay = new bool[coolingItmeFadeUIList.Count];
    }

    private void OnEnable()
    {
        ItemUpDate.BagUpData += UpDates;
    }

    private void OnDisable()
    {
        ItemUpDate.BagUpData -= UpDates;
    }

    private void UpDates()
    { 
        Debug.Log("更新");
        coolingItemUIList.Clear();
        coolingItmeUI[] coolingItmeUis = grid.GetComponentsInChildren<coolingItmeUI>();
        
        foreach (coolingItmeUI coolingItemUI in coolingItmeUis)
        {
            coolingItemUIList.Add(coolingItemUI);
        }
    }
    private void Update()
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

    
    private void Action(int bag , int slot)
    {
        var item = invventoryManger.ReturnBagSlotItem(bag, slot);
        if (item != null)
        {
            
            Debug.Log("1  :" + item.itemNameNbt);
            ItemType tpye = item.itemType;
            switch (tpye)
            {
                case ItemType.Purify:
                    if(!delay[slot])
                        if (invventoryManger.Instance.Purify(player, slot, bag))
                            Fade(item, slot);
                    break;
                case ItemType.Potion:
                    if(!delay[slot])
                        if (invventoryManger.Instance.Potion(player, slot, bag))
                            Fade(item, slot);
                    break;
                case ItemType.Buff:
                    if(!delay[slot])
                        if (invventoryManger.Instance.Buff(player, slot, bag))
                            Fade(item, slot);
                    break;
            }
        }
        else
        {
            Debug.Log("當前沒有物品");
        }
    }

    private void Fade(item item,int slot)
    {
        if (!delay[slot])
        {
            var delayTime = item.coolingTime;
            StartCoroutine(DelayFadTime(delayTime, slot));
            coolingItmeFadeUIList[slot].Fade(delayTime);
        }
    }

    private IEnumerator DelayFadTime(float time , int slot)
    {
        delay[slot] = true;
        yield return new WaitForSeconds(time);
        delay[slot] = false;
    }
    private void type(ItemType type , item item)
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

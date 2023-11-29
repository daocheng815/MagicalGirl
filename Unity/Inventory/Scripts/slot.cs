using System.Collections;
using System.Collections.Generic;
using ItemTypeEnum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class slot : MonoBehaviour
{
    public int slotID; //空格ID
    public item slotItem;
    public Image slotImage;
    public TextMeshProUGUI slotNum;
    public string slotInfo;
    

    public GameObject itemInSlot;

    GameObject playerObject;
    public Inventory bag;
    

    private void Awake()
    {
        playerObject = GameObject.Find("player");
    }
    public void ItemOnClicked()
    {
        invventoryManger.Instance.UpdateItemInfo(slotInfo);
        //先判定物品Type是否正確
        if (slotItem.itemType == ItemType.Potion)
        {
            if (bag.name == "mybag")
                invventoryManger.Instance.Potion(playerObject, slotID,0);
            if (bag.name == "shortcut")
                invventoryManger.Instance.Potion(playerObject, slotID,1);
        }
        else if (slotItem.itemType == ItemType.purify)
        {
            if (bag.name == "mybag")
                invventoryManger.Instance.Purify(playerObject, slotID,0);
            if (bag.name == "shortcut")
                invventoryManger.Instance.Purify(playerObject, slotID,1);
        }
    }


    public void SetupSlot(item item)
    {
        if(item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }

        slotItem = item;
        slotImage.sprite = item.itemImaage;
        slotNum.text = item.itemHeld.ToString();
        slotInfo = item.iteminfo;
    }
}

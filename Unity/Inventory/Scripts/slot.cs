using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class slot : MonoBehaviour
{
    public int slotID; //ªÅ®æID
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
        if (bag.name == "mybag")
            invventoryManger.Instance.Potion(playerObject, slotID,0);
        if (bag.name == "shortcut")
            invventoryManger.Instance.Potion(playerObject, slotID,1);
    }


    public void SetupSlot(item item)
    {
        if(item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }
        
        slotImage.sprite = item.itemImaage;
        slotNum.text = item.itemHeld.ToString();
        slotInfo = item.iteminfo;
    }
}

using Events;
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

    private RectTransform rt;
    
    public TextMeshProUGUI tmp;
    private void Awake()
    {
        playerObject = GameObject.Find("player");
        rt = GetComponent<RectTransform>();
    }
    public void ItemOnClicked()
    {
        invventoryManger.Instance.UpdateItemInfo(slotInfo);
        //先判定物品Type是否正確
        //監聽，顯示快捷物品選單
        BagFuncMenu.ItemOnClicked.Invoke(slotID, slotItem,bag, rt.localPosition);
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

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

// 滑鼠拖曳控制
public class itemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //紀錄原始父級
    public Transform originalparent;
    public Inventory[] mybag;
    private int currentItemID;//當前物品ID
    public GameObject itemPrefab;

    public void OnBeginDrag(PointerEventData eventData)
    {  
        //原始父級
        originalparent = transform.parent;
        //當前物品ID
        currentItemID = originalparent.GetComponent<slot>().slotID;
        //設定本身物件的父級，至少要高到不會影響UI視線
        transform.SetParent(transform.parent.parent.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    // 使用CanvasGroup來判定滑鼠映射(也就是滑鼠所指向的點)，blocksRaycasts是用來設定是否能夠穿透物件獲取底下的物件。

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //拖動(Drag)時回傳當前滑鼠映射的物件名稱
        if(eventData.pointerCurrentRaycast.gameObject != null)
            Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        //滑鼠射線不等於沒有任何物件
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == "itemPrefab(Clone)" && itemPrefab.name == "shortcutItemPrefab(Clone)"
                ||
                eventData.pointerCurrentRaycast.gameObject.name == "shortcutItemPrefab(Clone)" && itemPrefab.name == "itemPrefab(Clone)"
               )
            {
                
                Debug.Log("碰到");
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                //itemList的物品儲存位置置換
                mybag[1].itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID] = mybag[0].itemList[currentItemID];
                mybag[0].itemList[currentItemID] = null;

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                invventoryManger.Instance.RefreshItem(0);
                invventoryManger.Instance.RefreshItem(1);
                
                return;
            
            }
            //判定目前滑鼠映射的遊戲物件名稱是itemImage
            if (eventData.pointerCurrentRaycast.gameObject.name == "itemImage")
            {
                Debug.Log("滑鼠鼠標下沒有任何物件");
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;

                //itemList的物品儲存位置置換
                var temp = mybag[0].itemList[currentItemID];
                mybag[0].itemList[currentItemID] = mybag[0].itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID];
                mybag[0].itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID] = temp;

                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalparent.position;
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalparent);
                
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                invventoryManger.Instance.RefreshItem(0);
                invventoryManger.Instance.RefreshItem(1);
                
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject.name == itemPrefab.name && mybag[0].itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<slot>().slotID] == null)
            {
                Debug.Log("交換");
                //直接掛在slot下
                
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                //itemList的物品儲存位置置換
                mybag[0].itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID] = mybag[0].itemList[currentItemID];
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<slot>().slotID != currentItemID)
                    mybag[0].itemList[currentItemID] = null;

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                invventoryManger.Instance.RefreshItem(0);
                invventoryManger.Instance.RefreshItem(1);
                
                return;
            }
            
            
        }
        Debug.Log("歸位");
        //其他任何物件都得歸位
        transform.SetParent(originalparent);
        transform.position = originalparent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    
}

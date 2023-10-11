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
    public Inventory mybag;
    private int currentItemID;//當前物品ID

    public void OnBeginDrag(PointerEventData eventData)
    {  
        originalparent = transform.parent;
        currentItemID = originalparent.GetComponent<slot>().slotID;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    // 使用CanvasGroup來判定滑鼠映射(也就是滑鼠所指向的點)，blocksRaycasts是用來設定是否能夠穿透物件獲取底下的物件。

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //拖動(Drag)時回傳當前滑鼠映射的物件名稱
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                //判定目前滑鼠映射的遊戲物件名稱是itemImage
                if (eventData.pointerCurrentRaycast.gameObject.name == "itemImage")
                {
                    transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                    transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;

                    //itemList的物品儲存位置置換
                    var temp = mybag.itemList[currentItemID];
                    mybag.itemList[currentItemID] = mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID];
                    mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID] = temp;

                    eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalparent.position;
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalparent);
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    invventoryManger.RefreshItem();
                    return;
                }
                if (eventData.pointerCurrentRaycast.gameObject.name == "itemPrefab(Clone)" && mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<slot>().slotID] == null)
                {
                    //直接掛在slot下
                    transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                    transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                    //itemList的物品儲存位置置換
                    mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID] = mybag.itemList[currentItemID];
                    if (eventData.pointerCurrentRaycast.gameObject.GetComponent<slot>().slotID != currentItemID)
                        mybag.itemList[currentItemID] = null;

                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    invventoryManger.RefreshItem();
                    return;
                }
            }

            //其他任何物件都得歸位
            transform.SetParent(originalparent);
            transform.position = originalparent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    
}

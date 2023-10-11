using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

// �ƹ��즲����
public class itemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //������l����
    public Transform originalparent;
    public Inventory mybag;
    private int currentItemID;//��e���~ID

    public void OnBeginDrag(PointerEventData eventData)
    {  
        originalparent = transform.parent;
        currentItemID = originalparent.GetComponent<slot>().slotID;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    // �ϥ�CanvasGroup�ӧP�w�ƹ��M�g(�]�N�O�ƹ��ҫ��V���I)�AblocksRaycasts�O�Ψӳ]�w�O�_�����z����������U������C

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //���(Drag)�ɦ^�Ƿ�e�ƹ��M�g������W��
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                //�P�w�ثe�ƹ��M�g���C������W�٬OitemImage
                if (eventData.pointerCurrentRaycast.gameObject.name == "itemImage")
                {
                    transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                    transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;

                    //itemList�����~�x�s��m�m��
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
                    //�������bslot�U
                    transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                    transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                    //itemList�����~�x�s��m�m��
                    mybag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID] = mybag.itemList[currentItemID];
                    if (eventData.pointerCurrentRaycast.gameObject.GetComponent<slot>().slotID != currentItemID)
                        mybag.itemList[currentItemID] = null;

                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    invventoryManger.RefreshItem();
                    return;
                }
            }

            //��L���󪫥󳣱o�k��
            transform.SetParent(originalparent);
            transform.position = originalparent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    
}

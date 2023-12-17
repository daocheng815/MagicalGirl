using Events;
using UnityEngine;
using UnityEngine.EventSystems;

// �ƹ��즲����
public class itemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //������l����
    public Transform originalparent;
    public Inventory[] mybag;
    private int currentItemID;//��e���~ID
    public GameObject itemPrefab;

    public void OnBeginDrag(PointerEventData eventData)
    {
        BagFuncMenu.IsItemOnDrag.Invoke(true);
        Debug.Log("��ʶ}�l");
        //��l����
        originalparent = transform.parent;
        //��e���~ID
        currentItemID = originalparent.GetComponent<slot>().slotID;
        //�]�w�������󪺤��šA�ܤ֭n���줣�|�v�TUI���u
        transform.SetParent(transform.parent.parent.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    // �ϥ�CanvasGroup�ӧP�w�ƹ��M�g(�]�N�O�ƹ��ҫ��V���I)�AblocksRaycasts�O�Ψӳ]�w�O�_�����z����������U������C

    public void OnDrag(PointerEventData eventData)
    {
        
        transform.position = eventData.position;
        //���(Drag)�ɦ^�Ƿ�e�ƹ��M�g������W��
        if(eventData.pointerCurrentRaycast.gameObject != null)
            Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        //�ƹ��g�u������S�����󪫥�
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == "itemPrefab(Clone)" && itemPrefab.name == "shortcutItemPrefab(Clone)"
                ||
                eventData.pointerCurrentRaycast.gameObject.name == "shortcutItemPrefab(Clone)" && itemPrefab.name == "itemPrefab(Clone)"
               )
            {
                
                Debug.Log("�I��");
                BagFuncMenu.IsItemOnDrag(false);
                
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                //itemList�����~�x�s��m�m��
                mybag[1].itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID] = mybag[0].itemList[currentItemID];
                mybag[0].itemList[currentItemID] = null;

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                invventoryManger.Instance.RefreshItem(0);
                invventoryManger.Instance.RefreshItem(1);
                
                return;
            
            }
            //�P�w�ثe�ƹ��M�g���C������W�٬OitemImage
            if (eventData.pointerCurrentRaycast.gameObject.name == "itemImage" && itemPrefab.name == "itemPrefab(Clone)"||
                eventData.pointerCurrentRaycast.gameObject.name == "shortcutitemImage" && itemPrefab.name == "shortcutItemPrefab(Clone)")
            {
                Debug.Log("�ƹ����ФU�S�����󪫥�");
                BagFuncMenu.IsItemOnDrag(false);
                
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;

                //itemList�����~�x�s��m�m��
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
            if (eventData.pointerCurrentRaycast.gameObject.name == "itemImage" && itemPrefab.name == "shortcutItemPrefab(Clone)"||
                eventData.pointerCurrentRaycast.gameObject.name == "shortcutitemImage" && itemPrefab.name == "itemPrefab(Clone)")
            {
                Debug.Log("�I��");
                BagFuncMenu.IsItemOnDrag(false);
                
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                //itemList�����~�x�s��m�m��
                var temp = mybag[1].itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID];
                mybag[1].itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID] = mybag[0].itemList[currentItemID];
                mybag[0].itemList[currentItemID] = temp;

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                invventoryManger.Instance.RefreshItem(0);
                invventoryManger.Instance.RefreshItem(1);
                
                return;
            }
            if (eventData.pointerCurrentRaycast.gameObject.name == itemPrefab.name && mybag[0].itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<slot>().slotID] == null)
            {
                Debug.Log("�洫");
                BagFuncMenu.IsItemOnDrag(false);
                
                //�������bslot�U
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
                //itemList�����~�x�s��m�m��
                mybag[0].itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<slot>().slotID] = mybag[0].itemList[currentItemID];
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<slot>().slotID != currentItemID)
                    mybag[0].itemList[currentItemID] = null;

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                invventoryManger.Instance.RefreshItem(0);
                invventoryManger.Instance.RefreshItem(1);
                
                return;
            }
            
            
        }
        Debug.Log("�k��");
        BagFuncMenu.IsItemOnDrag(false);
        
        //��L���󪫥󳣱o�k��
        transform.SetParent(originalparent);
        transform.position = originalparent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}

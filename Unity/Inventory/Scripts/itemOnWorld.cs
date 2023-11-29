using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Events;
public class itemOnWorld : MonoBehaviour
{
    public item thisItem;
    public Inventory[] playinventory;
    private SpriteRenderer sr;
    private bool hasTriggered = false;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            AddNewItem();
            sr.DOFade(0f, 0.3f).OnComplete((() => Destroy(gameObject,0.3f)));
        }
    }
    public void AddNewItem()
    {
        //�ˬd�I�]�}�C���O�_���o�Ӫ��~�F
        bool isItem = false;
        int isitem = 0;
        for (int i = 0; i < playinventory.Length; i++)
        {
            isItem = playinventory[i].itemList.Contains(thisItem);
            if (isItem)
                isitem = i;
        }
        //�p�G�I�]�}�C����
        if (isItem)
        {
            thisItem.itemHeld += 1;
        }
        else
        {
            if (!playinventory[0].itemList.Contains(thisItem))
            {
                //�P�w�b�I�]��18�椤�O�_���Ŧ�
                for (int i = 0; i < playinventory[0].itemList.Count;i++)
                {
                    if (playinventory[0].itemList[i] == null)
                    {
                        playinventory[0].itemList[i] = thisItem;
                        break;
                    }
                }
            }
            else
            {
                thisItem.itemHeld += 1;
            }
        }
        //��s�I�]�e���d�y�A�������p�G�Y�Ӷ��Z�ɶ����u�|��s�@���C
        //invventoryManger.Instance.RefreshItem(1);
        //invventoryManger.Instance.RefreshItem(0);
        //����������~
        ItemDropsEvents.DropsSuccess();
    }
}

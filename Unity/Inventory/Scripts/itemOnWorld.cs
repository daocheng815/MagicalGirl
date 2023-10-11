using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class itemOnWorld : MonoBehaviour
{
    public item thisItem;
    public Inventory playinventory;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }

    public void AddNewItem()
    {
        // �P�_�o�ӦC��(�I�])���O�_�w�g�S���o�Ӫ��~
        if (!playinventory.itemList.Contains(thisItem))
        {
            //playinventory.itemList.Add(thisItem);
            //invventoryManger.CreateNewItem(thisItem);
            //�P�w�b�I�]��18�椤�O�_���Ŧ�
            for (int i = 0; i < playinventory.itemList.Count;i++)
            {
                if (playinventory.itemList[i] == null)
                {
                    playinventory.itemList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {
            thisItem.itemHeld += 1;
        }
    }
}

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
        // 判斷這個列表(背包)中是否已經沒有這個物品
        if (!playinventory.itemList.Contains(thisItem))
        {
            //playinventory.itemList.Add(thisItem);
            //invventoryManger.CreateNewItem(thisItem);
            //判定在背包的18格中是否有空位
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

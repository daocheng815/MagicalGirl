using System;
using System.Collections;
using UnityEngine;
using Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ItemDropsWorldManger : MonoBehaviour
{
    [SerializeField]private int dropsSuccessNum = 0;
    [SerializeField]private float dropsSuccessDelayTime = 0.5f;
    private bool successDelayTime = false;
    
    public GameObject drop;
    public Vector2 randomOffset = new Vector2(0,0);
    
    [Range(0f,100f)]
    public float chance;
    [Range(0,6)]
    public int itemID;
    public Vector3 test;
    private void OnEnable()
    {
        //註冊監聽事件
        ItemDropsEvents.itemDropsWorld += ItemDropsWorld;
        ItemDropsEvents.DropsSuccess += DropsSuccess;
    }
    private void OnDisable()
    {
        //註消監聽事件
        ItemDropsEvents.itemDropsWorld -= ItemDropsWorld;
        ItemDropsEvents.DropsSuccess -= DropsSuccess;
    }
    //更新背包需要在間距時間內只會更新一次，以減少卡頓。
    //等待更新背包
    private IEnumerator BagUpdataCoroutine()
    {
        successDelayTime = true;
        yield return new WaitForSeconds(dropsSuccessDelayTime);
        if (dropsSuccessNum > 0)
        {
            invventoryManger.Instance.RefreshItem(1);
            invventoryManger.Instance.RefreshItem(0);
            dropsSuccessNum = 0;
        }
        successDelayTime = false;
    }
    private void DropsSuccess()
    {
        dropsSuccessNum ++;
        if(!successDelayTime)
            StartCoroutine(BagUpdataCoroutine());
    }
    private void ItemDropsWorld(Vector3 pos, float dropsChance, int itemID,Vector3 offset)
    {
        //如果還沒更新就等待執行
        if(!itemUpDate.itemListUpDate)
            StartCoroutine(WaitForItemUpData(pos, dropsChance, itemID,offset));
        else
            Idw114514(pos,dropsChance,itemID,offset);
    }
    //等待更新後執行
    private IEnumerator WaitForItemUpData(Vector3 pos, float dropsChance, int itemID, Vector3 offset)
    {
        while (!itemUpDate.itemListUpDate)
        {
            yield return null;
        }
        Idw114514(pos,dropsChance,itemID,offset);
    }
    private void Idw114514(Vector3 pos, float dropsChance, int itemID,Vector3 offset)
    {
        //掉落物品事件等待時間為offset.z
        StartCoroutine(WaitForIdw114514(pos, dropsChance, itemID, offset));
    
        IEnumerator WaitForIdw114514(Vector3 pos, float dropsChance, int itemID, Vector3 offset)
        {
            yield return new WaitForSeconds(offset.z);
            //限定機率在0~100
            dropsChance = Mathf.Clamp(dropsChance, 0f, 100f);
            //設定隨機數
            var dropRange = Random.Range(0f, 1f);
        
            if (dropRange <= dropsChance/100)
            {
                Vector2 offsets = new Vector2(offset.x, offset.y);
                //限定物品ID在有限的範圍內
                itemID = Mathf.Clamp(itemID, 0, ItemList.Instance.Item.Count - 1);
                //設定初始位置
                Vector3 rangeOffset = new Vector3(Random.Range(-randomOffset.x,randomOffset.x), Random.Range(-randomOffset.y,randomOffset.y),0);
                GameObject drops = Instantiate(drop, pos + (Vector3)offsets + rangeOffset, Quaternion.identity);
                drops.gameObject.GetComponent<Rigidbody2D>().velocity = Random.Range(0f,1f) <= 0.5f ? Vector2.one : -Vector2.one;
                drops.gameObject.GetComponent<itemOnWorld>().thisItem = ItemList.Instance.Item[itemID];
                drops.gameObject.GetComponent<SpriteRenderer>().sprite = ItemList.Instance.Item[itemID].itemImaage;
            }
        }
    }
}

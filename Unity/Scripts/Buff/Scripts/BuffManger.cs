using Events;
using JetBrains.Annotations;
using UnityEngine;

public class BuffManger : Singleton<BuffManger>
{
    public GameObject player;
    public BuffList allBuffList;
    public Buff[] buffs;
    private void OnEnable()
    {
        BuffEvents.AddBuff += AddBuff;
        BuffEvents.DelBuff += DelBuff;
    }

    private void OnDisable()
    {
        BuffEvents.AddBuff -= AddBuff;
        BuffEvents.DelBuff -= DelBuff;
    }

    protected override void Awake()
    {
        base.Awake();
        UpDataAllBuffList();
    }
    //這裡有一個隱憂，就是如果資料過於龐大，也許初始化時會產生錯誤
    /// <summary>
    /// 更新原始Buff列表
    /// </summary>
    private void UpDataAllBuffList()
    {
        allBuffList.buffs.Clear();
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i].buffID = i;
            allBuffList.buffs.Add(buffs[i]);
        }
    }
    /// <summary>
    /// 判定目前是Buff否還可以疊加使用
    /// </summary>
    /// <param name="obj0"></param>
    /// <param name="buffID"></param>
    /// <returns></returns>
    public bool ExamineBuffOverlay(GameObject obj0, int buffID)
    {
        bool index = false;
        buffEffect obj = obj0.GetComponent<buffEffect>();
        if (obj != null && !obj.isUpData)
        {
            //設置buffID的最小值
            buffID = Mathf.Clamp(buffID, 0, buffs.Length - 1);
            Buff buff = allBuffList.buffs[buffID];
            //判定目前是Buff否還可以疊加使用，可以是True
            if (buff.buffOverlay)
            {
                index = obj.buffOverlayNum[buffID] != buff.maxBuffOverlayNum;
            }
            else
            {
                index = false;
            }
        }
        else
        {
            index = false;
        }
        return index;
    }
    /// <summary>
    /// 檢查某物件身上是否有某個BUFF，如果物件身上沒有buffEffect也是回傳False
    /// </summary>
    /// <param name="obj0"></param>
    /// <param name="buffID"></param>
    public bool ExamineBuff(GameObject obj0, int buffID)
    {
        bool index = false;
        buffEffect obj = obj0.GetComponent<buffEffect>();
        if (obj != null && !obj.isUpData)
        {
            //設置buffID的最小值
            buffID = Mathf.Clamp(buffID, 0, buffs.Length - 1);
            Buff buff = allBuffList.buffs[buffID];
            //檢查BUFF列表中是否存在效果了
            index = obj.buffList.Contains(buff);
            
            if(buff.buffOverlay)
                index = false;
        }
        else
        {
            index = false;
        }
        return index;
    }
    /// <summary>
    /// 新增Buff效果
    /// </summary>
    /// <param name="obj0"></param>
    /// <param name="buffID"></param>
    public void AddBuff(GameObject obj0,int buffID)
    {
        buffEffect obj = obj0.GetComponent<buffEffect>();
        if (obj != null && !obj.isUpData)
        {
            //設置buffID的最小值
            buffID = Mathf.Clamp(buffID, 0, buffs.Length - 1);
            Buff buff = allBuffList.buffs[buffID];
            //檢查BUFF列表中是否存在效果了
            bool contains = obj.buffList.Contains(buff);
            //如果不存在，且不可疊加時
            if (!contains && !buff.buffOverlay)
            {
                Debug.Log("新增Buff");
                //將BUFF加入清單
                obj.buffList.Add(buff);
                //開始BUFF計時
                obj.StartBuffTime(buff);
            }
            //如果可疊加，未疊滿時(不用理是否存在於列表了)
            if (!buff.buffOverlay) return;
            if (obj.buffOverlayNum[buffID] >= buff.maxBuffOverlayNum) return;
            
            //重製效果時間
            obj.buffTime[buffID] = 0f;
            //初始化層數
            if (obj.buffOverlayNum[buffID] == 0)
                obj.buffOverlayNum[buffID] = 1;
            //如果存在這個效果，就增加層數
            if (contains)
            {
                obj.buffOverlayNum[buffID] += 1;
                //移除原有效果
                DelBuff(obj0, buffID);
            }
            Debug.Log("新增Buff");
            obj.buffList.Add(buff);
            obj.StartBuffTime(buff);
        }
        else
        {
            Debug.Log("對象"+obj0+"沒有buff池，無法添加BUFF");
        }
    }

    /// <summary>
    /// 移除Buff效果
    /// </summary>
    /// <param name="obj0"></param>
    /// <param name="buffID"></param>
    public void DelBuff(GameObject obj0, int buffID)
    {
        buffEffect obj = obj0.GetComponent<buffEffect>();
        if (obj != null)
        {
            //搜尋buffList清單內是否有該buff
            for (int i = 0; i < obj.buffList.Count; i++)
            {
                Buff buff = obj.buffList[i];
                if (buff.buffID == buffID)
                {
                    //刪除指定索引位置的元素，後面的往前替補
                    obj.buffList.RemoveAt(i);
                }
            }
        }
        
    }
}

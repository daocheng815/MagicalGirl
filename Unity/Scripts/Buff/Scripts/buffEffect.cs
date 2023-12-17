using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Events;
using JetBrains.Annotations;
using UnityEngine;
using Debug = UnityEngine.Debug;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class buffEffect : MonoBehaviour
{
    //獲取腳本，且我希望Buff效果的處理僅此在這個腳本中
    
    [SerializeField]private Damageable damageable;
    [SerializeField]private PMagic pMagic;
    [SerializeField]private ShuDyeingVar shuDyeingVar;
    [SerializeField]private attack[] attacks;
    [SerializeField]private bool isDamageable;
    [SerializeField]private bool isShuDyeingVar;
    [SerializeField]private bool isAttacks;
    [SerializeField]private bool isPMagic;
    
    //記錄協成
    private Coroutine _buffCoroutine;
    
    // buff相關
    private int _buffNum;
    /// <summary>
    /// Buff列表
    /// </summary>
    public List<Buff> buffList = new List<Buff>();
    /// <summary>
    /// 是否在效果時間
    /// </summary>
    public bool[] isBuffTime;
    /// <summary>
    /// 當前效果時間
    /// </summary>
    public float[] buffTime;
    /// <summary>
    /// 效果疊加次數
    /// </summary>
    public int[] buffOverlayNum;
    /// <summary>
    /// 是否在冷卻時間
    /// </summary>
    public bool[] isBuffCooling;
    /// <summary>
    /// 當前冷卻時間
    /// </summary>
    public float[] buffCoolingTime;
    /// <summary>
    /// 更新資料
    /// </summary>
    public bool isUpData = true;
    [Range(0,10)]
    public int buffID;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        if (damageable != null)
            isDamageable = true;
        pMagic = GetComponent<PMagic>();
        if (pMagic != null)
            isPMagic = true;
        shuDyeingVar = GetComponent<ShuDyeingVar>();
        if (shuDyeingVar != null)
            isShuDyeingVar = true;
        attacks = gameObject.GetComponentsInChildren<attack>();
        if (attacks != null)
            isAttacks = true;
    }

    private void Start()
    {
        BuffUpData();
    }
    
    /// <summary>
    /// 開始計時效果時間
    /// </summary>
    /// <param name="buff">buff</param>
    public void StartBuffTime(Buff buff)
    {
        int buffID = buff.buffID;
        if (!isBuffTime[buffID])
        {
            isBuffTime[buffID] = true;
            //觸發Buff計時器
            _buffCoroutine = StartCoroutine(Timer(buff.buffTime));
            //如果當前物件是玩家的話就觸發UI的動作
            TriggerBuff(buff);
            UiAndMessage("附加"+buff.buffNameNbt+buff.buffType+"效果", buff);
        }//判定如果BUFF可以疊加
        else if(buff.buffOverlay)
        {
            
             TriggerBuff(buff);
            UiAndMessage("附加" + buff.buffNameNbt + buff.buffType + "效果，" + "目前層數" + buffOverlayNum[buffID] + "層", buff);
        }
        IEnumerator Timer(float time)
        {
            Debug.Log("T開始");
            buffTime[buffID] = 0f;
            while (buffTime[buffID] < time)
            {
                buffTime[buffID] += Time.deltaTime;
                yield return null;
            }
            isBuffTime[buffID] = false;
            buffTime[buffID] = 0f;
            buffOverlayNum[buffID] = 0;
            BuffEvents.DelBuff.Invoke(gameObject, buffID);
            Debug.Log("T結束");
        }
    }
    //觸發BUFF玩家UI與訊息
    private void UiAndMessage(string str,Buff buff)
    {
        if (gameObject.CompareTag("Player"))
        {
            BuffEvents.PlayerCreateBuffPrefab(buff,gameObject);
            GameMessageEvents.AddMessage(str, 3f);
        }
    }
    /// <summary>
    /// 每秒回血
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="spacing"></param>
    /// <param name="healNum"></param>
    /// <returns></returns>
    private IEnumerator Heal(Buff buff,float spacing,int healNum)
    {
        var timer = 0f;
        var spacingTimer = 0f;
        var isOverlayNum = buffOverlayNum[buff.buffID];
        while (timer < buff.buffTime && isBuffTime[buff.buffID] && isOverlayNum == buffOverlayNum[buff.buffID])
        {
            timer += Time.deltaTime;
            spacingTimer += Time.deltaTime;
            yield return null;
            if (spacingTimer >= spacing)
            {
                damageable.Heal(healNum);
                spacingTimer = 0f;
            }
        }
    }
    /// <summary>
    /// 每秒回復汙染值
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="spacing"></param>
    /// <param name="dyeingNum"></param>
    /// <returns></returns>
    private IEnumerator DyeingFilthySub(Buff buff,float spacing,int dyeingNum)
    {
        var timer = 0f;
        var spacingTimer = 0f;
        var isOverlayNum = buffOverlayNum[buff.buffID];
        while (timer < buff.buffTime && isBuffTime[buff.buffID] && isOverlayNum == buffOverlayNum[buff.buffID])
        {
            timer += Time.deltaTime;
            spacingTimer += Time.deltaTime;
            yield return null;
            if (spacingTimer >= spacing)
            {
                shuDyeingVar.FilthySub(dyeingNum);
                spacingTimer = 0f;
            }
        }
    }
    
    //觸發效果選擇器
    private void TriggerBuff(Buff buff)
    {
        switch (buff.buffID)
        {
            case 0:
            {
                break;
            }
            case 1:
            {
                break;
            }
            case 2:
            {
                switch (buffOverlayNum[buff.buffID])
                {
                    case 1:
                    {
                        StartCoroutine(Heal(buff, 1f,1));
                        break;
                    }
                    case 2:
                    {
                        StartCoroutine(Heal(buff, 1f,1));
                        StartCoroutine(DyeingFilthySub(buff, 1f,1));
                        break;
                    }
                    case 3:
                    {
                        StartCoroutine(Heal(buff, 1f,2));
                        StartCoroutine(DyeingFilthySub(buff, 1f,1));
                        break;
                    }
                    case 4:
                    {
                        StartCoroutine(Heal(buff, 1f,2));
                        StartCoroutine(DyeingFilthySub(buff, 1f,2));
                        break;
                    }
                    case 5:
                    {
                        StartCoroutine(Heal(buff, 1f,3));
                        StartCoroutine(DyeingFilthySub(buff, 1f,2));
                        break;
                    }
                }
                break;
            }
            case 3:
            {
                break;
            }
        }
        
    }
    /// <summary>
    /// 更新自身資料
    /// </summary>
    private void BuffUpData()
    {
        isUpData = true;
        _buffNum = BuffManger.Instance.allBuffList.buffs.Count;
        if (_buffNum != 0)
        {
            isBuffTime = new bool[_buffNum];
            buffTime = new float[_buffNum];
            isBuffCooling = new bool[_buffNum];
            buffCoolingTime = new float[_buffNum];
            buffOverlayNum = new int[_buffNum];
        }
        isUpData = false;
    }

}
#if UNITY_EDITOR
[CustomEditor(typeof(buffEffect))]
public class buffEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        buffEffect be = (buffEffect)target;
        if (GUILayout.Button("新增Buff"))
        {
            BuffEvents.AddBuff.Invoke(be.gameObject,be.buffID);
        }
    }
}    
#endif

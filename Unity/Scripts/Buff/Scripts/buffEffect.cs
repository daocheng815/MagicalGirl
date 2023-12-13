using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class buffEffect : MonoBehaviour
{
    
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
            StartCoroutine(Timer(buff.buffTime));
            //如果當前物件是玩家的話就觸發UI的動作
            if (gameObject.CompareTag("Player"))
            {
                BuffEvents.PlayerCreateBuffPrefab(buff,gameObject);
            }
        }//判定如果BUFF可以疊加
        else if(buff.buffOverlay)
        {
            if (gameObject.CompareTag("Player"))
            {
                BuffEvents.PlayerCreateBuffPrefab(buff,gameObject);
            }
        }
        IEnumerator Timer(float time)
        {
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
        }
    }
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

    private void Buff001()
    {
        
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

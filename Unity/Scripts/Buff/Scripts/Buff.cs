using buff;
using UnityEngine;
[CreateAssetMenu(fileName = "New Buff",menuName = "Buff/New Buff")]
public class Buff : ScriptableObject
{   /// <summary>
    /// Buff ID
    /// </summary>
    [Header("Buff ID")] public int buffID;
    /// <summary>
    /// Buff 名稱
    /// </summary>
    [Header("Buff 名稱")] public string buffName;
    /// <summary>
    /// Buff 中文名稱
    /// </summary>
    [Header("Buff 中文名稱")] public string buffNameNbt;
    /// <summary>
    /// Buff 詳細資訊
    /// </summary>
    [Header("Buff 詳細資訊"),TextArea] public string buffInfo;
    /// &lt;summary&gt;
    /// Buff 類別
    /// &lt;/summary&gt;
    [Header("Buff 類別")] public BuffType buffType;
    /// <summary>
    /// Buff 圖示
    /// </summary>
    [Header("Buff 圖示")] public Sprite buffImage;
    /// <summary>
    /// Buff 背景圖片
    /// </summary>
    [Header("Buff 背景圖片")] public Sprite buffBackGround;
    /// &lt;summary&gt;
    /// Buff 是否可疊加
    /// &lt;/summary&gt;
    [Header("Buff 是否可疊加")] public bool buffOverlay;
    /// <summary>
    /// Buff 最大疊加數
    /// </summary>
    [Header("Buff 最大疊加數")] public int maxBuffOverlayNum;
    /// <summary>
    /// Buff 效果時間
    /// </summary>
    [Header("Buff 效果時間")] public float buffTime;
    /// <summary>
    /// Buff 是否有冷卻時間
    /// </summary>
    [Header("Buff 是否有冷卻時間")] public bool buffCooling;
    /// <summary>
    /// Buff 冷卻時間
    /// </summary>
    [Header("Buff 冷卻時間")] public float buffCoolingTime;
}


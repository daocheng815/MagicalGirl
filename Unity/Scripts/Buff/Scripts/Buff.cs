using buff;
using UnityEngine;
[CreateAssetMenu(fileName = "New Buff",menuName = "Buff/New Buff")]
public class Buff : ScriptableObject
{   /// <summary>
    /// Buff ID
    /// </summary>
    [Header("Buff ID")] public int buffID;
    /// <summary>
    /// Buff �W��
    /// </summary>
    [Header("Buff �W��")] public string buffName;
    /// <summary>
    /// Buff ����W��
    /// </summary>
    [Header("Buff ����W��")] public string buffNameNbt;
    /// <summary>
    /// Buff �ԲӸ�T
    /// </summary>
    [Header("Buff �ԲӸ�T"),TextArea] public string buffInfo;
    /// &lt;summary&gt;
    /// Buff ���O
    /// &lt;/summary&gt;
    [Header("Buff ���O")] public BuffType buffType;
    /// <summary>
    /// Buff �ϥ�
    /// </summary>
    [Header("Buff �ϥ�")] public Sprite buffImage;
    /// <summary>
    /// Buff �I���Ϥ�
    /// </summary>
    [Header("Buff �I���Ϥ�")] public Sprite buffBackGround;
    /// &lt;summary&gt;
    /// Buff �O�_�i�|�[
    /// &lt;/summary&gt;
    [Header("Buff �O�_�i�|�[")] public bool buffOverlay;
    /// <summary>
    /// Buff �̤j�|�[��
    /// </summary>
    [Header("Buff �̤j�|�[��")] public int maxBuffOverlayNum;
    /// <summary>
    /// Buff �ĪG�ɶ�
    /// </summary>
    [Header("Buff �ĪG�ɶ�")] public float buffTime;
    /// <summary>
    /// Buff �O�_���N�o�ɶ�
    /// </summary>
    [Header("Buff �O�_���N�o�ɶ�")] public bool buffCooling;
    /// <summary>
    /// Buff �N�o�ɶ�
    /// </summary>
    [Header("Buff �N�o�ɶ�")] public float buffCoolingTime;
}


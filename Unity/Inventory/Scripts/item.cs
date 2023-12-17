using UnityEngine;
using ItemTypeEnum;

[CreateAssetMenu(fileName = "New Item",menuName = "Inventory/New Item")]
// ScriptableObject �OUnity���O���s�W���󪺤�k
public class item : ScriptableObject
{
    /// <summary>
    /// �W�@�L�G��ID
    /// </summary>
    [Header("���~ID")]
    public int itemID;
    [Header("���~���O")] 
    public ItemType itemType = new ItemType();
    [Header("���~�W��")]
    public string itemName;
    [Header("���~����W��")]
    public string itemNameNbt;
    [Header("���~�ϥ�")]
    public Sprite itemImaage;
    [Header("���~�ƶq")]
    public int itemHeld;
    [Header("���~�y�z")]
    [TextArea(10, 10)]
    public string iteminfo;
    [Header("�i�ϥΪ��~")]
    public bool available;
    [Header("�O�_���N�o�ɶ�")]
    public bool isCoolingTime;
    [Header("�N�o�ɶ�")]
    public float coolingTime;
    [Header("�^���q")]
    public int Healnum;
    [Header("�b�Ʀ��V�q")]
    public int purifyNum;
    [Header("BUFFID")]
    public int buffID;
}


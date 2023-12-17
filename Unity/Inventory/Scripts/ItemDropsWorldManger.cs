using System;
using System.Collections;
using Events;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ItemDropsWorldManger : MonoBehaviour
{
    public GameObject player;
    public float audioVolume = 1f;
    public AudioClip pickUpSource;
    private AudioSource _audioSource;
    public float sourceDelayTime = 0.1f;
    [Space(5)]
    
    [SerializeField]private int dropsSuccessNum;
    [SerializeField]private float dropsSuccessDelayTime = 0.5f;
    private bool _successDelayTime;
    
    public GameObject drop;
    public Vector2 randomOffset = new Vector2(0,0);
    
    [Range(0f,100f)]
    public float chance;
    #if UNITY_EDITOR
    [ItemIDName("���~�W�� : ")]
    #endif
    [Range(0,7)]
    public int itemID;
    public Vector3 test;

    private int[] _itemDragNum ;

    
    private void OnEnable()
    {
        //���U��ť�ƥ�
        ItemEvents.ItemDropsTheWorld += ItemDropsTheWorld;
        ItemEvents.ItemDropsTheSuccess += Drops_Success;
    }
    private void OnDisable()
    {
        //������ť�ƥ�
        ItemEvents.ItemDropsTheWorld -= ItemDropsTheWorld;
        ItemEvents.ItemDropsTheSuccess -= Drops_Success;
    }

    private void Start()
    {
        StartCoroutine(WaitForitemListUpData());
        IEnumerator WaitForitemListUpData()
        {
            //����ItemList��s����
            yield return new WaitUntil((() => ItemUpDate.ItemListUpDate));
            _itemDragNum = new int[ItemList.Instance.Item.Count];
        }
    }

    //��s�I�]�ݭn�b���Z�ɶ����u�|��s�@���A�H��֥d�y�C
    //���ݧ�s�I�]
    private IEnumerator BagUpdataCoroutine()
    {
        _successDelayTime = true;
        yield return new WaitForSeconds(dropsSuccessDelayTime);
        if (dropsSuccessNum > 0)
        {
            invventoryManger.Instance.RefreshItem(0);
            invventoryManger.Instance.RefreshItem(1);
            dropsSuccessNum = 0;

            for (int i = 0; i < _itemDragNum.Length; i++)
            {
                if (_itemDragNum[i] != 0)
                {
                    GameMessageEvents.AddMessage("��o"+ItemList.Instance.Item[i].itemNameNbt+"�A�ƶq:"+_itemDragNum[i] +"��",3f);
                    Invoke("AudioPlay",sourceDelayTime);
                    _itemDragNum[i] = 0;
                }
            }
            
        }
        _successDelayTime = false;
    }
    /// <summary>
    /// ���񪫫~�������
    /// </summary>
    private void AudioPlay()
    {
        _audioSource = player.AddComponent<AudioSource>();
        _audioSource.clip = pickUpSource;
        _audioSource.loop = false;
        _audioSource.volume = audioVolume;
        _audioSource.Play();
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private void Drops_Success(string itemName,int itemNum,item item)
    {
        _itemDragNum[item.itemID]++;
        dropsSuccessNum ++;
        if(!_successDelayTime)
            StartCoroutine(BagUpdataCoroutine());
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private void ItemDropsTheWorld(Vector3 pos, Vector2 dropsChance, int itemID,Vector3 offset)
    {
        //�p�G�٨S��s�N���ݰ���
        if(!ItemUpDate.ItemListUpDate)
            StartCoroutine(WaitItemUpData(pos, dropsChance, itemID,offset));
        else
            Idw114514(pos,dropsChance,itemID,offset);
    }
    //���ݧ�s�����
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator WaitItemUpData(Vector3 pos, Vector2 dropsChance, int itemid, Vector3 offset)
    {
        while (!ItemUpDate.ItemListUpDate)
        {
            yield return null;
        }
        Idw114514(pos,dropsChance,itemID,offset);
    }
    
    private void Idw114514(Vector3 pos, Vector2 dropsChance, int itemID,Vector3 offset)
    {
        //�������~�ƥ󵥫ݮɶ���offset.z
        StartCoroutine(WaitForIdw114514(pos, dropsChance, itemID, offset));
    
        IEnumerator WaitForIdw114514(Vector3 pos, Vector2 dropsChance, int itemID, Vector3 offset)
        {
            yield return new WaitForSeconds(offset.z);
            //���w���v�b0~100
            dropsChance.x = Mathf.Clamp(dropsChance.x, 0f, 100f);
            //�]�w�H����
            var dropRange = Random.Range(0f, 1f);
        
            if (dropRange <= dropsChance.x/100)
            {
                Vector2 offsets = new Vector2(offset.x, offset.y);
                //���w���~ID�b�������d��
                itemID = Mathf.Clamp(itemID, 0, ItemList.Instance.Item.Count - 1);
                //�]�w��l��m
                Vector3 rangeOffset = new Vector3(Random.Range(-randomOffset.x,randomOffset.x), Random.Range(-randomOffset.y,randomOffset.y),0);
                GameObject drops = Instantiate(drop, pos + (Vector3)offsets + rangeOffset, Quaternion.identity);
                drops.gameObject.GetComponent<Rigidbody2D>().velocity = Random.Range(0f,1f) <= 0.5f ? Vector2.one : -Vector2.one;
                itemOnWorld itemOnWorld = drops.gameObject.GetComponent<itemOnWorld>();
                itemOnWorld.thisItem = ItemList.Instance.Item[itemID];
                itemOnWorld.itemAddNum = (int)dropsChance.y;
                drops.gameObject.GetComponent<SpriteRenderer>().sprite = ItemList.Instance.Item[itemID].itemImaage;
            }
        }
    }
}

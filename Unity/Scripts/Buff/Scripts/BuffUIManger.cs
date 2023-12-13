using Unity.Mathematics;
using UnityEngine;
using Events;

public class BuffUIManger : MonoBehaviour
{
    public GameObject buffUIPrefab;
    public GameObject buffUIGrid;

    public GameObject[] buffPrefabList ;
    private void OnEnable()
    {
        BuffEvents.PlayerCreateBuffPrefab += CreateBuffPrefab;
    }
    private void OnDisable()
    {
        BuffEvents.PlayerCreateBuffPrefab -= CreateBuffPrefab;
    }

    public void Start()
    {
        var buffNum = BuffManger.Instance.allBuffList.buffs.Count;
        buffPrefabList = new GameObject[buffNum];
    }

    public void CreateBuffPrefab(Buff buff, GameObject gb)
    {
        Debug.Log("�Ы�");
        GameObject obj =  Instantiate(buffUIPrefab, transform.position, quaternion.identity);
        //�������Prefab���}��
        BuffUIPrefab buffUiPrefab = obj.GetComponent<BuffUIPrefab>();
        obj.transform.SetParent(buffUIGrid.transform);
        obj.GetComponent<RectTransform>().localScale = Vector3.one;
        //�R���C����Prefab����
        if (buffPrefabList[buff.buffID] != null)
        {
            Destroy(buffPrefabList[buff.buffID].gameObject);
        }
        //�A�N�s���[�J
        buffPrefabList[buff.buffID] = obj;
        //�}�l�ʵe
        buffUiPrefab.StartPlayerFadeAnime(buff.buffTime, buff, gb.GetComponent<buffEffect>());
    }
}

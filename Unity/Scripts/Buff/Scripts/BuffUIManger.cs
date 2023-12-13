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
        Debug.Log("創建");
        GameObject obj =  Instantiate(buffUIPrefab, transform.position, quaternion.identity);
        //獲取控制Prefab的腳本
        BuffUIPrefab buffUiPrefab = obj.GetComponent<BuffUIPrefab>();
        obj.transform.SetParent(buffUIGrid.transform);
        obj.GetComponent<RectTransform>().localScale = Vector3.one;
        //刪除列表內的Prefab物件
        if (buffPrefabList[buff.buffID] != null)
        {
            Destroy(buffPrefabList[buff.buffID].gameObject);
        }
        //再將新的加入
        buffPrefabList[buff.buffID] = obj;
        //開始動畫
        buffUiPrefab.StartPlayerFadeAnime(buff.buffTime, buff, gb.GetComponent<buffEffect>());
    }
}

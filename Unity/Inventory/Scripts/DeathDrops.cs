using Events;
using UnityEngine;

public class DeathDrops : MonoBehaviour
{
    Damageable damageable;
    private bool isDrops;
    public int[] itemID;
    [Header(" Vector2 : x掉落機率，0~100，y獲取物品數量。")]
    public Vector2[] dropsChance;
    [Header("x,y為掉落初始位置偏移(Vector2)， z值是掉落延遲時間。")]
    [Space(15)]
    public Vector3 dropOffsetAndDelayTime;
    private void Awake()
    {
        damageable = GetComponent<Damageable>();
    }
    private void Update()
    {
        if (damageable != null)
        {
            if (!isDrops && !damageable.IsAlive)
            {
                for (int i = 0; i < itemID.Length; i++)
                {
                    ItemEvents.
                        ItemDropsWorld.Invoke(gameObject.transform.position,dropsChance[i],itemID[i],dropOffsetAndDelayTime);
                    isDrops = true;
                }
            }
        }
    }
}
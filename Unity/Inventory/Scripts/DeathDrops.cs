using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Events;
public class DeathDrops : MonoBehaviour
{
    Damageable damageable;
    private bool isDrops;
    public int[] itemID;
    public float[] dropsChance;
    [Header("x,y��������l��m����(Vector2)�A z�ȬO��������ɶ��C")]
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
                    ItemDropsEvents.itemDropsWorld.Invoke(gameObject.transform.position,dropsChance[i],itemID[i],dropOffsetAndDelayTime);
                    isDrops = true;
                }
            }
        }
    }
}
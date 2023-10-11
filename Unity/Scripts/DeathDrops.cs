using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDrops : MonoBehaviour
{
    Damageable damageable;

    private bool isDrops;

    public GameObject[] potionPreFab;
    public float[] Drops_item_probability;

    private GameObject Prefab;
    public float potionPreFab_Offset_Y = -3.0f;
    public float org_prefabspeed_Time = 0f;
    private float prefabspeed_Time = 1f;
    private bool RorL;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
    }

    void Update()
    {
        if (!damageable.IsAlive)
        {
            Invoke("Drops", 0.8f);
            org_prefabspeed_Time += Time.deltaTime;
            Prefab_ON();
        }
    }

    void Prefab_ON()
    {
        RorL = Random.Range(0f, 1f) >= 0.5f;
        if (Prefab != null)
            if (org_prefabspeed_Time < prefabspeed_Time)
                if(RorL)
                    Prefab.transform.position = new Vector2
                    (gameObject.transform.position.x + org_prefabspeed_Time, gameObject.transform.position.y + potionPreFab_Offset_Y + org_prefabspeed_Time);
                else
                    Prefab.transform.position = new Vector2
                    (gameObject.transform.position.x - org_prefabspeed_Time, gameObject.transform.position.y + potionPreFab_Offset_Y + org_prefabspeed_Time);
    }
    private void Drops()
    {
        if (!isDrops)
        {
            for(int i = 0; i <potionPreFab.Length;i++)
            {
                float x = Random.Range(0f, 1f);
                float y = Drops_item_probability[i] / 100f;
                isDrops = true;
                Debug.Log(gameObject + "¤w¦º¤`    " + x +"    " + y + "    " + (x <= y));
                if(x <= y)
                    Prefab = Instantiate(potionPreFab[i], gameObject.transform.position, Quaternion.identity);
            }
        }
    }
}

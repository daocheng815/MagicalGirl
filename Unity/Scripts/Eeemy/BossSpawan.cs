using System;
using Events;
using UnityEngine;


namespace Eeemy
{
    public class BossSpawan : MonoBehaviour
    {
        public GameObject boss;
        public GameObject t;
        public GameObject myboss;
        public GameObject wall;
        public GameObject z;
        public GameObject brokenR;
        public GameObject brokenL;
        [SerializeField]private AVGSystem avgSystem;
        public void OnSpawan()
        {
            Debug.Log("觸發開關");
            if (myboss == null)
            {
                Debug.Log("檢查物品");
                if (invventoryManger.Instance.ItemExistenceCheckerAllBagNumDel(2, 30))
                {   
                    Debug.Log("產生BOSS");
                    myboss = Instantiate(boss, t.transform.position, Quaternion.identity);
                    myboss.AddComponent<Sairesuu_boss_plot>();
                    myboss.GetComponent<Sairesuu_boss_plot>().wall = wall;
                    myboss.GetComponent<Sairesuu_boss_plot>().avgSystem = avgSystem;
                    myboss.GetComponent<Sairesuu_boss>().brokenR = brokenR;
                    myboss.GetComponent<Sairesuu_boss>().brokenL = brokenL;
                    myboss.SetActive(true);
                    Debug.Log("刪除BOSS以外的物件");
                    Destroy(z.gameObject);
                    Destroy(gameObject);
                }
                else
                {
                    GameMessageEvents.AddMessage("破碎靈魂不夠，請戴上30個靈魂碎片，這樣才能召喚出塞蕾絲，請先去刷破碎靈魂", 5f);
                }
            }
        }

        private void Update()
        {
            if (myboss != null)
            {
                if (!myboss.GetComponent<Damageable>().IsAlive)
                    myboss = null;
            }
            
        }
    }
}
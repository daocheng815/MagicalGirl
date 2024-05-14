using System;
using DG.Tweening;
using Events;
using SaveLord;
using UnityEngine;
using UnityEngine.Events;

namespace PlayAction
{
    public class archerytarget : MonoBehaviour,ILordInterface
    {
        private Damageable damageable;
        private SpriteRenderer sr;
        [SerializeField]private GameObject ga;
        [SerializeField] private UnityEvent t;
        public bool successArcherytarget
        {
            get => EventRecordManger.Instance.GetBoolVal("success_archerytarget",false);
            set => EventRecordManger.Instance.SetBoolVal("success_archerytarget", value);
        }
        private void Awake()
        {
            damageable = GetComponent<Damageable>();
            sr = GetComponent<SpriteRenderer>();
        }
        
        private void Update()
        {
            if (!damageable.IsAlive)
            {
                DOTween.To(() => sr.color, x => sr.color = x, new Color(sr.color.r, sr.color.g, sr.color.b, 0f), 1f).OnComplete(
                    () =>
                    {
                        successArcherytarget = true;
                        Destroy(gameObject);
                        Destroy(ga);
                        t.Invoke();
                        GameMessageEvents.AddMessage("障礙物已經毀壞，可以通過。", 5f);
                    });
            }
        }

        public void Init()
        {
            if (EventRecordManger.Instance.GetBoolVal("success_archerytarget", false))
            {
                Destroy(gameObject);
            }
        }
    }
}
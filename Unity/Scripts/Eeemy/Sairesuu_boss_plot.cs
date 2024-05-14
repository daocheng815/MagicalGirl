using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Eeemy
{
    public class Sairesuu_boss_plot : MonoBehaviour
    {
        public GameObject wall;
        private Sairesuu_boss _sairesuuBoss;
        public AVGSystem avgSystem;
        private void Awake()
        {
            _sairesuuBoss = GetComponent<Sairesuu_boss>();
        }

        private Coroutine _waitEnd;
        
        public void Start()
        {
            Debug.Log("觸發劇情");
            _sairesuuBoss.enabled = false;
            avgSystem.OnDramticEmotion("Sairesuu_1");
            _waitEnd = StartCoroutine(WaitEnd());
        }

        private void OnEnable()
        {
            if(_waitEnd != null)
                StopCoroutine(_waitEnd);
        }

        private IEnumerator WaitEnd()
        {
            yield return new WaitUntil(() => avgSystem.sairesuu);
            _sairesuuBoss.enabled = true;
            
            wall.SetActive(true);
            WallFade(1);
        }

        public void  WallFade(float n)
        {
            var t = wall.GetComponent<Tilemap>();
            var c = t.color;
            var color = new Color(c.r, c.g, c.b, 0f);
            t.color = color;
            DOTween.To(() => t.color, x => t.color = x, new Color(c.r, c.g, c.b, n), 1f).OnComplete((() =>
            {
               if(n == 0)
                   wall.SetActive(false);
            }));
        }
    }
}
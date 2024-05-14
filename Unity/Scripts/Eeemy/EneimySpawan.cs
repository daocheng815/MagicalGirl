using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Eeemy
{
    public class EneimySpawan : MonoBehaviour
    {
        private BoxCollider2D _boxCollider2D;
        [SerializeField]private CullingObjectPooling cullingObjectPooling;
        
        [FormerlySerializedAs("_range1")] [SerializeField]private Vector2 range1;
        [FormerlySerializedAs("_range2")] [SerializeField]private Vector2 range2;

        public GameObject ememys;
        public List<GameObject> ememyPool = new List<GameObject>();
        public bool emeyPoolNull => ememyPool.All(element => element == null);
        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            var bounds = _boxCollider2D.bounds;
            var size = _boxCollider2D.size;
            var xsize = size.x*0.5f;
            var ysize = size.y*0.5f;
            range1 = new Vector2(bounds.center.x-xsize, bounds.center.y-ysize);
            range2 = new Vector2(bounds.center.x+xsize, bounds.center.y+ysize);
        }

        private void Start()
        {
            SpawanEnemy();
            SpawanEnemy();
            SpawanEnemy();
        }

        private void SpawanEnemy()
        {
            GameObject e = Instantiate(ememys, (Vector3)RandomVector2(), Quaternion.identity);
            e.transform.parent = transform;
            ememyPool.Add(e);
            cullingObjectPooling.AddPool(e);
        }

        [SerializeField]private float delayTime = 5f;
        [SerializeField]private float nowDelayTime;
        private void Update()
        {
            if (emeyPoolNull)
            {
                nowDelayTime += Time.deltaTime;
                if (nowDelayTime >= delayTime)
                {
                    nowDelayTime = 0f;
                    ememyPool.Clear();
                    SpawanEnemy();
                    SpawanEnemy();
                    SpawanEnemy();
                }
            }
        }

        private Vector3 RandomVector2()
        {
            var r1 = Random.Range(range1.x, range2.x);
            var r2 = Random.Range(range1.y, range2.y);
            return new Vector2(r1, r2);
        }
        
    }
}
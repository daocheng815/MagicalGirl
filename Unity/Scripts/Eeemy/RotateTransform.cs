using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class RotateTransform : MonoBehaviour
{
    //劍環繞某個中心點
        private Animator _animator;
        private Damageable _damageable;
        public Transform centerObject; // 物体中心
        private SpriteRenderer _sr;
        
        public float rotationSpeed = 45f; // 旋转速度
        public float radius = 2f; // 绕物体旋转的半径
        public float initialRotation = 0f; // 初始旋转角度
        public float initAngle = 0f;
        public bool isRotating = true; // 控制旋转的 bool 变量
        public float stopTime = 0f;
        private float angle;
        public bool surround = true;

        public bool isExist = false;
        

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _damageable = GetComponent<Damageable>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            // 设置劍的初始旋转角度
            transform.rotation = Quaternion.Euler(0f, 0f, initialRotation);
        }

        private void Update()
        {
            if (!_damageable.IsAlive && !isExist)
            {
                Destroys();
                
            }
            if(isExist)
                return;
            if (surround)
            {
                Surround();
            }
        }

        private void Surround()
        {
            if (isRotating)
            {
                // 计算剑的新位置
                angle = ((Time.time-stopTime) * rotationSpeed * 360f) % 360f;
                angle += initAngle; // 取余数操作，限制在 0 到 360 度之间
                Vector3 offset = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * radius, Mathf.Sin(Mathf.Deg2Rad * angle) * radius, 0f);
                transform.position = centerObject.position + offset;

                // 根据剑的当前位置动态改变角度
                float swordAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, swordAngle + initialRotation);
            }
            else
            {
                stopTime += Time.deltaTime;
            }
        }

        public void OnAttack(int a0)
        {
            if (!surround)
            {
                Destroys();
            }
        }

        public void OnBroken()
        {
            if(!_damageable.IsAlive)
                Destroys();
        }
        // 可以在其他地方设置 isRotating 的值，来控制旋转的开始和停止

        public void Destroys()
        {
            DOTween.To(() => _sr.color, x => _sr.color = x, new Color(0, 0, 0, 0), 0.5f);
            isExist = true;
            Destroy(gameObject,0.5f);
        }
}

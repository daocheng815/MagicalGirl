using UnityEngine;
using UnityEngine.Events;

namespace PlayAction
{
    public class LRColAction : MonoBehaviour
    {
        public UnityEvent rEvent;
        public UnityEvent lEvent;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player"))
                return;
            Vector2 otherObjectPosition = collision.transform.position;
            Vector2 thisObjectPosition = transform.position;
            if (otherObjectPosition.x > thisObjectPosition.x)
            {
                Debug.Log("從右側進入");
                rEvent.Invoke();
            }
            else if (otherObjectPosition.x < thisObjectPosition.x)
            {
                // ?左??入
                Debug.Log("從左側進入");
                lEvent.Invoke();
            }
        }
    }
}
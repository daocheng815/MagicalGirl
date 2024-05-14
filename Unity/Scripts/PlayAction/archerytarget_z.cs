using SaveLord;
using UnityEngine;
using UnityEngine.Events;

namespace PlayAction
{
    public class archerytarget_z : MonoBehaviour,IPlayAction,ILordInterface
    {
        public UnityEvent v1;
        public UnityEvent v2;
        public bool locks = false;
        public bool LockAction { get; set; }
        public int ActionCount { get; set; }
        public void Action()
        {
            if(!locks)
                v1.Invoke();
        }

        public void OnT()
        {
            locks = false;
            v2.Invoke();
            Destroy(gameObject);
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
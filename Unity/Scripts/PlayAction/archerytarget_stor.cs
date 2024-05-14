using SaveLord;
using UnityEngine;

namespace PlayAction
{
    public class archerytarget_stor : MonoBehaviour,ILordInterface
    {
        public void Init()
        {
            if (EventRecordManger.Instance.GetBoolVal("success_archerytarget", false))
            {
                Destroy(gameObject);
            }
        }
    }
}
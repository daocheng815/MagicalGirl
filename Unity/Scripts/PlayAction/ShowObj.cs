using UnityEngine;

namespace PlayAction
{
    public class ShowObj : MonoBehaviour,IPlayAction
    {
        public bool LockAction { get; set; }
        public int ActionCount { get; set; }
        public GameObject[] objs;
        public bool s;
        public void Action()
        {
            foreach (var g in objs)
            {
                g.SetActive(s);
            }
        }
    }
}
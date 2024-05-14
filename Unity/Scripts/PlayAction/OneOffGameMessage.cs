using Events;
using UnityEngine;

namespace PlayAction
{
    public class OneOffGameMessage : MonoBehaviour,IPlayAction
    {
        public bool LockAction { get; set; }
        public int ActionCount { get; set; }
        public float time =  5f;
        public string text = "";
        public void Action()
        {
            GameMessageEvents.AddMessage(text,time);
            gameObject.SetActive(false);
        }
    }
}
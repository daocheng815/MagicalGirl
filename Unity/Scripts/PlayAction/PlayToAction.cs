using System;
using UnityEngine;

namespace PlayAction
{
    public class PlayToAction : MonoBehaviour
    {
        private IPlayAction[] _playActions;
        private void Awake()
        {
            _playActions = gameObject.GetComponents<IPlayAction>();
        }

        public void Action()
        {
            foreach (var g in _playActions)
            {
                if (!g.LockAction)
                {
                    g.ActionCount += 1;
                }
                g.Action();
            }
        }
    }
}
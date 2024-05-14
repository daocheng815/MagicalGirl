using UnityEngine;

namespace PlayAction
{
    public class PlayTransmit : MonoBehaviour
    {
        public Transform t;
        public Transform player;
        public float delayTime = 0.5f;
        public void OnTransmit() { Invoke(nameof(Ot),delayTime); }
        private void Ot() { player.position = t.position; }
    }
}
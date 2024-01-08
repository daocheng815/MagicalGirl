using UnityEngine;

namespace Daocheng.BehaviorTrees
{
    /// <summary>
    /// 行為樹
    /// </summary>
    [RequireComponent(typeof(Blockboard))]
    public class BehaviorTree : MonoBehaviour
    {
        private Node _root = null;

        public Node Root
        {
            get => _root; 
            protected set => _root = value;
        }

        private Blockboard _blackboard = new Blockboard();
        public Blockboard Blackboard
        {
            get => _blackboard;
            set => _blackboard = value;
        }
        
        private void Awake()
        {
            OnSetup();
        }
        
        private void Update()
        {
            _root?.Evaluate(gameObject.transform, _blackboard);
            OnUpdate();
        }

        /// <summary>
        /// 虛函數，重載
        /// </summary>
        protected virtual void OnSetup() { }
        protected virtual void OnUpdate() { }
    }
}

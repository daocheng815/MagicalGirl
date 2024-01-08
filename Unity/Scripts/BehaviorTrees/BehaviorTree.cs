using UnityEngine;

namespace Daocheng.BehaviorTrees
{
    /// <summary>
    /// �欰��
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
        /// ���ơA����
        /// </summary>
        protected virtual void OnSetup() { }
        protected virtual void OnUpdate() { }
    }
}

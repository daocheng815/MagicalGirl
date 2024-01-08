using UnityEngine;

namespace Daocheng.BehaviorTrees.Decorator
{
    public class LimitedExecutionNode:Decorator
    {
        private float _executionInterval = 5f;  // 限制為每5秒執行一次
        private float _lastExecutionTime;

        public LimitedExecutionNode(float executionInterval)
        {
            _executionInterval = executionInterval;
            _lastExecutionTime = -_executionInterval;  // 初始為負數，確保在開始時可以立即執行
        }

        protected override Status OnEvaluate(Transform agent, Blockboard blackboard)
        {
            float currentTime = Time.time;

            if (currentTime - _lastExecutionTime >= _executionInterval)
            {
                // 在限制的時間間隔內執行
                _lastExecutionTime = currentTime;
                Debug.Log("Node executed!");
                return Status.Success;
            }

            // 未達到執行條件
            return Status.Failure;
        }
    }
}
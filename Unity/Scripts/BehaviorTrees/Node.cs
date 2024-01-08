using System.Collections.Generic;
using UnityEngine;

namespace Daocheng.BehaviorTrees
{
    public enum Status
    {
        Failure = 0,
        Success,
        Running
    }
    public abstract class Node
    {
        private Node parent;
        protected Node[] children;
        
        private Status status;
        public Status Status
        {
            get => status;
            protected set => status = value;
        }
        public Blockboard Blackboard { get; set; }

        public Status Evaluate(Transform agent,Blockboard blackboard)
        {
            Blackboard = blackboard;
            status = OnEvaluate(agent,blackboard);
            //Debug.Log($"{GetType().Name} - Entered...");
            //Debug.Log($"{GetType().Name} - {status}");
            //Debug.Log($"{GetType().Name} - Exited...");
            return status;
        }
        protected abstract Status OnEvaluate(Transform agent,Blockboard blackboard);
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Daocheng.BehaviorTrees
{
    public class FindRandomPath:Task
    {
        private bool ObtainPath
        {
            get => Blackboard.Get<bool>(Names.ObtainPath);
            set => Blackboard.Set(Names.ObtainPath,value);
        }
        private List<Vector2> PathNodes => Blackboard.Get<List<Vector2>>(Names.PathNodes);
        private readonly int _zMax, _zMin;
        public FindRandomPath(int zMax, int zMin)
        {
            _zMax = zMax;
            _zMin = zMin;
        }
        protected override Status OnEvaluate(Transform agent, Blockboard blackboard)
        {
            if (!ObtainPath)
            {
                Blackboard.Set(Names.CurrentIndex,0);
                PathNodes.Clear();
                ObtainPath = true;
                var node = (Vector2)SetAstartTileMap.Instance.RandomNotStopNode(agent.position, _zMax, _zMin);
                PathNodes.Add(node);
                return Status.Success;
            }
            return Status.Failure;
        }
    }
}
using System;
using System.Collections.Generic;

using UnityEngine;
namespace Daocheng.BehaviorTrees
{
    public class FindPathTask : Task
    {
        private bool ObtainPath
        {
            get => Blackboard.Get<bool>(Names.ObtainPath);
            set => Blackboard.Set(Names.ObtainPath,value);
        }
        private readonly Func<Vector3> _start,_end;
        private readonly bool _randomEndNode,_nodeStop;
        private readonly int _zMax,_zMin;
        public FindPathTask(Func<Vector3> start,Func<Vector3> end,int zMax,int zMin,bool randomEndNode = false ,bool nodeStop = false)
        {
            _start = start;
            _end = end;
            _randomEndNode = randomEndNode;
            _nodeStop = nodeStop;
            _zMax = zMax;
            _zMin = zMin;
        }
        protected override Status OnEvaluate(Transform agent, Blockboard blackboard)
        {
            if (!ObtainPath)
            {
                Debug.Log("尋找路徑");
                Blackboard.Set(Names.CurrentIndex,0);
                ObtainPath = true;
                var start = _start.Invoke();
                var end = _randomEndNode? SetAstartTileMap.Instance.RandomNotStopNode(_end.Invoke(), _zMax, _zMin,_nodeStop):_end.Invoke();
                if (end == null)
                    return Status.Failure;
                var nodes = SetAstartTileMap.Instance.NodesCtoW(
                    SetAstartTileMap.Instance.FastFindPath(start,(Vector3)end)
                );
                Blackboard.Set(Names.PathNodes,nodes);
                return Status.Success;
            }
            Debug.Log("已有路徑");
            if (Blackboard.Get<List<Vector2>>(Names.PathNodes) == null)
                ObtainPath = false;
            return Status.Failure;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
namespace Daocheng.BehaviorTrees
{
    public class PathMoveTask :  Task 
    {
        // 當前節點
        private int CurrentInddex
        {
            get => Blackboard.Get<int>(Names.CurrentIndex);
            set => Blackboard.Set(Names.CurrentIndex, value);
        }
        // 巡路節點
        private List<Vector2> PathNodes => Blackboard.Get<List<Vector2>>(Names.PathNodes);
        private Move Move => Blackboard.Get<Move>(Names.Move);
        protected override Status OnEvaluate(Transform agent, Blockboard blackboard)
        {
            var position = agent.position;
            for (int i = 0; i < PathNodes.Count-1; i++)
            {
                Vector3 direction = PathNodes[i + 1] - PathNodes[i];
                Debug.DrawRay(PathNodes[i],direction,Color.red);
            }

            if (CurrentInddex >= 0 && CurrentInddex < PathNodes.Count)
            {
                Move.MoveDirection = (PathNodes[CurrentInddex] - (Vector2)position).normalized;
                //尋找下個節點
                bool nextNode = Vector2.Distance(position, PathNodes[CurrentInddex]) < 0.1f;
                if (nextNode)
                {
                    // Debug.Log($"{CurrentInddex}  {_pathNodes.Count}");
                    if (CurrentInddex == PathNodes.Count-1)
                    {
                        Move.MoveDirection = Vector2.zero;
                        Debug.Log("巡路結束");
                        CurrentInddex = 0;
                        Blackboard.Set(Names.ObtainPath,false);
                        return Status.Success;
                    }
                    CurrentInddex ++;
                }
                return Status.Running;
            }

            CurrentInddex = 0;
            Debug.Log($"錯誤 CurrentInddex:{CurrentInddex} PathNodes.Count:{PathNodes.Count}");
            return Status.Failure;
        }
    }
}
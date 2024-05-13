using System;
using System.Collections;
using UnityEngine;

namespace Behavior_Tree
{
    public class ActionNode : Node
    {
        public Func<NodeState> action;//使用返回节点状态执行行为

        public ActionNode(Func<NodeState> action)
        {
            this.action = action;
        }

        public override NodeState Evaluate()
        {
            if(action == null)
            {
                return NodeState.Failure;
            }

            switch (action.Invoke())
            {
                case NodeState.Success:
                    return NodeState.Success;
                case NodeState.Failure:
                    return NodeState.Failure;
                case NodeState.Running:
                    return NodeState.Running;
            }

            return NodeState.Success;

        }
    }
}
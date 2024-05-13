using System;
using System.Collections;
using UnityEngine;

namespace Behavior_Tree
{
    public class ConditionNode : Node
    {
        public Func<bool> action;//无参有返回值的委托

        public ConditionNode(Func<bool> action)
        {
            this.action = action;
        }

        public override NodeState Evaluate()
        {
            if(action == null)
            {
                return NodeState.Failure;
            }

            return action.Invoke() ? NodeState.Success : NodeState.Failure;
        }


    }
}
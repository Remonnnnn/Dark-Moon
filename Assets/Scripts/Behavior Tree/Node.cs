using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior_Tree
{
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }
    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children=new List<Node>();

        private Dictionary<string,object>_dataContext=new Dictionary<string, object>();//���ڴ洢�ڵ�������

        public Node()
        {
            parent = null;
        }
        
        public Node(List<Node> children)
        {
            foreach(Node child in children)
            {
                _Attach(child);
            }
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual NodeState Evaluate() => NodeState.Failure;

        public void SetDate(string key,object value)//�洢����
        {
            _dataContext[key] = value;
        }
        public object GetData(string key)//��ȡ�ض�����
        {
            object value = null;
            if(_dataContext.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = parent;
            while(node != null)
            {
                value=node.GetData(key);
                if(value!=null)
                {
                    return value;
                }
                node = node.parent;
            }

            return null;
        }

        public bool ClearData(string key)//����ض�����
        {
            if(_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool cleared=node.ClearData(key);
                if (cleared)
                {
                    return true;
                }
                node = node.parent;
            }

            return false;
        }


    }
}


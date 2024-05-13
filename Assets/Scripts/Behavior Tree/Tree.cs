using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behavior_Tree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node root= null;

        protected virtual void Awake()
        {
            
        }
        protected void Start()
        {
            root = SetupTree();
        }

        protected void Update()
        {
            if(root != null)
            {
                root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
    }
}


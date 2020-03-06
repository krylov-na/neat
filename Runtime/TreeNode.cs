﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Neat
{
    public enum TreeNodeState
    {
        Created,
        Mounted
    }

    public abstract class TreeNode
    {
        public abstract Type PType { get; }
        public abstract Node Render();
        public abstract void Update();
        public abstract void Mount(GameObject root);
        public abstract void Unmount();
        public abstract GameObject GetGO(GameObject root);

        public TreeNode child;
    }

    public abstract class TreeNode<Props> : TreeNode
    {
        public Props currentProps;
    }

    public class WidgetTreeNode<Props> : TreeNode<Props>
    {
        public Widget<Props> widget;
        public override Type PType
        {
            get { return typeof(Props); }
        }

        public override Node Render()
        {
            return widget.Render(currentProps);
        }

        public override void Update() { }

        public override void Mount(GameObject root)
        {
            widget.OnComponentDidMount(currentProps);
        }

        public override void Unmount()
        {
            widget.OnComponentWillUnmount(currentProps);
        }

        public override GameObject GetGO(GameObject root)
        {
            return root;
        }
    }

    public class NativeTreeNode<Props> : TreeNode<Props>
    {
        public GameObject go;
        public NativeWidget<Props> widget;
        public override Type PType
        {
            get { return typeof(Props); }
        }

        public override Node Render()
        {
            return widget.Render(currentProps);
        }

        public override void Update() {
            widget.Update(currentProps);
        }

        public override void Mount(GameObject root)
        {
            go = new GameObject();
            go.transform.SetParent(root.transform);
            widget.OnComponentDidMount(go, currentProps);
        }

        public override void Unmount()
        {
            widget.OnComponentWillUnmount(go, currentProps);
        }

        public override GameObject GetGO(GameObject root)
        {
            return go;
        }
    }

    public class FragTreeNode : WidgetTreeNode<Frag>
    {
        public TreeNode[] children;
    }
}
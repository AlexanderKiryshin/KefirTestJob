using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeNode<TValue>
{
    #region Properties
    public TValue Value { get; set; }
    public List<TreeNode<TValue>> Childrens { get; private set; }
    public List<TreeNode<TValue>> Parents { get;  set; }
    public bool HasChild { get { return Childrens.Any(); } }
    #endregion
    #region Constructor
    public TreeNode()
    {
        Childrens = new List<TreeNode<TValue>>();
        Parents = new List<TreeNode<TValue>>();
    }
    public TreeNode(TValue value)
        : this()
    {
        Value = value;
    }
    #endregion
    #region Methods
    public void AddChild(TreeNode<TValue> treeNode)
    {
        treeNode.AddParent(this);
        Childrens.Add(treeNode);        
    }

    public void AddParent(TreeNode<TValue> treeNode)
    {
        Parents.Add(treeNode);
    }
    public TreeNode<TValue> AddChild(TValue value)
    {
        var treeNode = new TreeNode<TValue>(value);
        AddChild(treeNode);
        return treeNode;
    }

    public void AddExistingNode(TreeNode<TValue> treeNode)
    {
        AddChild(treeNode);
    }
    #endregion
}

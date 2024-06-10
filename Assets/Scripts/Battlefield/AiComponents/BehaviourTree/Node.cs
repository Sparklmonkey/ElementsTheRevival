using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Node 
{
    public enum Status {Success, Running, Failure}

    public readonly string NodeName;

    public List<Node> ChildNodes = new();
    protected int CurrentChild;

    public Node(string nodeName = "Node")
    {
        NodeName = nodeName;
    }

    public void AddChild(Node child) => ChildNodes.Add(child);
    public virtual Status Process() => ChildNodes[CurrentChild].Process();

    public virtual void Reset()
    {
        CurrentChild = 0;
        foreach (var child in ChildNodes)
        {
            child.Reset();
        }
    }
}
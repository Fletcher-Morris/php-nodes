using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Node
{
    public string nodeName;
    public int nodeId;
    public string tag;
    public bool isFunction = false;
    public List<NodeConnection> inConnections;
    public List<NodeConnection> outConnections;
    public NodeObject nodeObject;
    public int width = 150;
    public int height = 150;

    public abstract void Setup();
    public void SetObject(NodeObject _obj)
    {
        nodeObject = _obj;
    }
    public abstract bool Valid();
    public abstract void Serialize();
}
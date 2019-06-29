﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Node
{
    public string nodeName;
    public List<NodeConnection> inConnections;
    public List<NodeConnection> outConnections;
    public NodeObject nodeObject;
    public int width;

    public abstract void Setup();
    public void SetObject(NodeObject _obj)
    {
        nodeObject = _obj;
    }
    public abstract bool Valid();
    public abstract void Serialize();
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{    
    public static int STATIC_NODE_ID;

    public static Color ConnectionColor(DataType _type)
    {
        switch(_type)
        {
            case DataType.StringType:
                return Color.white;
            case DataType.IntType:
                return Color.blue;
            case DataType.BoolType:
                return Color.green;
            case DataType.FloatType:
                return Color.cyan;
            case DataType.ClassType:
                return Color.red;
            case DataType.ConType:
                return Color.yellow;
        }
        return Color.gray;
    }
}

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
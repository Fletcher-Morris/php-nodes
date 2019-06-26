using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeConnection
{
    public string connectionName;
    public Node connectionNode;
    public bool isOutput;
    public NodeConnection linkedConnection;
    public DataType dataType;
    public GameObject connectionObject;

    public NodeConnection(Node _node, string _name, bool _output, DataType _type)
    {
        connectionNode = _node;
        connectionName = _name;
        isOutput = _output;
        dataType =_type;
    }
}
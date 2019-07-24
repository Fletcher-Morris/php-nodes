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
    public Linker linker;
    [SerializeField] private int m_uniqueId;

    public int GetConnectorId() { return m_uniqueId; }
    public void SetConnectorId(int _id) { m_uniqueId = _id; }

    public NodeConnection(Node _node, string _name, bool _output, DataType _type)
    {
        m_uniqueId = Global.STATIC_CONNECTOR_ID;
        Global.STATIC_CONNECTOR_ID++;
        connectionNode = _node;
        connectionName = _name;
        isOutput = _output;
        dataType =_type;
    }
}
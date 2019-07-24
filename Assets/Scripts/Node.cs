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
    public List<int> m_preConnections;
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
    public abstract string Serialize();
    public abstract void Deserialize(string _data);

    public void MakePreconnections()
    {
        Debug.Log("Forming Preconnections");
        for (int i = 0; i < inConnections.Count; i++)
        {
            if (i < m_preConnections.Count)
            {
                NodeManager.Singleton.FormLink(inConnections[i].GetConnectorId(), m_preConnections[i]);
            }
            else
            {
                Debug.LogWarning("No PreConnection with id '" + i + "' exists!");
            }
        }
    }

    public void SetPreconnections(List<int> _conns)
    {
        Debug.Log("Setting Preconnections");
        m_preConnections = _conns;
    }
}
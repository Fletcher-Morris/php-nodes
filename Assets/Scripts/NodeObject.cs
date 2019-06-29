using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NodeObject : MonoBehaviour
{
    [SerializeField] private int m_nodeUniqueId;
    public int GetUniqueId(){return m_nodeUniqueId;}
    [SerializeField] private Node m_node;
    public Node GetNode(){return m_node;}
    [SerializeField] private bool m_initialized;

    public List<GameObject> inObjects;
    public List<GameObject> outObjects;

    [SerializeField] private GameObject connectionSprite;
    [SerializeField] private Text nameText;
    [SerializeField] private float connectorSpacing = 0.1f;

    private Vector3 lineOffset;

    private Transform connectorParent;

    public void Init(Node _nodeType)
    {
        m_nodeUniqueId = Global.STATIC_NODE_ID;
        Global.STATIC_NODE_ID++;
        m_node = _nodeType;
        m_node.SetObject(this);
        m_node.Setup();
        gameObject.name = "NODE_" + _nodeType.nodeName;
        nameText.text = _nodeType.nodeName;
        Refresh();
        m_initialized = true;
    }
    public void Init(Node _nodeType, NodeConnection _nodeConnection)
    {
        Init(_nodeType);
        foreach(NodeConnection nodeInput in m_node.inConnections)
        {
            if(nodeInput.dataType == _nodeConnection.dataType)
            {
                _nodeConnection.linkedConnection = nodeInput;
                nodeInput.linkedConnection = _nodeConnection;
                break;
            }
        }
    }

    void Refresh()
    {
        lineOffset = Vector3.zero;
        lineOffset.z = -5.0f;
        connectorParent = transform.Find("Connectors");
        foreach(Transform child in connectorParent)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        int inputs = m_node.inConnections.Count;
        foreach(NodeConnection input in m_node.inConnections)
        {
            GameObject newObject = Instantiate(connectionSprite, connectorParent);
            newObject.name = "Node_" + GetUniqueId() + "_Input_" + i.ToString();
            Vector3 pos = Vector3.zero;
            pos.x = (float)m_node.width * -0.5f;
            pos.y = 75 - (connectorSpacing * i);
            newObject.transform.localPosition = pos;
            input.connectionObject = newObject;
            newObject.GetComponent<Linker>().node = this;
            newObject.GetComponent<Linker>().connection = input;
            inObjects.Add(newObject);
            i++;
        }
        i = 0;
        int outputs = m_node.inConnections.Count;
        foreach(NodeConnection output in m_node.outConnections)
        {
            GameObject newObject = Instantiate(connectionSprite, connectorParent);
            newObject.name = "Node_" + GetUniqueId() + "_Output_" + i.ToString();
            Vector3 pos = Vector3.zero;
            pos.x = (float)m_node.width * 0.5f;
            pos.y = 75 - (connectorSpacing * i);
            newObject.transform.localPosition = pos;
            output.connectionObject = newObject;
            newObject.GetComponent<Linker>().node = this;
            newObject.GetComponent<Linker>().connection = output;
            outObjects.Add(newObject);
            i++;
        }
    }
    
    public void DrawVisual()
    {
        if(m_initialized == false) return;

        
        foreach(NodeConnection input in m_node.inConnections)
        {
            LineRenderer lR = input.connectionObject.GetComponent<LineRenderer>();
            if(input.linkedConnection != null)
            {
                lR.enabled = false;
            }
            else if(input.linker == NodeManager.Singleton.linkingConnection)
            {
                //  Draw Line
                lR.enabled = true;
                lR.startColor = Global.ConnectionColor(input.dataType);
                lR.endColor = Global.ConnectionColor(input.dataType);
                lR.SetPosition(0, input.connectionObject.transform.position + lineOffset);
                lR.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            else
            {
                lR.enabled = false;
            }
        }

        foreach(NodeConnection output in m_node.outConnections)
        {
            LineRenderer lR = output.connectionObject.GetComponent<LineRenderer>();
            if(output.linkedConnection != null)
            {
                //  Draw Lines
                lR.enabled = true;
                lR.startColor = Global.ConnectionColor(output.dataType);
                lR.endColor = Global.ConnectionColor(output.dataType);
                lR.SetPosition(0, output.connectionObject.transform.position + lineOffset);
                lR.SetPosition(1, output.linkedConnection.connectionObject.transform.position + lineOffset);
            }
            else if(output.linker == NodeManager.Singleton.linkingConnection)
            {
                //  Draw Lines
                lR.enabled = true;
                lR.startColor = Global.ConnectionColor(output.dataType);
                lR.endColor = Global.ConnectionColor(output.dataType);
                lR.SetPosition(0, output.connectionObject.transform.position + lineOffset);
                lR.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
            else
            {
                lR.enabled = false;
            }
        }
    }

    public void DeleteNode()
    {
        NodeManager.Singleton.nodeObjects.Remove(this);
        GameObject.Destroy(gameObject);
    }
}

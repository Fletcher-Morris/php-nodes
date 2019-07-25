using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class NodeLink : MonoBehaviour
{

    [Header("Object Stuff")]
    public Image sprite;
    public BezierLine bezier;
    [Header("Data Stuff")]
    public Node node;
    public NodeObject nodeObject;
    public string linkName;
    public bool isOutput;
    public DataType dataType;
    public NodeLink linkedLink;

    [SerializeField] private int m_linkId = -1;
    public void SetLinkId(int _id) { m_linkId = _id; }
    public int GetLinkId() {Debug.Log("Getting Link ID : " + m_linkId); return m_linkId; }


    public void Create(Node _node, string _name, bool _output, DataType _type)
    {
        SetNode(_node);
        linkName = _name;
        isOutput = _output;
        dataType = _type;
    }
    public void SetNode(Node _node)
    {
        SetNode(_node.nodeObject);
    }
    public void SetNode(NodeObject _node)
    {
        node = _node.GetNode();
        nodeObject = _node;
    }


    // Start is called before the first frame update
    void Start()
    {
        bezier = GetComponent<BezierLine>();
        sprite = GetComponent<Image>();
    }

    public void GenId()
    {
        m_linkId = Global.STATIC_LINK_ID;
        Global.STATIC_LINK_ID++;
    }

    public void RefreshColors()
    {
        sprite.color = NodeManager.Singleton.GetTagColor(dataType);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NodeObject : MonoBehaviour
{
    public int GetUniqueId(){return m_node.nodeId;}
    public void SetUniqueId(int newId) { m_node.nodeId = newId; }
    [SerializeField] private Node m_node;
    public Node GetNode(){ return m_node; }
    private bool m_initialized;
    public GameObject linkPrefab;
    public List<Image> headerSprites;
    public List<Image> panelSprites;
    public Text nameText;
    public Button moveButton;
    public Button deleteButton;
    private float connectorSpacing = 0.1f;
    public Vector3 lineOffset;
    public Transform inputTransform;
    public Transform outputTransform;
    public GameObject shadow;
    [Header("UI Prefabs")]
    public GameObject boolUiPrefab;
    public GameObject intUiPrefab;
    public GameObject floatUiPrefab;
    public GameObject stringUiPrefab;

    private void Start()
    {
        if(GetNode().m_preConnections != null)
        {
            if (GetNode().m_preConnections.Count >= 1)
            {
                Refresh();
                GetNode().MakePreconnections();
            }
        }
    }

    public void Init(Node _nodeType, int _id)
    {
        m_node = _nodeType;
        m_node.SetObject(this);
        m_node.Setup();
        if (_id == -1)
        {
            SetUniqueId(Global.STATIC_NODE_ID);
            Global.STATIC_NODE_ID++;
        }
        else
        {
            SetUniqueId(_id);
        }
        gameObject.name = "NODE_" + _nodeType.nodeName;
        nameText.text = _nodeType.nodeName;
        Refresh();
        m_initialized = true;
    }
    public void Init(Node _nodeType)
    {
        Init(_nodeType, -1);
    }

    public void UpdateColors()
    {
        Color headerCol = NodeManager.Singleton.GetTagColor(m_node.tag);
        foreach(Image img in headerSprites)
        {
            img.color = headerCol;
        }
        foreach(NodeLink link in m_node.inLinks)
        {
            link.RefreshColors();
        }
        foreach (NodeLink link in m_node.outLinks)
        {
            link.RefreshColors();
        }
    }

    public void Refresh()
    {
        int i = 0;
        int inputs = m_node.inLinks.Count;
        foreach(NodeLink input in m_node.inLinks)
        {
            input.transform.name = "Node_" + GetUniqueId() + "_Input_" + i.ToString();
            input.transform.GetChild(0).GetComponent<Text>().text = input.linkName;
            Vector3 pos = Vector3.zero;
            input.isOutput = false;
            pos.x = (float)m_node.width * -0.5f;
            input.transform.localPosition = pos;
            input.node = this.m_node;
            i++;
        }
        int o = 0;
        int outputs = m_node.inLinks.Count;
        foreach(NodeLink output in m_node.outLinks)
        {
            output.transform.name = "Node_" + GetUniqueId() + "_Output_" + i.ToString();
            output.transform.GetChild(1).GetComponent<Text>().text = output.linkName;
            Vector3 pos = Vector3.zero;
            output.isOutput = true;
            pos.x = (float)m_node.width * 0.5f;
            output.transform.localPosition = pos;
            output.node = this.m_node;
            o++;
        }
    }
    
    public void DrawVisual()
    {
        if(m_initialized == false) return;

        foreach (Button btn in transform.GetComponentsInChildren<Button>()) { btn.interactable = NodeManager.Singleton.movingNode == null; }
        foreach (InputField field in transform.GetComponentsInChildren<InputField>()) { field.interactable = NodeManager.Singleton.movingNode == null; }
        foreach (Toggle tog in transform.GetComponentsInChildren<Toggle>()) { tog.interactable = NodeManager.Singleton.movingNode == null; }
        foreach (Dropdown drop in transform.GetComponentsInChildren<Dropdown>()) { drop.interactable = NodeManager.Singleton.movingNode == null; }
        foreach (Slider slid in transform.GetComponentsInChildren<Slider>()) { slid.interactable = NodeManager.Singleton.movingNode == null; }
        shadow.SetActive(NodeManager.Singleton.movingNode == this);

        foreach (NodeLink input in m_node.inLinks)
        {
            if (input == NodeManager.Singleton.linkingLink)
            {
                input.bezier.line.enabled = true;
                input.bezier.line.startColor = NodeManager.Singleton.GetTagColor(input.dataType);
                input.bezier.line.endColor = NodeManager.Singleton.GetTagColor(input.dataType);
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                input.bezier.start.x = input.transform.position.x;
                input.bezier.start.y = input.transform.position.y;
                input.bezier.start.z = input.transform.position.z;
                input.bezier.end.x = mousePos.x;
                input.bezier.end.y = mousePos.y;
                input.bezier.end.z = input.transform.position.z;
                input.bezier.UpdatePath();
            }
            else if (input.linkedLink != null)
            {
                input.bezier.line.enabled = true;
                input.bezier.line.endColor = NodeManager.Singleton.GetTagColor(input.dataType);
                input.bezier.line.startColor = NodeManager.Singleton.GetTagColor(input.dataType);
                input.bezier.start.x = input.transform.position.x;
                input.bezier.start.y = input.transform.position.y;
                input.bezier.start.z = input.transform.position.z;
                input.bezier.end.x = input.linkedLink.transform.position.x;
                input.bezier.end.y = input.linkedLink.transform.position.y;
                input.bezier.end.z = input.linkedLink.transform.position.z;
                input.bezier.UpdatePath();
            }
            else
            {
                input.bezier.line.enabled = false;
            }
        }

        foreach (NodeLink output in m_node.outLinks)
        {
            if (output == NodeManager.Singleton.linkingLink)
            {
                //  Draw Lines
                output.bezier.line.enabled = true;
                output.bezier.line.startColor = NodeManager.Singleton.GetTagColor(output.dataType);
                output.bezier.line.endColor = NodeManager.Singleton.GetTagColor(output.dataType);
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                output.bezier.start.x = output.transform.position.x;
                output.bezier.start.y = output.transform.position.y;
                output.bezier.start.z = output.transform.position.z;
                output.bezier.end.x = mousePos.x;
                output.bezier.end.y = mousePos.y;
                output.bezier.end.z = output.transform.position.z;
                output.bezier.UpdatePath();
            }
            else
            {
                output.bezier.line.enabled = false;
            }
        }
    }

    public void DeleteNode()
    {
        NodeManager.Singleton.nodeObjects.Remove(this);
        if (NodeManager.Singleton.autoGenToggle.isOn) NodeManager.Singleton.SaveNodeGraph();
        GameObject.Destroy(gameObject);
    }

    public void MoveNode()
    {
        transform.SetAsLastSibling();
        NodeManager.Singleton.movingNode = this;
    }
}
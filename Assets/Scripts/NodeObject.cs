using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NodeObject : MonoBehaviour
{
    public int GetUniqueId(){return m_node.nodeId;}
    public void SetUniqueId(int newId) { m_node.nodeId = newId; }
    private Node m_node;
    public Node GetNode(){ return m_node; }
    private bool m_initialized;
    public List<GameObject> inObjects;
    public List<GameObject> outObjects;
    [SerializeField] private GameObject connectionSprite;
    public Image header;
    public Text nameText;
    public Button moveButton;
    public Button deleteButton;
    private float connectorSpacing = 0.1f;
    public Vector3 lineOffset;
    public Transform inputTransform;
    public Transform outputTransform;
    public Image shadow;
    [Header("UI Prefabs")]
    public GameObject boolUiPrefab;
    public GameObject intUiPrefab;
    public GameObject floatUiPrefab;
    public GameObject stringUiPrefab;

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

    public void UpdateColors()
    {
        Color headerCol;
        if (m_node.tag == "variable") { headerCol = Global.variableNodeColor; }
        else if (m_node.tag == "sql") { headerCol = Global.sqlNodeColor; }
        else if (m_node.tag == "maths") { headerCol = Global.mathNodeColor; }
        else if (m_node.tag == "php") { headerCol = Global.phpNodeColor; }
        else if (m_node.tag == "string") { headerCol = Global.stringNodeColor; }
        else { headerCol = Color.white; }
        header.color = headerCol;
        foreach(Linker l in )
    }

    void Refresh()
    {
        foreach(Transform child in inputTransform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in outputTransform)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        int inputs = m_node.inConnections.Count;
        foreach(NodeConnection input in m_node.inConnections)
        {
            GameObject newObject = Instantiate(connectionSprite, inputTransform);
            newObject.name = "Node_" + GetUniqueId() + "_Input_" + i.ToString();
            newObject.transform.GetChild(0).GetComponent<Text>().text = input.connectionName;
            Vector3 pos = Vector3.zero;
            pos.x = (float)m_node.width * -0.5f;
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
            GameObject newObject = Instantiate(connectionSprite, outputTransform);
            newObject.name = "Node_" + GetUniqueId() + "_Output_" + i.ToString();
            newObject.transform.GetChild(1).GetComponent<Text>().text = output.connectionName;
            Vector3 pos = Vector3.zero;
            pos.x = (float)m_node.width * 0.5f;
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

        foreach (Button btn in transform.GetComponentsInChildren<Button>()) { btn.interactable = NodeManager.Singleton.movingNode == null; }
        foreach (InputField field in transform.GetComponentsInChildren<InputField>()) { field.interactable = NodeManager.Singleton.movingNode == null; }
        foreach (Toggle tog in transform.GetComponentsInChildren<Toggle>()) { tog.interactable = NodeManager.Singleton.movingNode == null; }
        foreach (Dropdown drop in transform.GetComponentsInChildren<Dropdown>()) { drop.interactable = NodeManager.Singleton.movingNode == null; }
        foreach (Slider slid in transform.GetComponentsInChildren<Slider>()) { slid.interactable = NodeManager.Singleton.movingNode == null; }
        shadow.enabled = NodeManager.Singleton.movingNode == this;

        foreach (NodeConnection input in m_node.inConnections)
        {
            LineRenderer lR = input.connectionObject.GetComponent<LineRenderer>();
            if(input.linkedConnection != null)
            {
                if(input.linkedConnection.connectionObject != null)
                {
                    //  Draw Lines
                    lR.enabled = true;
                    lR.startColor = Global.ConnectionColor(input.linkedConnection.dataType);
                    lR.endColor = Global.ConnectionColor(input.dataType);
                    Vector3 n1 = input.connectionObject.transform.position;
                    n1.z = 0.0f;
                    Vector3 n2 = input.connectionObject.transform.position;
                    if (input.linkedConnection != null) n2 = input.linkedConnection.connectionObject.transform.position;
                    else n2 = input.connectionObject.transform.position;
                    n2.z = 0.0f;
                    lR.SetPosition(0, n2);
                    lR.SetPosition(1, n1);
                }
                else
                {
                    lR.enabled = false;
                }
            }
            else if(input.linker == NodeManager.Singleton.linkingConnection)
            {
                //  Draw Line
                lR.enabled = true;
                lR.startColor = Global.ConnectionColor(input.dataType);
                lR.endColor = Global.ConnectionColor(input.dataType);
                lR.SetPosition(0, input.connectionObject.transform.position + lineOffset);
                Vector3 cP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cP.z = 0.0f;
                lR.SetPosition(1, cP);
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
                lR.enabled = false;
            }
            else if(output.linker == NodeManager.Singleton.linkingConnection)
            {
                //  Draw Lines
                lR.enabled = true;
                lR.startColor = Global.ConnectionColor(output.dataType);
                lR.endColor = Global.ConnectionColor(output.dataType);
                Vector3 nP = output.connectionObject.transform.position;
                nP.z = 0.0f;
                lR.SetPosition(0, nP);
                Vector3 cP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cP.z = 0.0f;
                lR.SetPosition(1, cP);
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
        if (NodeManager.Singleton.autoGenToggle.isOn) NodeManager.Singleton.SaveNodeGraph();
        GameObject.Destroy(gameObject);
    }

    public void MoveNode()
    {
        transform.SetAsLastSibling();
        NodeManager.Singleton.movingNode = this;
    }
}

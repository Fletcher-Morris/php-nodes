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
    public List<GameObject> inObjects;
    public List<GameObject> outObjects;
    [SerializeField] private GameObject connectionSprite;
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
        if(GetNode().m_preConnections.Count >= 1)
        {
            Refresh();
            GetNode().MakePreconnections();
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
        Color headerCol = NodeManager.Singleton.GetTagColor(m_node.tag);
        foreach(Image img in headerSprites)
        {
            img.color = headerCol;
        }
        foreach(GameObject obj in inObjects)
        {
            Linker linker = obj.GetComponent<Linker>();
            linker.UpdateColors();
        }
    }

    public void Refresh()
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
        shadow.SetActive(NodeManager.Singleton.movingNode == this);

        foreach (NodeConnection input in m_node.inConnections)
        {
            if(input.linkedConnection != null)
            {
                if(input.linkedConnection.connectionObject != null)
                {
                    //  Draw Lines
                    input.linker.bezier.line.enabled = true;
                    input.linker.bezier.line.endColor = NodeManager.Singleton.GetTagColor(input.linkedConnection.dataType);
                    input.linker.bezier.line.startColor = NodeManager.Singleton.GetTagColor(input.dataType);

                    input.linker.bezier.start.x = input.connectionObject.transform.position.x;
                    input.linker.bezier.start.y = input.connectionObject.transform.position.y;
                    input.linker.bezier.start.z = input.connectionObject.transform.position.z;

                    input.linker.bezier.end.x = input.linkedConnection.connectionObject.transform.position.x;
                    input.linker.bezier.end.y = input.linkedConnection.connectionObject.transform.position.y;
                    input.linker.bezier.end.z = input.linkedConnection.connectionObject.transform.position.z;

                    input.linker.bezier.UpdatePath();
                }
                else
                {
                    input.linker.bezier.line.enabled = false;
                }
            }
            else if(input.linker == NodeManager.Singleton.linkingConnection && input.linker != null)
            {
                //  Draw Line
                input.linker.bezier.line.enabled = true;
                input.linker.bezier.line.startColor = NodeManager.Singleton.GetTagColor(input.dataType);
                input.linker.bezier.line.endColor = NodeManager.Singleton.GetTagColor(input.dataType);
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                input.linker.bezier.start.x = input.connectionObject.transform.position.x;
                input.linker.bezier.start.y = input.connectionObject.transform.position.y;
                input.linker.bezier.start.z = input.connectionObject.transform.position.z;

                input.linker.bezier.end.x = mousePos.x;
                input.linker.bezier.end.y = mousePos.y;
                input.linker.bezier.end.z = input.connectionObject.transform.position.z;

                input.linker.bezier.UpdatePath();
            }
            else if(input.linker != null)
            {
                input.linker.bezier.line.enabled = false;
            }
        }

        foreach (NodeConnection output in m_node.outConnections)
        {
            if (output.linker == NodeManager.Singleton.linkingConnection && output.linker != null)
            {
                //  Draw Lines
                output.linker.bezier.line.enabled = true;
                output.linker.bezier.line.startColor = NodeManager.Singleton.GetTagColor(output.dataType);
                output.linker.bezier.line.endColor = NodeManager.Singleton.GetTagColor(output.dataType);
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                output.linker.bezier.start.x = output.connectionObject.transform.position.x;
                output.linker.bezier.start.y = output.connectionObject.transform.position.y;
                output.linker.bezier.start.z = output.connectionObject.transform.position.z;

                output.linker.bezier.end.x = mousePos.x;
                output.linker.bezier.end.y = mousePos.y;
                output.linker.bezier.end.z = output.connectionObject.transform.position.z;

                output.linker.bezier.UpdatePath();
            }
            else if (output.linker != null)
            {
                output.linker.bezier.line.enabled = false;
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

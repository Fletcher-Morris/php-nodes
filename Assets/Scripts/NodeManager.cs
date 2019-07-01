using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Singleton;

    public Transform contentArea;
    public ScrollRect scrollView;
    public Transform buttonArea;
    public GameObject nodePrefab;
    public Toggle autoGenToggle;
    public Text generatedNodeGraph;


    public List<NodeObject> nodeObjects;

    public Linker linkingConnection;
    public bool draggingConnection;
    public NodeObject movingNode;

    private int currentZoomLevel = 1;

    public float minConnectorDist = 10.0f;

    public Vector2 mousePos;
    public float safeAreaMin;
    public float safeAreaMax;
    public bool inSafeZone;

    void Start()
    {
        Singleton = this;
        foreach(Text txt in buttonArea.GetComponentsInChildren<Text>())
        {
            txt.text = txt.transform.name;
        }
        foreach(Button btn in buttonArea.GetComponentsInChildren<Button>())
        {
            btn.transform.GetChild(0).GetComponent<Text>().text = btn.transform.name;
            System.Type t = System.Type.GetType("Node_" + btn.transform.name);
            if(t == null)
            {
                btn.interactable = false;
            }
        }
    }

    GameObject GetClosestLinker(Vector3 pos)
    {
        GameObject closest = null;
        float dist = 0.0f;
        foreach(NodeObject obj in nodeObjects)
        {
            foreach(GameObject con in obj.inObjects)
            {
                float d = Vector3.Distance(con.transform.position, pos);
                if(d <= dist || closest == null)
                {
                    closest = con;
                    dist = d;
                }
            }
            foreach(GameObject con in obj.outObjects)
            {
                float d = Vector3.Distance(con.transform.position, pos);
                if(d <= dist || closest == null)
                {
                    closest = con;
                    dist = d;
                }
            }
        }
        if(closest != null) return closest;
        else return null;
    }

    public void FormLink(Linker _a, Linker _b)
    {
        if(_a != null && _b != null)
        {
            if(_a.node == _b.node)
            {
                Debug.Log("Cannot link a node to its self!");
            }
            else if(linkingConnection.connection.isOutput != _b.connection.isOutput)
            {
                if(_a.connection.linkedConnection != null)
                {
                    _a.connection.linkedConnection.linkedConnection = null;
                }
                if(_b.connection.linkedConnection != null)
                {
                    _b.connection.linkedConnection.linkedConnection = null;
                }
                _a.connection.linkedConnection = _b.connection;
                _b.connection.linkedConnection = _a.connection;
                Debug.Log("Linked Nodes " + _a.node.GetUniqueId() + " & " + _b.node.GetUniqueId());
            }
            else
            {
                Debug.Log("Can only link an output to an input!");
            }
        }
        if (autoGenToggle.isOn) SaveNodeGraph();
    }

    public void CreateNode(string _nodeClassName)
    {
        System.Type t = System.Type.GetType(_nodeClassName);
        if(t == null)
        {
            Debug.LogWarning("Could not create node of type '" + _nodeClassName + "'!");
            return;
        }
        GameObject newNode = Instantiate(nodePrefab, contentArea);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newNode.transform.localPosition = pos;
        newNode.GetComponent<NodeObject>().Init((Node)System.Activator.CreateInstance(t));
        nodeObjects.Add(newNode.GetComponent<NodeObject>());
        movingNode = newNode.GetComponent<NodeObject>();
        if (autoGenToggle.isOn) SaveNodeGraph();
    }
    public void CreateNode(Button _button)
    {
        CreateNode("Node_" + _button.transform.name);
    }

    public void AlignToGrid()
    {
        foreach(NodeObject node in nodeObjects)
        {
            node.transform.localPosition = Align(node.transform.localPosition);
        }
        if (autoGenToggle.isOn) SaveNodeGraph();
    }
    public Vector3 Align(Vector3 pos)
    {
        float grid = 150f / 2.0f;
        float scale = 1.0f / grid;
        pos.x *= scale;
        pos.y *= scale;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);
        pos.x *= grid;
        pos.y *= grid;
        return pos;
    }
    public void Recenter()
    {
        StartCoroutine(RecenterCoroutine());
    }
    private IEnumerator RecenterCoroutine()
    {
        Vector3 diff = contentArea.transform.position;
        diff.z = 0.0f;
        while(diff.magnitude >= 0.1f)
        {
            contentArea.transform.localPosition = Vector3.Lerp(contentArea.transform.localPosition, Vector3.zero, Time.deltaTime * 20.0f);
            diff = contentArea.transform.localPosition;
            yield return new WaitForEndOfFrame();
        }
        contentArea.transform.localPosition = Vector3.zero;
        yield return null;
    }

    NodeObject FindNodeObjectById(int _id)
    {
        foreach(NodeObject obj in nodeObjects)
        {
            if (obj.GetUniqueId() == _id) return obj;
        }
        return null;
    }
    Node FindNodeById(int _id)
    {
        NodeObject obj = FindNodeObjectById(_id);
        if (obj != null) return obj.GetNode();
        return null;
    }
    NodeConnection FindConnectionById(int _id)
    {
        foreach (NodeObject obj in nodeObjects)
        {
            foreach(NodeConnection con in obj.GetNode().inConnections)
            {
                if (con.id == _id) return con;
            }
            foreach (NodeConnection con in obj.GetNode().outConnections)
            {
                if (con.id == _id) return con;
            }
        }
        return null;
    }
    Linker FindLinkerById(int _id)
    {
        NodeConnection con = FindConnectionById(_id);
        if (con != null) return con.linker;
        return null;
    }

    void Update()
    {
        inSafeZone = true;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < safeAreaMin) inSafeZone = false;
        if (mousePos.x > safeAreaMax) inSafeZone = false;
        if (movingNode != null) MoveNode();
        else DragConnection();
        scrollView.horizontal = !draggingConnection;
        scrollView.vertical = !draggingConnection;
        foreach(NodeObject node in nodeObjects)
        {
            node.DrawVisual();
        }
    }

    private void MoveNode()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.y += (movingNode.transform.position.y - movingNode.moveButton.transform.position.y);
        newPos.z = 10.0f;
        movingNode.transform.position = Vector3.Lerp(movingNode.transform.position, newPos, 20.0f * Time.deltaTime);
        if (Input.GetMouseButtonDown(0))
        {
            movingNode = null;
            if (autoGenToggle.isOn) SaveNodeGraph();
        }
    }

    private void DragConnection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            draggingConnection = false;
            linkingConnection = null;
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject c = GetClosestLinker(pos);
            if(c != null)
            {
                Linker closest = c.GetComponent<Linker>();
                Vector2 p = closest.transform.position;
                if (Vector3.Distance(p, pos) <= minConnectorDist)
                {
                    linkingConnection = closest;
                    draggingConnection = true;
                    if(closest.connection.linkedConnection != null)
                    {
                        closest.connection.linkedConnection.linkedConnection = null;
                    }
                    closest.connection.linkedConnection = null;
                }
                else
                {
                    linkingConnection = null;
                    draggingConnection = false;
                }
            }
            else
            {
                linkingConnection = null;
                draggingConnection = false;
            }
            if (autoGenToggle.isOn) SaveNodeGraph();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if(linkingConnection != null && draggingConnection == true)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Linker closest = GetClosestLinker(pos).GetComponent<Linker>();
                Vector2 p = closest.transform.position;
                if (Vector3.Distance(p, pos) <= minConnectorDist)
                {
                    FormLink(linkingConnection, closest);
                }
            }
            linkingConnection = null;
            draggingConnection = false;
        }
        else if(Input.GetMouseButton(0) == false)
        {
            draggingConnection = false;
            linkingConnection = null;
        }
    }

    public void ZoomOut()
    {
        currentZoomLevel *= 2;
        currentZoomLevel = Mathf.Clamp(currentZoomLevel, 1, 4);
        StartCoroutine(Zoom((float)(1.0f/(float)currentZoomLevel)));
    }
    public void ZoomIn()
    {
        currentZoomLevel /= 2;
        currentZoomLevel = Mathf.Clamp(currentZoomLevel, 1, 4);
        StartCoroutine(Zoom((float)(1.0f/(float)currentZoomLevel)));
    }
    private IEnumerator Zoom(float scale)
    {
        float diff = Mathf.Abs(contentArea.localScale.x - scale);
        while(diff >= 0.01f)
        {
            float s = Mathf.MoveTowards(contentArea.localScale.x, scale, 5.0f * Time.deltaTime);
            Vector3 newScale = contentArea.localScale;
            newScale.x = s;
            newScale.y = s;
            newScale.z = 1.0f;
            contentArea.localScale = newScale;
            diff = Mathf.Abs(contentArea.localScale.x - scale);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    public void SaveNodeGraph()
    {
        Global.STATIC_NODE_ID = 0;
        string str = "PHP NODE GRAPH VERSION 0.1";
        foreach(NodeObject obj in nodeObjects)
        {
            str += "\nSTART-NODE\n";
            Node node = obj.GetNode();
            node.nodeId = Global.STATIC_NODE_ID;
            Global.STATIC_NODE_ID++;
            str += node.nodeName;
            str += ",";
            str += obj.GetUniqueId();
            str += ",";
            str += obj.transform.localPosition.x;
            str += ",";
            str += obj.transform.localPosition.y;
            str += "\ninputs\n";
            foreach(NodeConnection con in node.inConnections)
            {
                if (con.linkedConnection != null)
                {
                    str += con.linkedConnection.id;
                }
                else
                {
                    str += "-1";
                }
                str += ",";
            }
            str += "\noutputs\n";
            foreach (NodeConnection con in node.outConnections)
            {
                if (con.linkedConnection != null)
                {
                    str += con.linkedConnection.id;
                }
                else
                {
                    str += "-1";
                }
                str += ",";
            }
            str += "END-NODE";
        }
        generatedNodeGraph.text = str;
        Global.CopyToClipboard(str);
        Debug.Log("Saved Node Graph");
    }

    public void CreateGraphFromString(string str)
    {
        string[] lines = str.Split('\n');

        string newNodeName;
        string newNodeType;
        int newNodeId;
        float newNodeXPos;
        float newNodeYPos;


        foreach(string line in lines)
        {
            if(line == "START-NODE")
            {

            }
            else if(line == "END-NODE")
            {

            }
        }
    }
}

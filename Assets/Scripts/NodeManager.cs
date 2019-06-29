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


    public List<NodeObject> nodeObjects;

    public Linker linkingConnection;
    public bool draggingConnection;

    public float minConnectorDist = 10.0f;

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
        Vector3 pos = -contentArea.transform.localPosition;
        bool foundPos = false;
        while(foundPos == false)
        {
            foundPos = true;
            foreach(NodeObject obj in nodeObjects)
            {
                if(pos == obj.transform.localPosition)
                {
                    foundPos = false;
                    pos.x += 20;
                    pos.y -= 20;
                }
            }
        }
        newNode.transform.localPosition = pos;
        newNode.GetComponent<NodeObject>().Init((Node)System.Activator.CreateInstance(t));
        nodeObjects.Add(newNode.GetComponent<NodeObject>());
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
    }
    public Vector3 Align(Vector3 pos)
    {
        pos.x *= 0.5f;
        pos.y *= 0.5f;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);
        pos.x *= 2f;
        pos.y *= 2f;
        return pos;
    }
    public void Recenter()
    {
        StartCoroutine(RecenterCoroutine());
    }
    private IEnumerator RecenterCoroutine()
    {
        Vector3 diff = contentArea.transform.position;
        while(diff.magnitude >= 0.1f)
        {
            contentArea.transform.position = Vector3.Lerp(contentArea.transform.position, Vector3.zero, Time.deltaTime * 20.0f);
            diff = contentArea.transform.position;
            yield return new WaitForEndOfFrame();
        }
        contentArea.transform.position = Vector3.zero;
        yield return null;
    }

    void Update()
    {
        DragConnection();
        scrollView.horizontal = !draggingConnection;
        scrollView.vertical = !draggingConnection;
        foreach(NodeObject node in nodeObjects)
        {
            node.DrawVisual();
        }
    }

    private void DragConnection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            draggingConnection = false;
            linkingConnection = null;
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject c = GetClosestLinker(pos);
            if(c != null)
            {
                Linker closest = c.GetComponent<Linker>();
                if(Vector3.Distance(closest.transform.position, pos) <= minConnectorDist)
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
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if(linkingConnection != null && draggingConnection == true)
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Linker closest = GetClosestLinker(pos).GetComponent<Linker>();
                if(Vector3.Distance(closest.transform.position, pos) <= minConnectorDist)
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

    void OnDrawGizmos()
    {
        if(linkingConnection == null) return;
        Gizmos.DrawIcon(linkingConnection.gameObject.transform.position, "Light Gizmo.tiff", true);
    }
}

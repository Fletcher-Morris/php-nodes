using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Singleton;

    public Transform contentArea;
    public Text helpText;
    public ScrollRect scrollView;
    public Transform buttonArea;
    public GameObject nodePrefab;


    public List<NodeObject> nodeObjects;

    public NodeLink linkingLink;
    public bool draggingLink;
    public NodeObject movingNode;

    private int currentZoomLevel = 1;

    public float minConnectorDist = 10.0f;

    public Vector2 mousePos;
    public float safeAreaMin;
    public float safeAreaMax;
    public bool inSafeZone;

    [Header("Color Scheme")]
    public List<ColorTag> colorTags;

    void Start()
    {
        Singleton = this;
        UpdateGlobalColors();
        foreach(Text txt in buttonArea.GetComponentsInChildren<Text>())
        {
            txt.text = txt.transform.name;
        }
        foreach(Button btn in buttonArea.GetComponentsInChildren<Button>())
        {
            btn.transform.GetChild(0).GetComponent<Text>().text = btn.transform.name;
            System.Type t = System.Type.GetType("Node_" + btn.transform.name.Replace(" ", ""));
            if(t == null)
            {
                btn.interactable = false;
            }
        }
        buttonArea.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonArea.GetComponent<RectTransform>().sizeDelta.x, 50 * buttonArea.childCount);
    }

    public void UpdateGlobalColors()
    {
        helpText.text = "";
        foreach(ColorTag ct in colorTags)
        {
            if (colorTags.IndexOf(ct) != 0) helpText.text += "  ";
            helpText.text += "<color=#" + ColorUtility.ToHtmlStringRGBA(ct.color) + ">";
            helpText.text += ct.tag;
            helpText.text += "</color>";
        }

        foreach (NodeObject obj in nodeObjects)
        {
            obj.UpdateColors();
        }
    }
    public Color GetTagColor(string _tag)
    {
        foreach (ColorTag ct in colorTags)
        {
            if (ct.tag.ToLower() == _tag.ToLower()) { return ct.color; }
            else if((ct.tag + "type").ToLower() == _tag.ToLower()) { return ct.color; }
        }
        return Color.grey;
    }
    public Color GetTagColor(DataType _type)
    {
        return GetTagColor(_type.ToString());
    }

    NodeLink GetClosestNodeLink(Vector3 pos)
    {
        NodeLink closest = null;
        float dist = 0.0f;
        foreach(NodeObject obj in nodeObjects)
        {
            foreach(NodeLink con in obj.GetNode().inLinks)
            {
                float d = Vector3.Distance(con.transform.position, pos);
                if(d <= dist || closest == null)
                {
                    closest = con;
                    dist = d;
                }
            }
            foreach(NodeLink con in obj.GetNode().outLinks)
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

    public void FormLink(NodeLink _a, NodeLink _b)
    {
        if(_a != null && _b != null)
        {
            if(_a.node == _b.node)
            {
                Debug.Log("Cannot link a node to its self!");
            }
            else if(_a.isOutput != _b.isOutput)
            {
                NodeLink _input;
                NodeLink _output;
                if (_a.isOutput) { _input = _b; _output = _a; }
                else { _input = _a; _output = _b; }

                _output.linkedLink = null;
                _input.linkedLink = _output;

                Debug.Log("Linked nodes " + _a.nodeObject.GetUniqueId() + " & " + _b.nodeObject.GetUniqueId());
            }
            else
            {
                Debug.Log("Can only link an output to an input!");
            }
        }
        else
        {
            if (_a == null) Debug.Log("NodeLink a is null!");
            if (_b == null) Debug.Log("NodeLink b is null!");
        }
    }
    public void FormLink(int _a, int _b)
    {
        if (_a == -1 || _b == -1) return;
        FormLink(FindLinkById(_a), FindLinkById(_b));
    }

    public void CreateNode(string _nodeClassName)
    {
        _nodeClassName = _nodeClassName.Replace(" ", "");
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
        UpdateGlobalColors();
        Debug.Log("Created Node Of Type '" + _nodeClassName + "'.");
    }
    public void CreateNode(Button _button)
    {
        CreateNode("Node_" + _button.transform.name);
    }
    public void CreateNode(Vector2 _nodePos, int _nodeFlow, List<int> _nodeInputs, List<string> _nodeData)
    {
        if(_nodeData.Count >= 1)
        {
            CreateNode(_nodeData[0]);
            movingNode = null;
            Node newNode = GetNewestNode();
            newNode.nodeObject.transform.localPosition = new Vector3(_nodePos.x, _nodePos.y, 10.0f);
            newNode.Deserialize(_nodeData);
            if(_nodeInputs.Count >= 1) newNode.SetPreconnections(_nodeInputs);
        }
        else
        {
            Debug.LogWarning("Not Enough Data To Create Node!");
        }
    }
    public Node GetNewestNode()
    {
        return nodeObjects[nodeObjects.Count - 1].GetNode();
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
        float grid = 50f;
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

    public NodeObject FindNodeObjectById(int _id)
    {
        foreach(NodeObject obj in nodeObjects)
        {
            if (obj.GetUniqueId() == _id) return obj;
        }
        return null;
    }
    public Node FindNodeById(int _id)
    {
        NodeObject obj = FindNodeObjectById(_id);
        if (obj != null) return obj.GetNode();
        return null;
    }
    public NodeLink FindLinkById(int _id)
    {
        foreach (NodeObject obj in nodeObjects)
        {
            foreach(NodeLink link in obj.GetNode().inLinks)
            {
                if (link.GetLinkId() == _id)
                {
                    return link;
                }
            }
            foreach (NodeLink link in obj.GetNode().outLinks)
            {
                if (link.GetLinkId() == _id)
                {
                    return link;
                }
            }
        }
        Debug.LogWarning("Could not find link with id '" + _id + "'!");
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
        scrollView.horizontal = !draggingLink;
        scrollView.vertical = !draggingLink;
        foreach(NodeObject node in nodeObjects)
        {
            node.DrawVisual();
        }
    }

    private void MoveNode()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPos.y += (movingNode.transform.position.y - movingNode.moveButton.transform.position.y);
        newPos.z = -5.5f;
        movingNode.transform.position = Vector3.Lerp(movingNode.transform.position, newPos, 20.0f * Time.deltaTime);
        if (Input.GetMouseButtonDown(0))
        {
            movingNode.transform.localPosition = new Vector3(movingNode.transform.localPosition.x, movingNode.transform.localPosition.y, 10.0f);
            movingNode = null;
        }
    }

    private void DragConnection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            draggingLink = false;
            linkingLink = null;
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            NodeLink closest = GetClosestNodeLink(pos);
            if(closest != null)
            {
                Vector2 p = closest.transform.position;
                if (Vector3.Distance(p, pos) <= minConnectorDist)
                {
                    linkingLink = closest;
                    draggingLink = true;
                    if(closest.linkedLink != null)
                    {
                        closest.linkedLink = null;
                    }
                    closest.linkedLink = null;
                }
                else
                {
                    linkingLink = null;
                    draggingLink = false;
                }
            }
            else
            {
                linkingLink = null;
                draggingLink = false;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if(linkingLink != null && draggingLink == true)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                NodeLink closest = GetClosestNodeLink(pos).GetComponent<NodeLink>();
                Vector2 p = closest.transform.position;
                if (Vector3.Distance(p, pos) <= minConnectorDist)
                {
                    FormLink(linkingLink, closest);
                }
            }
            linkingLink = null;
            draggingLink = false;
        }
        else if(Input.GetMouseButton(0) == false)
        {
            draggingLink = false;
            linkingLink = null;
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

    public void ClearGraph()
    {
        StartCoroutine(ClearGraphAnim());
        Global.STATIC_NODE_ID = 0;
        Global.STATIC_LINK_ID = 0;
    }
    private IEnumerator ClearGraphAnim()
    {
        while(nodeObjects.Count > 0)
        {
            nodeObjects[0].DeleteNode();
            yield return new WaitForSeconds(0.025f);
        }
        yield return null;
    }

    public void SaveNodeGraph()
    {
        Global.STATIC_NODE_ID = 0;
        Global.STATIC_LINK_ID = 0;
        string str = "#PHP NODE GRAPH VERSION 0.1#";
        foreach(NodeObject obj in nodeObjects)
        {
            Node node = obj.GetNode();
            node.nodeId = Global.STATIC_NODE_ID;
            Global.STATIC_NODE_ID++;
            str += "\n\n~#START-NODE#~";
            str += "#POS#~";
            str += obj.transform.localPosition.x;
            str += ",";
            str += obj.transform.localPosition.y;
            foreach (NodeLink link in node.inLinks)
            {
                link.SetLinkId(Global.STATIC_LINK_ID);
                Global.STATIC_LINK_ID++;
            }
            foreach (NodeLink link in node.outLinks)
            {
                link.SetLinkId(Global.STATIC_LINK_ID);
                Global.STATIC_LINK_ID++;
            }
            str += "~#INPUTS#~";
            foreach (NodeLink link in node.inLinks)
            {
                if (link.linkedLink != null)
                {
                    str += link.linkedLink.GetLinkId();
                }
                else
                {
                    str += "-1";
                }
                str += ",";
            }
            str += "~#DATA#~";
            str += node.GetType();
            str += "~";
            str += node.Serialize();
            str += "~#END-NODE#~";
        }
        Global.CopyToClipboard(str);
        Debug.Log("Saved Node Graph To Clipboard");
    }

    public void CreateGraphFromClipBoard()
    {
        CreateGraphFromString(Global.PasteFromClipboard());
    }
    public void CreateGraphFromString(string str)
    {
        ClearGraph();
        string[] lines = str.Split('~');

        bool makingNode = false;
        string makingType = "";

        int makingNodeFlow = -1;
        Vector2 makingNodePos = new Vector2();
        List<int> makingNodeInputs = new List<int>();
        List<string> makingNodeData = new List<string>();

        foreach(string line in lines)
        {
            if(line == "#START-NODE#")
            {
                makingNode = true;
                makingType = "";
                makingNodeInputs = new List<int>();
                makingNodeData = new List<string>();
                makingNodeFlow = -1;
            }
            else if(line == "#END-NODE#")
            {
                makingType = "";
                CreateNode(makingNodePos, makingNodeFlow, makingNodeInputs, makingNodeData);
                makingNodeInputs = new List<int>();
                makingNodeData = new List<string>();
                makingNodeFlow = -1;
            }
            else if(makingNode)
            {
                if (line == "#INPUTS#")
                {
                    makingType = "inputs";
                }
                else if (line == "#FLOW#")
                {
                    makingType = "flow";
                }
                else if (line == "#DATA#")
                {
                    makingType = "data";
                }
                else if(line == "#POS#")
                {
                    makingType = "pos";
                }
                else if(makingType == "pos")
                {
                    float x, y;
                    bool isFloat = float.TryParse(line.Split(',')[0], out x);
                    if (!isFloat)
                    {
                        x = 0;
                        Debug.LogWarning("'" + line + "' is not an Float");
                    }
                    isFloat = float.TryParse(line.Split(',')[1], out y);
                    if (!isFloat)
                    {
                        y = 0;
                        Debug.LogWarning("'" + line + "' is not an Float");
                    }
                    makingNodePos = new Vector2(x, y);
                }
                else if(makingType == "inputs")
                {
                    string[] split = line.Split(',');
                    foreach (string c in split)
                    {
                        if(c != "")
                        {
                            int inputInt;
                            bool isInt = int.TryParse(c, out inputInt);
                            if (isInt)
                            {
                                makingNodeInputs.Add(inputInt);
                            }
                            else
                            {
                                Debug.LogWarning("'" + c + "' is not an Integer");
                            }
                        }
                    }
                }
                else if(makingType == "data")
                {
                    makingNodeData.Add(line);
                }
            }
        }
        foreach (NodeObject node in nodeObjects)
        {
            node.GetNode().GenLinkIds();
        }
        foreach (NodeObject node in nodeObjects)
        {
            node.GetNode().MakePreconnections();
        }
        Debug.Log("Pasted " + nodeObjects.Count + " nodes.");
    }

    public void GeneratePhpCode()
    {
        Global.CopyToClipboard(PhpGenerator.GeneratePhpCode(this));
    }
}
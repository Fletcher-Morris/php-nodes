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
    public GameObject panelObject;
    public GameObject panelMask;
    public List<Image> headerSprites;
    public List<Image> panelSprites;
    public List<Sprite> panelAssets;
    public Text nameText;
    public Button moveButton;
    public Button deleteButton;
    public Vector3 lineOffset;
    public Transform inputTransform;
    public Transform outputTransform;
    public GameObject shadow;
    public float deleteAnimSpeed = 1.0f;
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
                input.bezier.start.x = input.GetLinkPosition().x;
                input.bezier.start.y = input.GetLinkPosition().y;
                input.bezier.start.z = input.GetLinkPosition().z;
                input.bezier.end.x = mousePos.x;
                input.bezier.end.y = mousePos.y;
                input.bezier.end.z = input.GetLinkPosition().z;
                input.bezier.UpdatePath();
            }
            else if (input.linkedLink != null)
            {
                input.bezier.line.enabled = true;
                input.bezier.line.endColor = NodeManager.Singleton.GetTagColor(input.linkedLink.dataType);
                input.bezier.line.startColor = NodeManager.Singleton.GetTagColor(input.dataType);
                input.bezier.start.x = input.GetLinkPosition().x;
                input.bezier.start.y = input.GetLinkPosition().y;
                input.bezier.start.z = input.GetLinkPosition().z;
                input.bezier.end.x = input.linkedLink.GetLinkPosition().x;
                input.bezier.end.y = input.linkedLink.GetLinkPosition().y;
                input.bezier.end.z = input.linkedLink.GetLinkPosition().z;
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
                output.bezier.start.x = output.GetLinkPosition().x;
                output.bezier.start.y = output.GetLinkPosition().y;
                output.bezier.start.z = output.GetLinkPosition().z;
                output.bezier.end.x = mousePos.x;
                output.bezier.end.y = mousePos.y;
                output.bezier.end.z = output.GetLinkPosition().z;
                output.bezier.UpdatePath();
            }
            else
            {
                output.bezier.line.enabled = false;
            }
        }
    }

    public void MoveNode()
    {
        transform.SetAsLastSibling();
        NodeManager.Singleton.movingNode = this;
    }

    public void DeleteNode()
    {
        StartCoroutine(DeleteNodeAnim());
    }

    private IEnumerator DeleteNodeAnim()
    {
        NodeManager.Singleton.nodeObjects.Remove(this);
        bool deleted = false;
        while (deleted == false)
        {
            Vector3 newScale = transform.localScale;
            newScale.x -= deleteAnimSpeed * Time.deltaTime;
            newScale.y -= deleteAnimSpeed * Time.deltaTime;
            transform.localScale = newScale;
            if (transform.localScale.x < 0.01f) deleted = true;
            yield return new WaitForEndOfFrame();
        }
        GameObject.Destroy(gameObject);
    }

    private float m_lastTitleClick = 0.0f;
    private bool m_minimised = false;
    public bool IsMinimised() { return m_minimised; }
    public void TitleClick()
    {
        float t = Time.time;
        if (m_lastTitleClick + 0.4f > t)
        {
            //  Double Click
            Debug.Log("DOUBLE CLICK");
            m_lastTitleClick = 0.0f;
            if (m_minimised == false) StartCoroutine(MinimiseAnim());
            else StartCoroutine(MaximiseAnim());
        }
        else
        {
            //  Single Click
            Debug.Log("CLICK");
            m_lastTitleClick = t;
        }
    }

    private IEnumerator MinimiseAnim()
    {
        RectTransform panelRect = panelObject.GetComponent<RectTransform>();
        m_minimised = true;
        bool animating = true;
        while (animating == true)
        {
            float y = panelRect.anchoredPosition.y;
            if(y >= panelRect.sizeDelta.y)
            {
                animating = false;
                panelRect.anchoredPosition = new Vector2(0.0f, panelRect.sizeDelta.y);
            }
            else
            {
                y += Time.deltaTime * 1000;
                panelRect.anchoredPosition = new Vector2(0.0f, y);
            }
            yield return new WaitForEndOfFrame();
        }
        headerSprites[6].sprite = panelAssets[1];
        headerSprites[7].sprite = panelAssets[0];
        headerSprites[8].sprite = panelAssets[2];
        shadow.GetComponent<RectTransform>().SetBottom(90.0f);
        yield return null;
    }

    private IEnumerator MaximiseAnim()
    {
        headerSprites[6].sprite = panelAssets[4];
        headerSprites[7].sprite = panelAssets[4];
        headerSprites[8].sprite = panelAssets[4];
        shadow.GetComponent<RectTransform>().SetBottom(-20.0f);
        RectTransform panelRect = panelObject.GetComponent<RectTransform>();
        bool animating = true;
        while (animating == true)
        {
            float y = panelRect.anchoredPosition.y;
            if (y <= 0.0f)
            {
                animating = false;
                panelRect.anchoredPosition = new Vector2(0.0f, 0.0f);
            }
            else
            {
                y -= Time.deltaTime * 1000;
                panelRect.anchoredPosition = new Vector2(0.0f, y);
            }
            yield return new WaitForEndOfFrame();
        }
        m_minimised = false;
        yield return null;
    }
}
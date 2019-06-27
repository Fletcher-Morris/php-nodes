using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeManager : MonoBehaviour
{
    public Transform contentArea;
    public Transform buttonArea;
    public GameObject nodePrefab;


    public List<NodeObject> nodeObjects;

    void Start()
    {
        foreach(Text tx in buttonArea.GetComponentsInChildren<Text>())
        {
            tx.text = tx.transform.parent.name;
            System.Type t = System.Type.GetType("Node_" + tx.text);
            if(t == null)
            {
                tx.transform.parent.GetComponent<Button>().interactable = false;
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
        nodeObjects.Add(newNode.GetComponent<NodeObject>());
        newNode.GetComponent<NodeObject>().Init((Node)System.Activator.CreateInstance(t));
    }
    public void CreateNode(Button _button)
    {
        CreateNode("Node_" + _button.transform.name);
    }

    public void AlignToGrid()
    {
        foreach(NodeObject node in nodeObjects)
        {
            Vector3 pos = node.transform.position;
            pos.x *= 0.5f;
            pos.y *= 0.5f;
            pos.x = Mathf.RoundToInt(pos.x);
            pos.y = Mathf.RoundToInt(pos.y);
            pos.x *= 2f;
            pos.y *= 2f;
            node.transform.position = pos;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && nodeObjects.Count >= 2)
        {
            nodeObjects[0].GetNode().outConnections[0].linkedConnection = nodeObjects[1].GetNode().inConnections[0];
            nodeObjects[1].GetNode().inConnections[0].linkedConnection = nodeObjects[0].GetNode().outConnections[0];
            Debug.Log("Connected Objects");
        }

        foreach(NodeObject node in nodeObjects)
        {
            node.DrawVisual();
        }
    }
}

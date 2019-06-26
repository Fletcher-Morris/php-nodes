using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeManager : MonoBehaviour
{
    public Transform contentArea;
    public GameObject nodePrefab;


    public List<NodeObject> nodeObjects;

    public void NewNode()
    {
        GameObject newNode = Instantiate(nodePrefab, contentArea);
        nodeObjects.Add(newNode.GetComponent<NodeObject>());
        newNode.GetComponent<NodeObject>().Init(new Node_Add());
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

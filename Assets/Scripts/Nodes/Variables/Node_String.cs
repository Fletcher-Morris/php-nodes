using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node_String : Node
{
    InputField field;

    public override void Setup()
    {
        nodeName = "STRING";
        tag = "string";
        width = 200;
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        GameObject obj = GameObject.Instantiate(nodeObject.stringUiPrefab, nodeObject.transform);
        field = obj.GetComponent<InputField>();
        //  Set up inputs
        {
            inConnections= new List<NodeConnection>();
        }
        //  Set up outputs
        {
            outConnections= new List<NodeConnection>();
            outConnections.Add(new NodeConnection(this, "Out", true, DataType.StringType));
        }
    }

    public override bool Valid()
    {
        return true;
    }

    public override void Serialize()
    {
        
    } 
}
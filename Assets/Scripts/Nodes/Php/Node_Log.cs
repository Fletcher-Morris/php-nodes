using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Log : Node
{
    public override void Setup()
    {
        nodeName = "LOG";
        tag = "php";
        isFunction = true;
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        //  Set up inputs
        {
            inConnections = new List<NodeConnection>();
            inConnections.Add(new NodeConnection(this, "Input", false, DataType.StringType));
            inConnections.Add(new NodeConnection(this, "Email", false, DataType.StringType));
        }
        //  Set up outputs
        {
            outConnections = new List<NodeConnection>();
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
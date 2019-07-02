using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Append : Node
{
    public override void Setup()
    {
        nodeName = "APPEND";
        tag = "string";
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        //  Set up inputs
        {
            inConnections = new List<NodeConnection>();
            inConnections.Add(new NodeConnection(this, "A", false, DataType.StringType));
            inConnections.Add(new NodeConnection(this, "B", false, DataType.StringType));
            inConnections.Add(new NodeConnection(this, "C", false, DataType.StringType));
            inConnections.Add(new NodeConnection(this, "D", false, DataType.StringType));
        }
        //  Set up outputs
        {
            outConnections = new List<NodeConnection>();
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
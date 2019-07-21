using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Compare : Node
{
    public override void Setup()
    {
        nodeName = "COMPARE";
        tag = "maths";
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        //  Set up inputs
        {
            inConnections = new List<NodeConnection>();
            inConnections.Add(new NodeConnection(this, "A", false, DataType.ClassType));
            inConnections.Add(new NodeConnection(this, "B", false, DataType.ClassType));
            inConnections.Add(new NodeConnection(this, "Operator", false, DataType.StringType));
        }
        //  Set up outputs
        {
            outConnections = new List<NodeConnection>();
            outConnections.Add(new NodeConnection(this, "True", true, DataType.StringType));
            outConnections.Add(new NodeConnection(this, "False", true, DataType.StringType));
        }
    }

    public override bool Valid()
    {
        return true;
    }

    public override string Serialize()
    {
        return "";
    }

    public override void Deserialize(string _data)
    {
    }
}
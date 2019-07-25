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
            inLinks = new List<NodeLink>();
            inLinks.Add(CreateNodeLink(this, "A", false, DataType.ClassType));
            inLinks.Add(CreateNodeLink(this, "B", false, DataType.ClassType));
            inLinks.Add(CreateNodeLink(this, "Operator", false, DataType.StringType));
        }
        //  Set up outputs
        {
            outLinks = new List<NodeLink>();
            outLinks.Add(CreateNodeLink(this, "True", true, DataType.StringType));
            outLinks.Add(CreateNodeLink(this, "False", true, DataType.StringType));
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
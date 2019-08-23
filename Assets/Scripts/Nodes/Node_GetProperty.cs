using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_GetProperty : Node
{
    public override void Setup()
    {
        nodeName = "GET PROPERTY";
        tag = "php";
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        //  Set up inputs
        {
            inLinks = new List<NodeLink>();
            inLinks.Add(CreateNodeLink(this, "Instance", false, DataType.ClassType));
            inLinks.Add(CreateNodeLink(this, "Property", false, DataType.StringType));
        }
        //  Set up outputs
        {
            outLinks = new List<NodeLink>();
            outLinks.Add(CreateNodeLink(this, "Out", true, DataType.ClassType));
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

    public override void Deserialize(List<string> _data)
    {
    }

    public override string GenPhpCode()
    {
        
        return null;
    }
}
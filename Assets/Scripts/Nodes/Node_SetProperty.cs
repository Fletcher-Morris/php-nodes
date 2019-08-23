using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_SetProperty : Node
{
    public override void Setup()
    {
        nodeName = "SET PROPERTY";
        tag = "php";
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        //  Set up inputs
        {
            inLinks = new List<NodeLink>();
            inLinks.Add(CreateNodeLink(this, "Instance", false, DataType.ClassType));
            inLinks.Add(CreateNodeLink(this, "Property", false, DataType.StringType));
            inLinks.Add(CreateNodeLink(this, "Value", false, DataType.ClassType));
        }
        //  Set up outputs
        {
            outLinks = new List<NodeLink>();
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
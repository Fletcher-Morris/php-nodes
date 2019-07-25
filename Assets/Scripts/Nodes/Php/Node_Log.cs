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
            inLinks = new List<NodeLink>();
            inLinks.Add(CreateNodeLink(this, "Input", false, DataType.StringType));
            inLinks.Add(CreateNodeLink(this, "Email", false, DataType.StringType));
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

    public override void Deserialize(string _data)
    {
    }
}
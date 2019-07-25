using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Connect : Node
{
    public override void Setup()
    {
        nodeName = "CONNECT";
        tag = "sql";
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        //  Set up inputs
        {
            inLinks = new List<NodeLink>();
            inLinks.Add(CreateNodeLink(this, "Address", false, DataType.StringType));
            inLinks.Add(CreateNodeLink(this, "Username", false, DataType.StringType));
            inLinks.Add(CreateNodeLink(this, "Password", false, DataType.StringType));
            inLinks.Add(CreateNodeLink(this, "Database", false, DataType.StringType));
        }
        //  Set up outputs
        {
            outLinks = new List<NodeLink>();
            outLinks.Add(CreateNodeLink(this, "Success", true, DataType.IntType));
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
}
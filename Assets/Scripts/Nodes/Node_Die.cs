using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Die : Node
{
    public override void Deserialize(List<string> _data)
    {

    }

    public override string GenPhpCode()
    {
        return "";
    }

    public override string Serialize()
    {
        return "";
    }

    public override void Setup()
    {

        nodeName = "DIE";
        tag = "php";
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        //  Set up inputs
        {
            inLinks = new List<NodeLink>();
        }
        //  Set up outputs
        {
            outLinks = new List<NodeLink>();
            inLinks.Add(CreateNodeLink(this, "Return", true, DataType.StringType));
        }
    }

    public override bool Valid()
    {
        return true;
    }
}

﻿using System.Collections;
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
            inLinks = new List<NodeLink>();
            inLinks.Add(CreateNodeLink(this, "A", false, DataType.StringType));
            inLinks.Add(CreateNodeLink(this, "B", false, DataType.StringType));
            inLinks.Add(CreateNodeLink(this, "C", false, DataType.StringType));
            inLinks.Add(CreateNodeLink(this, "D", false, DataType.StringType));
        }
        //  Set up outputs
        {
            outLinks = new List<NodeLink>();
            outLinks.Add(CreateNodeLink(this, "Out", true, DataType.StringType));
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
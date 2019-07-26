using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Node_Modulus : Node
{
    public override void Setup()
    {
        nodeName = "MODULUS";
        tag = "maths";
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        //  Set up inputs
        {
            inLinks = new List<NodeLink>();
            inLinks.Add(CreateNodeLink(this, "A", false, DataType.IntType));
            inLinks.Add(CreateNodeLink(this, "B", false, DataType.IntType));
        }
        //  Set up outputs
        {
            outLinks = new List<NodeLink>();
            outLinks.Add(CreateNodeLink(this, "Out", true, DataType.IntType));
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
        if (PhpGenerator.FunctionExists("Node_Modulus") == false)
        {
            string implem = "function Node_Modulus($_a, $_b)\r\n" +
            "{\r\n" +
            "\t$_c = $_a % $_b;\r\n" +
            "\treturn $_c;\r\n" +
            "}\r\n";
            PhpGenerator.GenFunction("Node_Modulus", implem);
        }
        return null;
    }
}
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
            inLinks.Add(CreateNodeLink(this, "True", false, DataType.ClassType));
            inLinks.Add(CreateNodeLink(this, "False", false, DataType.ClassType));
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

    public override void Deserialize(List<string> _data)
    {
    }

    public override string GenPhpCode()
    {
        if (PhpGenerator.FunctionExists("Compare") == false)
        {
            string implem = "function Compare($_a, $_b, $_op, $_true, $_false)\r\n" +
            "{\r\n" +
            "\tif($_op == \"==\")\r\n" +
            "\t{\r\n" +
            "\tif($_a == $_b) return $_true;\r\n" +
            "\t}\r\n" +
            "\telse if($_op == \"!=\" || $_op == \"<>\")\r\n" +
            "\t{\r\n" +
            "\tif($_a != $_b) return $_true;\r\n" +
            "\t}\r\n" +
            "\telse if($_op == \">\")\r\n" +
            "\t{\r\n" +
            "\tif($_a > $_b) return $_true;\r\n" +
            "\t}\r\n" +
            "\telse if($_op == \"<\")\r\n" +
            "\t{\r\n" +
            "\tif($_a < $_b) return $_true;\r\n" +
            "\t}\r\n" +
            "\telse if($_op == \">=\")\r\n" +
            "\t{\r\n" +
            "\tif($_a >= $_b) return $_true;\r\n" +
            "\t}\r\n" +
            "\telse if($_op == \"<=\")\r\n" +
            "\t{\r\n" +
            "\tif($_a <= $_b) return $_true;\r\n" +
            "\t}\r\n" +
            "\treturn $_false;\r\n" +
            "}\r\n";
            PhpGenerator.GenFunction("Compare", implem);
        }
        return null;
    }
}
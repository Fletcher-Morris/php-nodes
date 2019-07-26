using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node_Variable : Node
{
    InputField field;

    public override void Setup()
    {
        nodeName = "VARIABLE";
        tag = "sql";
        width = 200;
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        GameObject obj = GameObject.Instantiate(nodeObject.stringUiPrefab, nodeObject.transform);
        field = obj.GetComponent<InputField>();
        //  Set up inputs
        {
            inLinks = new List<NodeLink>();
            inLinks.Add(CreateNodeLink(this, "Var Name", false, DataType.StringType));
            inLinks.Add(CreateNodeLink(this, "In", false, DataType.ClassType));
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
        return field.text.ToString();
    }

    public override void Deserialize(List<string> _data)
    {

    }

    public override string GenPhpCode()
    {
        string varName = PhpGenerator.GenUniqueVarName("");

        return "";
    }
}
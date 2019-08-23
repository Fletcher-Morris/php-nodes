using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node_String : Node
{
    InputField field;

    public override void Setup()
    {
        nodeName = "STRING";
        tag = "string";
        width = 200;
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        GameObject obj = GameObject.Instantiate(nodeObject.stringUiPrefab, nodeObject.panelObject.transform);
        field = obj.GetComponent<InputField>();
        //  Set up inputs
        {
            inLinks= new List<NodeLink>();
        }
        //  Set up outputs
        {
            outLinks= new List<NodeLink>();
            outLinks.Add(CreateNodeLink(this, "Out", true, DataType.StringType));
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
        field.text = _data[1].ToString();
    }

    public override string GenPhpCode()
    {
        string varName = PhpGenerator.GenUniqueVarName(field.text.ToString());
        return "";
    }
}
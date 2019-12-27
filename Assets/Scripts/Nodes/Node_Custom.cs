using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node_Custom : Node
{
    InputField field;

    public override void Setup()
    {
        nodeName = "CUSTOM";
        tag = "php";
        width = 200;
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        GameObject obj = GameObject.Instantiate(nodeObject.stringUiPrefab, nodeObject.panelObject.transform);
        field = obj.GetComponent<InputField>();
        //  Set up inputs
        {
            inLinks = new List<NodeLink>();
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
        return field.text.ToString();
    }
}
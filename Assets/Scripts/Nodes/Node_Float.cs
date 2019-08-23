using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node_Float : Node
{
    InputField field;

    public override void Setup()
    {
        nodeName = "FLOAT";
        tag = "float";
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        GameObject obj = GameObject.Instantiate(nodeObject.floatUiPrefab, nodeObject.panelObject.transform);
        field = obj.GetComponent<InputField>();
        //  Set up inputs
        {
            inLinks= new List<NodeLink>();
        }
        //  Set up outputs
        {
            outLinks= new List<NodeLink>();
            outLinks.Add(CreateNodeLink(this, "Out", true, DataType.FloatType));
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
        field.text = float.Parse(_data[1]).ToString();
    }

    public override string GenPhpCode()
    {
        return "";
    }
}
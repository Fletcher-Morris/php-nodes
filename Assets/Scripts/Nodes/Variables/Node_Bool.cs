using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node_Bool : Node
{
    Toggle toggle;

    public override void Setup()
    {
        nodeName = "BOOL";
        tag = "bool";
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        GameObject obj = GameObject.Instantiate(nodeObject.boolUiPrefab, nodeObject.transform);
        toggle = obj.GetComponent<Toggle>();
        //  Set up inputs
        {
            inLinks= new List<NodeLink>();
        }
        //  Set up outputs
        {
            outLinks= new List<NodeLink>();
            outLinks.Add(CreateNodeLink(this, "Out", true, DataType.BoolType));
        }
    }

    public override bool Valid()
    {
        return true;
    }

    public override string Serialize()
    {
        return toggle.isOn.ToString();
    }

    public override void Deserialize(string _data)
    {
        toggle.isOn = bool.Parse(_data);
    }
}
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
        tag = "variable";
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        GameObject obj = GameObject.Instantiate(nodeObject.boolUiPrefab, nodeObject.transform);
        toggle = obj.GetComponent<Toggle>();
        //  Set up inputs
        {
            inConnections= new List<NodeConnection>();
        }
        //  Set up outputs
        {
            outConnections= new List<NodeConnection>();
            outConnections.Add(new NodeConnection(this, "Out", true, DataType.BoolType));
        }
    }

    public override bool Valid()
    {
        return true;
    }

    public override void Serialize()
    {
        bool value = toggle.isOn;
    }
}
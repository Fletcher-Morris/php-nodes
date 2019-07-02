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
        tag = "variable";
        nodeObject.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        GameObject obj = GameObject.Instantiate(nodeObject.floatUiPrefab, nodeObject.transform);
        field = obj.GetComponent<InputField>();
        //  Set up inputs
        {
            inConnections= new List<NodeConnection>();
        }
        //  Set up outputs
        {
            outConnections= new List<NodeConnection>();
            outConnections.Add(new NodeConnection(this, "Out", true, DataType.FloatType));
        }
    }

    public override bool Valid()
    {
        return true;
    }

    public override void Serialize()
    {
        
    } 
}
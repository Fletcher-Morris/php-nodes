using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_String : Node
{
    public override void Setup()
    {
        nodeName = "STRING";
        width = 150;
        //  Set up inputs
        {
            inConnections= new List<NodeConnection>();
        }
        //  Set up outputs
        {
            outConnections= new List<NodeConnection>();
            outConnections.Add(new NodeConnection(this, "Out", true, DataType.StringType));
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
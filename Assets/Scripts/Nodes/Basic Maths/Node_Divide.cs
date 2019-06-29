using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Divide : Node
{
    public override void Setup()
    {
        nodeName = "DIVIDE";
        width = 150;
        //  Set up inputs
        {
            inConnections= new List<NodeConnection>();
            inConnections.Add(new NodeConnection(this, "A", false, DataType.IntType));
            inConnections.Add(new NodeConnection(this, "B", false, DataType.IntType));
        }
        //  Set up outputs
        {
            outConnections= new List<NodeConnection>();
            outConnections.Add(new NodeConnection(this, "Out", true, DataType.IntType));
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
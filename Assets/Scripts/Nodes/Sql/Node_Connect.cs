using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_Connect : Node
{
    public override void Setup()
    {
        nodeName = "CONNECT";
        width = 150;
        //  Set up inputs
        {
            inConnections = new List<NodeConnection>();
            inConnections.Add(new NodeConnection(this, "Address", false, DataType.StringType));
            inConnections.Add(new NodeConnection(this, "Username", false, DataType.StringType));
            inConnections.Add(new NodeConnection(this, "Password", false, DataType.StringType));
            inConnections.Add(new NodeConnection(this, "Database", false, DataType.StringType));
        }
        //  Set up outputs
        {
            outConnections = new List<NodeConnection>();
            outConnections.Add(new NodeConnection(this, "Success", true, DataType.IntType));
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Linker : MonoBehaviour
{
    public NodeObject node;
    public NodeConnection connection;

    void Start()
    {
        connection.linker = this;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Linker : MonoBehaviour
{
    public NodeObject node;
    public NodeConnection connection;
    public Image sprite;

    void Start()
    {
        sprite.color = Global.ConnectionColor(connection.dataType);
        connection.linker = this;
    }
}

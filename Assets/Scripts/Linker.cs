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
    public BezierLine bezier;

    void Start()
    {
        UpdateColors();
        bezier = GetComponent<BezierLine>();
        connection.linker = this;
    }

    public void UpdateColors()
    {
        sprite.color = NodeManager.Singleton.GetTagColor(connection.dataType);
    }
}

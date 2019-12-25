using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateNodeButton : MonoBehaviour
{
    private bool isButton = false;
    void Start()
    {
        isButton = GetComponent<Button>() != null;
    }
    public void CreateNode()
    {
        if (isButton) NodeManager.Singleton.CreateNode("Node_" + transform.name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static int STATIC_NODE_ID;
    public static int STATIC_LINK_ID;

    public static float RemapFloat(float value, float min, float max, float newMin, float newMax)
    {
        float result = (newMin + (value - min) * (newMax - newMin) / (max - min));
        return result;
    }

    public static void CopyToClipboard(string copy)
    {
        TextEditor te = new TextEditor();
        te.text = copy;
        te.SelectAll();
        te.Copy();
    }
    public static string PasteFromClipboard()
    {
        TextEditor te = new TextEditor();
        te.Paste();
        return te.text;
    }
}
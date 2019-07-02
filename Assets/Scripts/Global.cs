using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static int STATIC_NODE_ID;
    public static int STATIC_CONNECTOR_ID;

    public static Color stringLinkColor;
    public static Color intLinkColor;
    public static Color floatLinkColor;
    public static Color boolLinkColor;
    public static Color classLinkColor;
    public static Color flowLinkColor;
    public static Color sqlNodeColor;
    public static Color phpNodeColor;
    public static Color variableNodeColor;
    public static Color mathNodeColor;
    public static Color stringNodeColor;

    public static Color ConnectionColor(DataType _type)
    {
        switch (_type)
        {
            case DataType.StringType:
                return stringLinkColor;
            case DataType.IntType:
                return intLinkColor;
            case DataType.BoolType:
                return boolLinkColor;
            case DataType.FloatType:
                return floatLinkColor;
            case DataType.ClassType:
                return classLinkColor;
            case DataType.FlowType:
                return flowLinkColor;
        }
        return Color.gray;
    }

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
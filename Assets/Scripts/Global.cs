using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static int STATIC_NODE_ID;

    public static Color ConnectionColor(DataType _type)
    {
        switch (_type)
        {
            case DataType.StringType:
                return Color.white;
            case DataType.IntType:
                return Color.blue;
            case DataType.BoolType:
                return Color.green;
            case DataType.FloatType:
                return Color.cyan;
            case DataType.ClassType:
                return Color.red;
            case DataType.ConType:
                return Color.yellow;
        }
        return Color.gray;
    }

    public static float RemapFloat(float value, float min, float max, float newMin, float newMax)
    {
        float result = (newMin + (value - min) * (newMax - newMin) / (max - min));
        return result;
    }
}
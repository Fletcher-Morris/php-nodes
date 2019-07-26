using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhpGenerator
{

    private static List<string> m_usedVariableNames;
    private static List<string> m_usedFunctionNames;
    private static List<string> m_functionImplementations;

    public static string GenUniqueVarName(string _desiredName)
    {
        string uniqueName = _desiredName;
        bool foundName = false;
        int i = 0;
        while (foundName == false)
        {
            if(VarNameExists(_desiredName + i.ToString()))
            {
                i++;
            }
            else
            {
                foundName = true;
                uniqueName = _desiredName + i.ToString();
            }
        }
        return uniqueName;
    }

    public static void GenFunction(string _functionName, string _functionImplementation)
    {
        if(m_usedFunctionNames.Contains(_functionName))
        {

        }
        else
        {
            m_usedFunctionNames.Add(_functionName);
            m_functionImplementations.Add(_functionImplementation);
        }
    }
    public static bool FunctionExists(string _functionName)
    {
        return m_usedFunctionNames.Contains(_functionName);
    }

    public static bool VarNameExists(string _name)
    {
        return m_usedVariableNames.Contains(_name);
    }

    public static string GeneratePhpCode(NodeManager nodes)
    {
        m_usedVariableNames = new List<string>();
        m_usedFunctionNames = new List<string>();
        m_functionImplementations = new List<string>();

        List<int> proccessedNodes = new List<int>();

        string phpString = "";
        List<string> phpLineList = new List<string>();

        phpLineList.Add("<?php");
        foreach (NodeObject node in nodes.nodeObjects)
        {
            if(proccessedNodes.Contains(node.GetUniqueId()))
            {

            }
            else
            {
                string code = node.GetNode().GenPhpCode();
                if (code != null) phpLineList.Add(code);
            }
        }
        foreach(string implem in m_functionImplementations)
        {
            phpLineList.Add(implem);
        }
        phpLineList.Add("?>");

        foreach (string line in phpLineList) phpString += (line + "\n");
        return phpString;
    }
}

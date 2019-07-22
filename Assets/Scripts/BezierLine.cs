﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BezierPath
{
    public List<Vector3> pathPoints;
    private int segments;
    public int pointCount;

    public BezierPath()
    {
        pathPoints = new List<Vector3>();
        pointCount = 100;
    }

    public void DeletePath()
    {
        pathPoints.Clear();
    }

    Vector3 BezierPathCalculation(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float tt = t * t;
        float ttt = t * tt;
        float u = 1.0f - t;
        float uu = u * u;
        float uuu = u * uu;

        Vector3 B = new Vector3();
        B = uuu * p0;
        B += 3.0f * uu * t * p1;
        B += 3.0f * u * tt * p2;
        B += ttt * p3;

        return B;
    }

    public void CreateCurve(List<Vector3> controlPoints)
    {
        segments = controlPoints.Count / 3;

        for (int s = 0; s < controlPoints.Count - 3; s += 3)
        {
            Vector3 p0 = controlPoints[s];
            Vector3 p1 = controlPoints[s + 1];
            Vector3 p2 = controlPoints[s + 2];
            Vector3 p3 = controlPoints[s + 3];

            if (s == 0)
            {
                pathPoints.Add(BezierPathCalculation(p0, p1, p2, p3, 0.0f));
            }

            for (int p = 0; p < (pointCount / segments); p++)
            {
                float t = (1.0f / (pointCount / segments)) * p;
                Vector3 point = new Vector3();
                point = BezierPathCalculation(p0, p1, p2, p3, t);
                pathPoints.Add(point);
                Debug.Log("!");
            }
        }
    }
}

public class BezierLine : MonoBehaviour
{
    LineRenderer lines;
    public List<GameObject> objects;
    public BezierPath path = new BezierPath();
    
    // Use this for initialization
    void Start()
    {
        UpdatePath();
    }
    
    // Update is called once per frame 
    void Update()
    {
        UpdatePath();
    }

    private void UpdatePath()
    {
        if (lines == null) lines = GetComponent<LineRenderer>();

        path.DeletePath();
        List<Vector3> c = new List<Vector3>();
        for (int o = 0; o < objects.Count; o++)
        {
            if (objects[o] != null)
            {
                Vector3 p = objects[o].transform.position;
                c.Add(p);
                Debug.Log("Added Point '" + p.ToString() + "'");
            }
        }
        path.CreateCurve(c);

        lines.positionCount = path.pointCount;
        for (int i = 0; i < (path.pointCount); i++)
        {
            lines.SetPositions(path.pathPoints.ToArray());
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnGlyph
{
    public List<Star> stars;
    public List<Direction> directions;
    public LineRenderer lineRenderer;
    public GameObject lineRendererGameobject;

    public DrawnGlyph()
    {
        stars = new List<Star>();
        lineRendererGameobject = new GameObject("Line Renderer");
        lineRendererGameobject.layer = 9;
        lineRenderer = lineRendererGameobject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    public void updateLine()
    {
        if (stars.Count > 1)
        {
            lineRenderer.positionCount = stars.Count;
            for (int i = 0; i < stars.Count; i++)
            {
                lineRenderer.SetPosition(i, stars[i].worldPosOffset);
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
}

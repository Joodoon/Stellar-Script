using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawnGlyph
{
    public List<Star> stars;
    public List<Direction> directions;
    public LineRenderer lineRenderer;

    public DrawnGlyph()
    {
        stars = new List<Star>();
        lineRenderer = new GameObject("Line Renderer").AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star
{
    public static Vector2 gridDimensions;
    public static Vector2 gridPosition;

    public static Grid grid;

    public Vector2 arrayPos;
    public Vector2 worldPos;

    public bool selected = false;
    public bool lastSelected = false;
    public Direction lastDirection;

    public Star(Vector2 gridPos){
        arrayPos = gridPos;
        worldPos = grid.CellToWorld(new Vector3Int((int)gridPos.x, (int)gridPos.y, 0));
    }
}

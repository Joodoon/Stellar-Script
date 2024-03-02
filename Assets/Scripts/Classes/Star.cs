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
    public Vector2 worldPosOffset;
    public Vector2 currentVel;

    public bool selected = false;
    public bool lastSelected = false;
    public Direction lastDirection;

    public Vector2 roamTarget;

    public Star(Vector2 gridPos){
        arrayPos = gridPos;
        worldPos = grid.CellToWorld(new Vector3Int((int)gridPos.x, (int)gridPos.y, 0));
        worldPosOffset = worldPos;
        roamTarget = new Vector2(Random.Range(-.35f, .35f), Random.Range(-.35f, .35f));
    }

    public void updatePosition(){
        worldPos = grid.CellToWorld(new Vector3Int((int)arrayPos.x, (int)arrayPos.y, 0));

        if(Time.frameCount % 2000 == 0) {
            roamTarget = new Vector2(Random.Range(-.35f, .35f), Random.Range(-.35f, .35f));
        }

        // slowly lerp the star's worldpos to the roam target
        //worldPosOffset = Vector2.SmoothDamp(worldPosOffset, worldPos + roamTarget, ref currentVel, 4f);
        worldPosOffset = worldPos;
    }
}

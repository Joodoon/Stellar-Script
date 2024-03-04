using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star
{
    public static bool isCasting = false;
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
    private float t;

    public Image image;

    public Star(Vector2 gridPos){
        arrayPos = gridPos;
        worldPos = new Vector2(grid.CellToWorld(new Vector3Int((int)gridPos.x, (int)gridPos.y, 0)).x, grid.CellToWorld(new Vector3Int((int)gridPos.x, (int)gridPos.y, 0)).y);
        worldPosOffset = worldPos;
        roamTarget = new Vector2(Random.Range(-.35f, .35f), Random.Range(-.35f, .35f));
    }

    public void updatePosition(){
        worldPos = new Vector2(grid.CellToWorld(new Vector3Int((int)arrayPos.x, (int)arrayPos.y, 0)).x, grid.CellToWorld(new Vector3Int((int)arrayPos.x, (int)arrayPos.y, 0)).y);

        if(Time.frameCount % 1000 == 0) {
            Vector2 newRoamTarget = new Vector2(Random.Range(-.35f, .35f), Random.Range(-.35f, .35f));
            roamTarget = Vector2.Lerp(roamTarget, newRoamTarget, Time.unscaledDeltaTime);
        }

        t = Mathf.Sin(Time.unscaledTime);
        // slowly lerp the star's worldpos to the roam target
        worldPosOffset = Vector2.LerpUnclamped(worldPos, worldPos + roamTarget * 1.5f, t);

        updateColor();
    }

    public void updateColor(){
        if(isCasting){
            if(selected){
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            }
            else{
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
            }
        }
        else{
            if(selected){
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            }
            else{
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            }
        }
    }
}

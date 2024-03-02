using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System;

public enum Direction{
    Up, Down, RightUp, RightDown, LeftUp, LeftDown, NullDir
}

public class StarGrid : MonoBehaviour
{
    [SerializeField] public Vector2 gridDimensions;
    [SerializeField] public GameObject starPrefab;
    [SerializeField] public static Grid gridComponent;
    [SerializeField] public GameObject starPlane;

    Vector2 totalGridWorldSize;

    public Star[,] grid;
    public ArrayList stars;
    public ArrayList constellationPatterns;



    void Start(){
        gridComponent = GetComponent<Grid>();

        Star.gridDimensions = gridDimensions;
        Star.gridPosition = transform.position;
        Star.grid = gridComponent;

        constellationSetup();

        stars = new ArrayList();

        createGrid();
        drawGrid();

        totalGridWorldSize = new Vector2(gridDimensions.x * gridComponent.cellSize.x, gridDimensions.y * gridComponent.cellSize.y * .75f);
    }

    void createGrid(){
        grid = new Star[(int)gridDimensions.x, (int)gridDimensions.y];
        for(int x = 0; x < gridDimensions.x; x++){
            for(int y = 0; y < gridDimensions.y; y++){
                grid[x, y] = new Star(new Vector2(x, y));
            }
        }
    }

    void drawGrid(){
        for(int x = 0; x < gridDimensions.x; x++){
            for(int y = 0; y < gridDimensions.y; y++){
                GameObject star = Instantiate(starPrefab, grid[x, y].worldPos, Quaternion.identity, starPlane.transform);
                stars.Add(star);
            }
        }
    }

    void constellationSetup(){
        constellationPatterns = new ArrayList
        {
            new Self(),
            new Up(),
            new Right(),
            new Force()
        };
    }

    public Direction indexToPattern(Vector2 index1, Vector2 index2){
        Direction dir;
        Vector2 start = new Vector2(index1.y, index1.x);
        Vector2 end = new Vector2(index2.y, index2.x);

        if(start.x == end.x){
            if(start.y < end.y){
                dir = Direction.Up;
            }else{
                dir = Direction.Down;
            }
        }else if(start.x < end.x){
            if(start.y == end.y){
                dir = (int)start.x % 2 == 1 ? Direction.RightDown : Direction.RightUp;
            }else{
                dir = (int)start.x % 2 == 1 ? Direction.RightUp : Direction.RightDown;
            }
        }else{
            if(start.y == end.y){
                dir = (int)start.x % 2 == 1 ? Direction.LeftDown : Direction.LeftUp;
            }else{
                dir = (int)start.x % 2 == 1 ? Direction.LeftUp : Direction.LeftDown;
            }
        }
        return dir;
    }

    public Star GetStarAtPosition(Vector2 position){
        if(position.x >= 0 && position.x < gridDimensions.x && position.y >= 0 && position.y < gridDimensions.y){
            return grid[(int)position.x, (int)position.y];
        }
        return null;
    }

    public Vector2 GetDistBetweenStars(Star star1, Star star2){
        return new Vector2(Math.Abs(star1.arrayPos.x - star2.arrayPos.x), Math.Abs(star1.arrayPos.y - star2.arrayPos.y));
    }

    void Update(){
        gridComponent.transform.position = new Vector3(starPlane.transform.position.x - totalGridWorldSize.y/2, starPlane.transform.position.y - totalGridWorldSize.x/2, 0f);
        //gridComponent.transform.position = starPlane.transform.position;

        for(int i = 0; i < stars.Count; i++){
            Star star = grid[(int)(i / gridDimensions.y), i % (int)gridDimensions.y];
            star.updatePosition();
            ((GameObject)stars[i]).transform.position = star.worldPosOffset;
            if(star.lastSelected){
                ((GameObject)stars[i]).GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if(star.selected){
                ((GameObject)stars[i]).GetComponent<SpriteRenderer>().color = Color.yellow;
            }
            else{
                ((GameObject)stars[i]).GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
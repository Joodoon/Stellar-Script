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
                GameObject star = Instantiate(starPrefab, grid[x, y].worldPos, Quaternion.identity);
                stars.Add(star);
            }
        }
    }

    void constellationSetup(){
        constellationPatterns = new ArrayList
        {
            new Self(),
            new Up(),
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

    void Update(){
        // change colour of only the last selected star
        // iterate through all stars and change the colour of the last selected star
        for(int i = 0; i < stars.Count; i++){
            Star star = grid[(int)(i / gridDimensions.y), i % (int)gridDimensions.y];
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
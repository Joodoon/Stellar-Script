using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ConstellationDrawing : MonoBehaviour
{
    private List<Star> stars;
    public static ArrayList drawnConstellations;
    private DrawnGlyph currentGlyph;
    private StarGrid starGrid;

    [SerializeField] Player player;

    SpellStack spellStack;

    public TextMeshProUGUI textMeshPro;

    private void Start()
    {
        stars = new List<Star>();
        drawnConstellations = new ArrayList();
        starGrid = GetComponent<StarGrid>();
        spellStack = GetComponent<SpellStack>();
    }

    private void Update()
    {
        foreach (DrawnGlyph glyph in drawnConstellations)
        {
            glyph.updateLine();
        }

        if(player.isCasting){
            Vector2 mousePos = GetMouseGridPosition();

            if (Input.GetMouseButton(0) && IsWithinGridBounds(mousePos)) {
                Star star = starGrid.GetStarAtPosition(mousePos);

                if(stars.Count == 0 && star != null && !star.selected){
                    currentGlyph = new DrawnGlyph();
                    drawnConstellations.Add(currentGlyph);
                }

                if (star != null) {
                    if (!star.selected && 
                    (stars.Count == 0 || 
                    (star != stars[stars.Count - 1] && starGrid.GetDistBetweenStars(star, stars[stars.Count - 1]).x <= 1 && starGrid.GetDistBetweenStars(star, stars[stars.Count - 1]).y <= 1))) {
                        AddStarToConstellation(star);
                    }
                    // FUCK this line of code 
                    else if(stars.Count > 0 && stars[stars.Count - 1].lastDirection != starGrid.indexToPattern(stars[stars.Count - 1].arrayPos, star.arrayPos) && stars[stars.Count - 1].lastDirection != getOpposite(starGrid.indexToPattern(stars[stars.Count - 1].arrayPos, star.arrayPos)) && star != stars[stars.Count - 1] && starGrid.GetDistBetweenStars(star, stars[stars.Count - 1]).x <= 1 && starGrid.GetDistBetweenStars(star, stars[stars.Count - 1]).y <= 1 && !star.lastSelected){
                        AddStarToConstellation(star);
                    }
                    else if(star.lastSelected && stars.Count >= 1){
                        RemoveStarFromConstellation(star);
                    }
                    else{}
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                if (stars.Count >= 1) {
                    CompleteConstellation();
                }
            }
        }
    }

    private Direction getOpposite(Direction dir){
        switch(dir){
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.RightUp:
                return Direction.LeftDown;
            case Direction.RightDown:
                return Direction.LeftUp;
            case Direction.LeftUp:
                return Direction.RightDown;
            case Direction.LeftDown:
                return Direction.RightUp;
            default:
                return Direction.NullDir;
        }
    }

    private Vector2 GetMouseGridPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {
            Vector2 worldPos = hit.point;
            return new Vector2(StarGrid.gridComponent.WorldToCell(worldPos).x,
                            StarGrid.gridComponent.WorldToCell(worldPos).y);
        }
        return Vector2.zero;
    }

    private bool IsWithinGridBounds(Vector2 position) {
        return position.x >= 0 && position.x < starGrid.gridDimensions.x &&
               position.y >= 0 && position.y < starGrid.gridDimensions.y;
    }

    private void AddStarToConstellation(Star star) {
        star.selected = true;

        if (stars.Count > 1) {
            stars[stars.Count - 1].lastSelected = true;
            stars[stars.Count - 2].lastSelected = false;
            stars[stars.Count - 1].lastDirection = starGrid.indexToPattern(stars[stars.Count - 1].arrayPos, star.arrayPos);
            //Debug.Log(stars[stars.Count - 1].lastDirection);
        } else if (stars.Count == 1) {
            stars[0].lastSelected = true;
            stars[0].lastDirection = starGrid.indexToPattern(stars[0].arrayPos, star.arrayPos);
            //Debug.Log(stars[0].lastDirection);
        }

        stars.Add(star);
        currentGlyph.stars = new List<Star>(stars);
    }

    private void RemoveStarFromConstellation(Star star) {
        if(stars.Count > 1) {
            // get number of occurences of previous star
            int prevCount = 0;
            foreach (Star s in stars) {
                if (s == stars[stars.Count - 1]) {
                    prevCount++;
                }
            }

            if(prevCount <= 1){
                star.selected = false;
                stars[stars.Count - 1].selected = false;
            }

            
            stars[stars.Count - 1].lastSelected = false;
            stars[stars.Count - 1].lastDirection = Direction.NullDir;
            stars.RemoveAt(stars.Count - 1);
            stars[stars.Count - 1].lastSelected = true;

            currentGlyph.stars = new List<Star>(stars);
        }
        else if (stars.Count == 1){
            stars[0].selected = false;
            stars[0].lastSelected = false;
            stars.RemoveAt(0);

            currentGlyph.stars = new List<Star>(stars);

            CompleteConstellation();
        }
    }

    private void CompleteConstellation()
    {
        bool validConstellationFound = false;

        if(stars.Count >= 1){
            List<Direction> directions = new List<Direction>();
            for (int i = 0; i < stars.Count - 1; i++) {
                directions.Add(starGrid.indexToPattern(stars[i].arrayPos, stars[i + 1].arrayPos));
            } 

            currentGlyph.directions = new List<Direction>(directions);

            // check if the constellation is a valid pattern
            foreach (Constellation constellation in starGrid.constellationPatterns) {
                bool valid = true;
                if (constellation.pattern.Count == directions.Count) {   
                    for (int i = 0; i < constellation.pattern.Count; i++) {
                        if ((Direction)constellation.pattern[i] != directions[i]) {
                            valid = false;
                        }
                    }
                    if (valid) {
                        validConstellationFound = true;
                        constellation.Scry();
                        spellStack.constellations.Add(constellation.copy());
                        textMeshPro.text = spellStack.constellations.ToCommaSeparatedString();
                        break;
                    }
                }
            }

            if (!validConstellationFound) {
                currentGlyph.stars.Clear();
                currentGlyph.updateLine();
                Destroy(currentGlyph.lineRendererGameobject);
                drawnConstellations.Remove(currentGlyph);

                // iterate over grid nested array
                foreach (Star star in stars) {
                    star.selected = false;
                    star.lastSelected = false;
                }
            }

            foreach (Star star in stars)
            {
                //star.selected = false;
                star.lastSelected = false;
                star.lastDirection = Direction.NullDir;
            }

            stars.Clear();
        }
    }
}
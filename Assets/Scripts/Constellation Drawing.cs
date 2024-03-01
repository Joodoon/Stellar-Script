using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ConstellationDrawing : MonoBehaviour
{
    private List<Star> stars;
    private List<List<Star>> constellations;
    private StarGrid starGrid;

    LineRenderer lineRenderer;

    SpellStack spellStack;

    public TextMeshProUGUI textMeshPro;

    private void Start()
    {
        stars = new List<Star>();
        constellations = new List<List<Star>>();
        starGrid = GetComponent<StarGrid>();
        spellStack = GetComponent<SpellStack>();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        foreach (Star star in stars)
        {
            lineRenderer.positionCount = stars.Count;
            lineRenderer.SetPosition(stars.IndexOf(star), star.worldPos);
        }

        Vector2 mousePos = GetMouseGridPosition();

        if (Input.GetMouseButton(0) && IsWithinGridBounds(mousePos)) {
            Star star = starGrid.GetStarAtPosition(mousePos);

            if (star != null) {
                if (!star.selected && (stars.Count == 0 || star != stars[stars.Count - 1])) {
                    AddStarToConstellation(star);
                }
                else if (star.lastSelected && stars.Count >= 1) {
                    RemoveStarFromConstellation(star);
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (stars.Count >= 1) {
                CompleteConstellation();
            }
            stars.Clear();
        }
    }

    private Vector2 GetMouseGridPosition() {
        return new Vector2(StarGrid.gridComponent.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x,
                           StarGrid.gridComponent.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)).y);
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
            star.lastDirection = starGrid.indexToPattern(stars[stars.Count - 1].arrayPos, star.arrayPos);
        }

        stars.Add(star);
    }

    private void RemoveStarFromConstellation(Star star) {
        if(stars.Count > 1) {
            star.selected = false;
            stars[stars.Count - 1].selected = false;
            stars[stars.Count - 1].lastSelected = false;
            stars.RemoveAt(stars.Count - 1);
            stars[stars.Count - 1].lastSelected = true;
        }
        else if (stars.Count == 1){
            stars[0].selected = false;
            stars[0].lastSelected = false;
            stars.RemoveAt(0);
            CompleteConstellation();
        }
    }

    private void CompleteConstellation()
    {
        if(stars.Count >= 1){
            constellations.Add(new List<Star>(stars));

            List<Direction> directions = new List<Direction>();
            for (int i = 0; i < stars.Count - 1; i++) {
                directions.Add(starGrid.indexToPattern(stars[i].arrayPos, stars[i + 1].arrayPos));
                Debug.Log(stars[i].arrayPos + " " + stars[i + 1].arrayPos);
            }

            // check if the constellation is a valid pattern
            foreach (Constellation constellation in starGrid.constellationPatterns) {
                if (constellation.pattern.Count == directions.Count) {
                    bool valid = true;

                    // debug.log the directions of both the constellation and the drawn pattern
                    Debug.Log(constellation.name + " " + constellation.pattern.ToCommaSeparatedString());
                    Debug.Log("directions " + directions.ToCommaSeparatedString());

                    for (int i = 0; i < constellation.pattern.Count; i++) {
                        if ((Direction)constellation.pattern[i] != directions[i]) {
                            valid = false;
                            break;
                        }
                    }
                    if (valid) {
                        constellation.Scry();
                        spellStack.constellations.Add(constellation);
                        textMeshPro.text = spellStack.constellations.ToCommaSeparatedString();
                    }
                }
            }

            foreach (Star star in stars)
            {
                star.selected = false;
                star.lastSelected = false;
            }
        }
    }
}
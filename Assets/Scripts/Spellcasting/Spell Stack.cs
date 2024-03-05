using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellStack : MonoBehaviour
{
    [SerializeField] public ArrayList constellations;
    [SerializeField] public TextMeshProUGUI textMeshPro;
    [SerializeField] public Player player;
    [SerializeField] public StarGrid starGrid;

    void Start()
    {
        constellations = new ArrayList();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !player.isCasting && !DialogueTrigger.dialogueActive && ConstellationDrawing.drawnConstellations.Count > 0)
        { 
            foreach (DrawnGlyph glyph in ConstellationDrawing.drawnConstellations)
            {
                Destroy(glyph.lineRenderer.gameObject);
            }

            // iterate over grid nested array
            for (int x = 0; x < starGrid.gridDimensions.x; x++)
            {
                for (int y = 0; y < starGrid.gridDimensions.y; y++)
                {
                    starGrid.grid[x, y].selected = false;
                    starGrid.grid[x, y].lastSelected = false;
                }
            }

            StartCoroutine(castSpell());
            ConstellationDrawing.drawnConstellations.Clear();
            textMeshPro.text = "";
        }
    }

    public void updateText(){
        textMeshPro.text = "";
        foreach (Spellscry spellscry in Constellation.spellscryStack.elements)
        {
            textMeshPro.text += spellscry.text + "\n";
        }

    }

    IEnumerator castSpell()
    {
        foreach (Constellation constellation in constellations)
        {
            yield return StartCoroutine(constellation.Cast());
        }
        constellations.Clear();
        Constellation.spellscryStack.elements.Clear();
        yield return null;
    }
}

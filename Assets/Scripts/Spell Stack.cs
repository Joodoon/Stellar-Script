using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellStack : MonoBehaviour
{
    [SerializeField] public ArrayList constellations;
    [SerializeField] public TextMeshProUGUI textMeshPro;

    void Start()
    {
        constellations = new ArrayList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            foreach (DrawnGlyph glyph in ConstellationDrawing.drawnConstellations)
            {
                Destroy(glyph.lineRenderer.gameObject);
            }
            castSpell();
            ConstellationDrawing.drawnConstellations.Clear();
            textMeshPro.text = "";
        }
    }

    void castSpell()
    {
        foreach (Constellation constellation in constellations)
        {
            constellation.Cast();
        }
        constellations.Clear();
    }
}

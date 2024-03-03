using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpellStack : MonoBehaviour
{
    [SerializeField] public ArrayList constellations;
    [SerializeField] public TextMeshProUGUI textMeshPro;
    [SerializeField] public Player player;

    void Start()
    {
        constellations = new ArrayList();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !player.isCasting)
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

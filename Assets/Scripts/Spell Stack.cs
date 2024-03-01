using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellStack : MonoBehaviour
{
    [SerializeField] public ArrayList constellations;

    void Start()
    {
        constellations = new ArrayList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            castSpell();
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

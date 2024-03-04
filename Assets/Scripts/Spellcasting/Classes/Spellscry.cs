using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellscry
{
    public GameObject[] obj;
    public Vector2? vector;

    public Spellscry(GameObject[] obj)
    {
        this.obj = obj;
        this.vector = null;
    }

    public Spellscry(Vector2 vector)
    {
        this.obj = null;
        this.vector = vector;
    }

    public Spellscry()
    {
        this.obj = null;
        this.vector = null;
    }
}

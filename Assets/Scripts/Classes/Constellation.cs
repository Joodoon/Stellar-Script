using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Constellation
{
    public string name;
    public ArrayList pattern;

    public abstract void Cast();

    public abstract void Scry();

    public static ArrayList spellscryStack;

    public Constellation() {
        spellscryStack = new ArrayList();
    }
}

public class Self : Constellation
{
    public Self() : base() {
        name = "Self";
        pattern = new ArrayList
        {
            Direction.Down
        };
    }

    public override void Cast()
    {
        Debug.Log("Self: Cast");
    }

    public override void Scry()
    {
        spellscryStack.Add(new Spellscry(GameObject.Find("Player")));
    }
}

public class Up : Constellation
{
    public Up() : base() {
        name = "Up";
        pattern = new ArrayList
        {
            Direction.Up,
            Direction.Up
        };
    }

    public override void Cast()
    {
        Debug.Log("Up: Cast");
    }

    public override void Scry()
    {
        spellscryStack.Add(new Spellscry(new Vector2(0, 1) * 100));
    }
}

public class Force : Constellation
{
    GameObject obj;
    Vector2? vector;

    public Force() : base() {
        name = "Force";
        pattern = new ArrayList
        {
            Direction.Up,
            Direction.RightUp,
            Direction.RightDown,
            Direction.Down
        };
    }

    public override void Cast()
    {
        if(spellscryStack.Count < 2){
            Debug.Log("Force: Not enough spellscries!");
            return;
        }

        if(obj == null){
            Debug.Log("Force: No object to apply force to");
        }
        else if(vector == null){
            Debug.Log("Force: No vector to apply force");
        }
        else{
            Debug.Log("Force: Cast");
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.AddForce((Vector2)vector);

            spellscryStack.RemoveAt(spellscryStack.Count - 1);
            spellscryStack.RemoveAt(spellscryStack.Count - 1);
            
            // TODO: update textMeshPro
        }
    }

    public override void Scry()
    {
        if(spellscryStack.Count < 2){
            return;
        }

        obj = ((Spellscry)spellscryStack[spellscryStack.Count - 2]).obj;
        vector = ((Spellscry)spellscryStack[spellscryStack.Count - 1]).vector;
    }
}
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Constellation
{
    public string name;
    public ArrayList pattern;

    // update spellscryCastingStack
    public abstract IEnumerator Cast();

    // update spellscryStack
    public abstract void Scry();

    public static TheStack spellscryStack;
    public static TheStack spellscryCastingStack;

    public Constellation copy()
    {
        return (Constellation)this.MemberwiseClone();
    }

    public Constellation() {
        spellscryStack = new TheStack();
        spellscryCastingStack = new TheStack();
    }
}


public class Self : Constellation
{
    public Self() : base() {
        name = "Self";
        pattern = new ArrayList
        {
            Direction.RightUp,
            Direction.RightDown,
            Direction.LeftDown,
            Direction.LeftUp
        };
    }

    public override IEnumerator Cast()
    {
        spellscryCastingStack.Push(new Spellscry(new GameObject[] { GameObject.Find("Player") }));
        yield return null;
    }

    public override void Scry()
    {
        Spellscry spellscry = new Spellscry(new GameObject[] { GameObject.Find("Player") });
        spellscry.text = "Self";
        spellscryStack.Push(spellscry);
    }
}

public class Mouse : Constellation
{
    public Mouse() : base() {
        name = "Mouse";
        pattern = new ArrayList
        {
            Direction.RightDown,
            Direction.RightUp,
            Direction.LeftUp,
            Direction.LeftDown
        };
    }

    public override IEnumerator Cast()
    {
        spellscryCastingStack.Push(new Spellscry(new GameObject[] { GameObject.Find("Mouse") }));
        yield return null;
    }

    public override void Scry()
    {
        Spellscry spellscry = new Spellscry(new GameObject[] { GameObject.Find("Mouse") });
        spellscry.text = "Mouse";
        spellscryStack.Push(spellscry);
    }
}

public class Up : Constellation
{
    public Up() : base() {
        name = "Up";
        pattern = new ArrayList
        {
            Direction.RightUp,
            Direction.RightDown
        };
    }

    public override IEnumerator Cast()
    {
        spellscryCastingStack.Push(new Spellscry(new Vector2(0, 1)));
        yield return null;
    }

    public override void Scry()
    {
        Spellscry spellscry = new Spellscry(new Vector2(0, 1));
        spellscry.text = "Up";
        spellscryStack.Push(spellscry);
    }
}

public class Down : Constellation
{
    public Down() : base() {
        name = "Down";
        pattern = new ArrayList
        {
            Direction.RightDown,
            Direction.RightUp
        };
    }

    public override IEnumerator Cast()
    {
        spellscryCastingStack.Push(new Spellscry(new Vector2(0, -1)));
        yield return null;
    }

    public override void Scry()
    {
        Spellscry spellscry = new Spellscry(new Vector2(0, -1));
        spellscry.text = "Down";
        spellscryStack.Push(spellscry);
    }
}

public class Right : Constellation
{
    public Right() : base() {
        name = "Right";
        pattern = new ArrayList
        {
            Direction.RightDown,
            Direction.LeftDown
        };
    }

    public override IEnumerator Cast()
    {
        spellscryCastingStack.Push(new Spellscry(new Vector2(1, 0)));
        yield return null;
    }

    public override void Scry()
    {
        Spellscry spellscry = new Spellscry(new Vector2(1, 0));
        spellscry.text = "Right";
        spellscryStack.Push(spellscry);
    }
}

public class Left : Constellation
{
    public Left() : base() {
        name = "Left";
        pattern = new ArrayList
        {
            Direction.LeftDown,
            Direction.RightDown
        };
    }

    public override IEnumerator Cast()
    {
        spellscryCastingStack.Push(new Spellscry(new Vector2(-1, 0)));
        yield return null;
    }

    public override void Scry()
    {
        Spellscry spellscry = new Spellscry(new Vector2(-1, 0));
        spellscry.text = "Left";
        spellscryStack.Push(spellscry);
    }
}

public class Position : Constellation
{
    GameObject[] obj;
    Spellscry objSpellscry;

    public Position() : base() {
        name = "Position";
        pattern = new ArrayList
        {
            Direction.Down,
            Direction.Down
        };
    }

    public override IEnumerator Cast()
    {
        objSpellscry = (Spellscry)spellscryCastingStack.Pop();
        obj = objSpellscry.obj;

        Vector2 avgPos = new Vector2(0, 0);
        foreach(GameObject obj in obj){
            avgPos += (Vector2)obj.transform.position;
        }
        avgPos /= obj.Length;

        Spellscry spellscry = new Spellscry(avgPos);
        spellscryCastingStack.Push(spellscry);
        yield return null;
    }

    public override void Scry()
    {
        if(spellscryStack.Count < 1){
            return;
        }
        objSpellscry = (Spellscry)spellscryStack.Pop();
        obj = objSpellscry.obj;

        Spellscry spellscry = new Spellscry();
        spellscry.text = "(" + objSpellscry.text + "-Position)";
        spellscryStack.Push(spellscry);
    }
}

public class Force : Constellation
{
    GameObject[] obj;
    Vector2? vector;
    Spellscry objSpellscry;
    Spellscry vectorSpellscry;

    public Force() : base() {
        name = "Force";
        pattern = new ArrayList
        {
            Direction.Up,
            Direction.RightUp,
            Direction.RightDown,
            Direction.Down,
            Direction.LeftUp,
            Direction.LeftDown
        };
    }

    public override IEnumerator Cast()
    {
        if(spellscryCastingStack.Count < 2){
            yield return null;
        }

        vectorSpellscry = (Spellscry)spellscryCastingStack.Pop();
        vector = vectorSpellscry.vector;
        objSpellscry = (Spellscry)spellscryCastingStack.Pop();
        obj = objSpellscry.obj;

        spellscryCastingStack.Push(new Spellscry(obj));

        if(obj == null){
            Debug.Log("Force: No object to apply force to");
        }
        else if(vector == null){
            Debug.Log("Force: No vector to apply force");
        }
        else{
            foreach(GameObject obj in obj){
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.AddForce(((Vector2)vector) * 100, ForceMode2D.Impulse);
            }
        }
        yield return null;
    }

    public override void Scry()
    {
        if(spellscryStack.Count < 2){
            return;
        }

        
        vectorSpellscry = (Spellscry)spellscryStack.Pop();
        vector = vectorSpellscry.vector;
        objSpellscry = (Spellscry)spellscryStack.Pop();
        obj = objSpellscry.obj;

        Spellscry spellscry = new Spellscry(obj);
        string objText = objSpellscry.text;
        string vectorText = vectorSpellscry.text;
        
        spellscry.text = objText + " Force-" + vectorText;
        spellscryStack.Push(spellscry);
    }
}

public class Delay : Constellation{
    public Delay() : base() {
        name = "Delay";
        pattern = new ArrayList
        {
            Direction.Up,
            Direction.Up
        };
    }

    public override IEnumerator Cast()
    {
        yield return new WaitForSeconds(1);
    }

    public override void Scry()
    {
        Spellscry spellscry = (Spellscry)spellscryStack.Peek();
        spellscry.text = spellscry.text + "-Delay(1s)";
    }
}

public class Nearest : Constellation{
    GameObject[] obj;
    GameObject[] nearest;

    public Nearest() : base() {
        name = "Nearest";
        pattern = new ArrayList
        {
            Direction.Up,
            Direction.RightDown,
            Direction.RightDown
        };
    }

    public override IEnumerator Cast()
    {
        nearest = new GameObject[1];
        obj = ((Spellscry)spellscryCastingStack.Pop()).obj;

        // iterate through all gameobjects with the tag 'entity' and find the nearest one to the vector position
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Entity");
        
        // get average position of gameobjects in obj
        Vector2 avgPos = new Vector2(0, 0);
        foreach(GameObject entity in obj){
            avgPos += (Vector2)entity.transform.position;
        }
        avgPos /= obj.Length;

        float minDist = Mathf.Infinity;
        foreach(GameObject entity in entities){
            // exclude entities in obj
            if(System.Array.IndexOf(obj, entity) != -1){
                continue;
            }

            float dist = Vector2.Distance(entity.transform.position, avgPos);
            if(dist < minDist){
                minDist = dist;
                nearest[0] = entity;
            }
        }

        spellscryCastingStack.Push(new Spellscry(nearest));

        yield return null;
    }

    public override void Scry()
    {
        if(spellscryStack.Count < 1){
            return;
        }

        Spellscry spellscry = new Spellscry();
        spellscry.text = "Nearest to " + ((Spellscry)spellscryStack.Pop()).text;
        spellscryStack.Push(spellscry);
    }
}

public class Burst : Constellation{
    ArrayList hits;
    Vector2? vector;

    public Burst() : base() {
        name = "Burst";
        pattern = new ArrayList
        {
            Direction.Up,
            Direction.RightDown,
            Direction.Up
        };
    }

    public override IEnumerator Cast()
    {
        hits = new ArrayList();
        vector = ((Spellscry)spellscryCastingStack.Pop()).vector;

        // iterate through all gameobjects with the tag 'entity' and add them to hits if they are within a certain radius of the vector position
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Entity");
        foreach(GameObject entity in entities){
            if(Vector2.Distance(entity.transform.position, (Vector2)vector) < 20){
                hits.Add(entity);
            }
        }

        spellscryCastingStack.Push(new Spellscry(hits.ToArray(typeof(GameObject)) as GameObject[]));

        yield return null;
    }

    public override void Scry()
    {
        if(spellscryStack.Count < 1){
            return;
        }

        Spellscry spellscry = new Spellscry();
        spellscry.text = "Hits in radius 20 around " + ((Spellscry)spellscryStack.Pop()).text;
        spellscryStack.Push(spellscry);
    }
}

public class Scale : Constellation
{
    GameObject[] obj;
    Vector2? vector;
    Spellscry objSpellscry;
    Spellscry vectorSpellscry;

    public Scale() : base() {
        name = "Scale";
        pattern = new ArrayList
        {
            Direction.Up,
            Direction.RightUp,
            Direction.RightDown,
            Direction.Down,
            Direction.LeftDown,
            Direction.LeftUp
        };
    }

    public override IEnumerator Cast()
    {
        if(spellscryCastingStack.Count < 2){
            yield return null;
        }

        vectorSpellscry = (Spellscry)spellscryCastingStack.Pop();
        vector = vectorSpellscry.vector;
        objSpellscry = (Spellscry)spellscryCastingStack.Pop();
        obj = objSpellscry.obj;

        spellscryCastingStack.Push(new Spellscry(obj));

        foreach(GameObject obj in obj){
            obj.transform.localScale *= ((Vector2)vector).magnitude;
        }

        yield return null;
    }

    public override void Scry()
    {
        if(spellscryStack.Count < 2){
            return;
        }

        
        vectorSpellscry = (Spellscry)spellscryStack.Pop();
        vector = vectorSpellscry.vector;
        objSpellscry = (Spellscry)spellscryStack.Pop();
        obj = objSpellscry.obj;

        Spellscry spellscry = new Spellscry(obj);
        string objText = objSpellscry.text;
        string vectorText = ((Spellscry)vectorSpellscry).text;
        
        spellscry.text = objText + " Scaled by-" + vectorText;
        spellscryStack.Push(spellscry);
    }
}

public class Double : Constellation{
    Vector2? vector;

    public Double() : base() {
        name = "Double";
        pattern = new ArrayList
        {
            Direction.RightUp,
            Direction.RightUp
        };
    }

    public override IEnumerator Cast()
    {
        vector = ((Spellscry)spellscryCastingStack.Pop()).vector;
        vector = new Vector2(((Vector2)vector).x * 2, ((Vector2)vector).y * 2);
        spellscryCastingStack.Push(new Spellscry((Vector2)vector));
        yield return null;
    }

    public override void Scry()
    {
        Spellscry spellscry = (Spellscry)spellscryStack.Peek();
        spellscry.text = spellscry.text + "x2";
    }
}

public class Half : Constellation{
    Vector2? vector;

    public Half() : base() {
        name = "Half";
        pattern = new ArrayList
        {
            Direction.RightDown,
            Direction.RightDown
        };
    }

    public override IEnumerator Cast()
    {
        vector = ((Spellscry)spellscryCastingStack.Pop()).vector;
        vector = new Vector2(((Vector2)vector).x / 2, ((Vector2)vector).y / 2);
        spellscryCastingStack.Push(new Spellscry((Vector2)vector));
        yield return null;
    }

    public override void Scry()
    {
        Spellscry spellscry = (Spellscry)spellscryStack.Peek();
        spellscry.text = spellscry.text + "/2";
    }
}
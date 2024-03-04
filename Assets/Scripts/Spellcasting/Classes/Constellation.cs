using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Constellation
{
    public string name;
    public ArrayList pattern;

    public abstract IEnumerator Cast();

    public abstract void Scry();

    public static TheStack spellscryStack;

    public Constellation copy()
    {
        return (Constellation)this.MemberwiseClone();
    }

    public Constellation() {
        spellscryStack = new TheStack();
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
        yield return null;
    }

    public override void Scry()
    {
        spellscryStack.Push(new Spellscry(new GameObject[] { GameObject.Find("Player") }));
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
        yield return null;
    }

    public override void Scry()
    {
        spellscryStack.Push(new Spellscry(new Vector2(0, 1)));
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
        yield return null;
    }

    public override void Scry()
    {
        spellscryStack.Push(new Spellscry(new Vector2(1, 0)));
    }
}

public class Force : Constellation
{
    GameObject[] obj;
    Vector2? vector;

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
        if(obj == null){
            Debug.Log("Force: No object to apply force to");
        }
        else if(vector == null){
            Debug.Log("Force: No vector to apply force");
        }
        else{
            foreach(GameObject obj in obj){
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.AddForce((Vector2)vector * 100, ForceMode2D.Impulse);
            }
        }
        vector = null;
        obj = null;
        yield return null;
    }

    public override void Scry()
    {
        if(spellscryStack.Count < 2){
            return;
        }

        vector = ((Spellscry)spellscryStack.Pop()).vector;
        obj = ((Spellscry)spellscryStack.Pop()).obj;
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

    }
}
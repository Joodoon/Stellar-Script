using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStack
{
    public ArrayList elements;
    public int Count;

    public TheStack(){
        elements = new ArrayList();
    }

    public void Push(object element){
        elements.Add(element);
        Count++;
    }

    public object Pop(){
        object element = elements[elements.Count - 1];
        elements.RemoveAt(elements.Count - 1);
        Count--;
        return element;
    }

    public object Peek(){
        return elements[elements.Count - 1];
    }

    public object getAt(int index){
        return elements[index];
    }
}

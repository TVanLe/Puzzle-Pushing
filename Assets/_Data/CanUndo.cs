using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CanUndo : MonoBehaviour
{
    [SerializeField] protected Stack<Vector3> MyStack;

    protected virtual void Awake()
    {
        MyStack = new Stack<Vector3>();
        MyStack.Push(transform.position);
    }

    public  void AddToStack(Vector3 newPos)
    {
        MyStack.Push(newPos);
    }
    
    public virtual void Undo()
    {
        if (MyStack.Count > 1)
        {
            MyStack.Pop();
            Vector3 lastPos = MyStack.Peek();
            this.transform.position = lastPos;
        }
    }

    public void POPStack()
    {
        this.MyStack.Pop();
    }
}

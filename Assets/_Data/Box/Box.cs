using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Box : CanUndo
{
    public override void Undo()
    {
        BoxManager.allBoxLocation.Remove(transform.position);
        base.Undo();
        BoxManager.allBoxLocation[transform.position] = transform;
    }
    
}

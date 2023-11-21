using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManger : MonoBehaviour
{
   public static InputManger Instance;
   [SerializeField] public bool undo;
   [SerializeField] public bool restart;
   [SerializeField] public bool back;
    
   private void Awake()
   {
      InputManger.Instance = this;
   }

   private void Update()
   {
      Undo();
      Restart();
      Back();
   }

   protected virtual void Undo()
   {
      if (Input.GetKeyDown(KeyCode.Z))
      {
         Debug.Log("undo");
         this.undo = true;
      }
      else
      {
         this.undo = false;
      }
   }

   protected virtual void Restart()
   {
      if (Input.GetKeyDown(KeyCode.R))
      {
         Debug.Log("Restart");
         this.restart = true;
      }
      else
      {
         this.restart = false;
      }
   }

   protected virtual void Back()
   {
      if (Input.GetKeyDown(KeyCode.X))
      {
         this.back = true;
      }
      else
      {
         this.back = false;
      }
   }
   
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CanUndo
{
   [SerializeField] protected float speedSmoothly;
   [SerializeField] protected Vector3 nextLocation;
   
   private void Start()
   {
      nextLocation = transform.position;
   }

   private void Update()
   {
      Moving();
      ForUndo();
   }

   
   
   protected virtual void Moving()
   {
      if (transform.position == nextLocation)
      {
         GetMovingInput();
      }
      
      // if not found wall , player can move
      if (!TilemapManager.wallIsBlocked.ContainsKey(nextLocation))
      {
         // if not found box , player can move
         if (!BoxManager.allBoxLocation.ContainsKey(nextLocation))
         {
            //Add to Stack
            if (nextLocation != MyStack.Peek())
            {
               AudioManager.Instance.PlaySFX(AudioManager.Instance.player);
               AddToStack(nextLocation);
               BoxManager.Instance.CalculateStackForEveryBoxes();
            }
            
            transform.position = Vector3.MoveTowards(transform.position, nextLocation, speedSmoothly * Time.deltaTime);
         }
         else
         {
            //Handle collider box

            Vector3 nextBoxLocation = nextLocation * 2 - transform.position;
            if (BoxCanMove(nextBoxLocation))
            {
               Debug.Log("Move Box");
               AudioManager.Instance.PlaySFX(AudioManager.Instance.box);
               //player move
               transform.position = Vector3.MoveTowards(transform.position, nextLocation, speedSmoothly * Time.deltaTime);
               BoxManager.Instance.TranslateBox(nextLocation, nextBoxLocation);
               
               //Add to Stack
               if (nextLocation != MyStack.Peek())
               {
                  AddToStack(nextLocation);
                  if(transform.position == nextLocation)
                     BoxManager.Instance.CalculateStackForEveryBoxes();
               }
            }
            else
            {
               Debug.Log("Can't move box");
               //resest nextLocation back transform
               nextLocation = transform.position;
            }
         }
      }
      else
      {
         //resest nextLocation back transform
         nextLocation = transform.position;
      }
   }
   
   protected virtual void GetMovingInput()
   {
      if (!GameManager.NotGetInput)
      {
         if(Input.GetKeyDown(KeyCode.A))
         {
            transform.rotation = Quaternion.Euler(0,0,90);
            nextLocation = transform.position + new Vector3(-1, 0);
         }
         else if(Input.GetKeyDown(KeyCode.W))
         {
            transform.rotation = Quaternion.Euler(0,0,0);
            nextLocation = transform.position + new Vector3(0, 1);
         }
         else if(Input.GetKeyDown(KeyCode.S))
         {
            transform.rotation = Quaternion.Euler(0,0,180);
            nextLocation = transform.position + new Vector3(0, -1);
         }
         else if(Input.GetKeyDown(KeyCode.D))
         {
            transform.rotation = Quaternion.Euler(0,0,270);
            nextLocation = transform.position + new Vector3(1, 0);
         }
      }
   }

   protected virtual bool BoxCanMove(Vector3 nextBoxLocation)
   {
      if (!TilemapManager.wallIsBlocked.ContainsKey(nextBoxLocation) &&
          !BoxManager.allBoxLocation.ContainsKey(nextBoxLocation))
         return true;

      return false;
   }


   protected virtual void ForUndo()
   {
      if (InputManger.Instance.undo)
      {
         Undo();
         BoxManager.Instance.CalculateUndoForEveryBoxes();
      }
   }

   public override void Undo()
   {
      base.Undo();
      nextLocation = transform.position;
   }

   public void TranslatePlayerforPush(Vector3 pos)
   {
      if (!BoxManager.allBoxLocation.ContainsKey(pos))
      {
         StartCoroutine(TEST(pos));
      }
   }

   public bool CheckToPushMoveUp(Vector3 pos, Vector3 location)
   {
      if ((BoxManager.allBoxLocation.ContainsKey(pos) || TilemapManager.wallIsBlocked.ContainsKey(pos)) && nextLocation == location)
      {
         return false;
      }
      return true;
   }
   
   IEnumerator TEST(Vector3 pos)
   {
         nextLocation = pos;
         MyStack.Pop();
         AddToStack(pos);
         while (transform.position != pos)
         {
            transform.position = Vector3.MoveTowards(transform.position, pos, speedSmoothly * Time.deltaTime);
            yield return null; // Chờ mỗi frame
         }   
   }

   public Vector3 GetNextLocation()
   {
      return nextLocation;
   }
}

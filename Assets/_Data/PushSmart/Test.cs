using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Test : MonoBehaviour
{
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected Vector3 newPos;
    [SerializeField] protected Vector3 oldPos;
    [SerializeField] protected float speedPush;
    private Transform obstacle;
     private Transform push;
     private Transform sitdown;
     private Player player;
    private void Awake()
    {
        obstacle = transform.Find("obstacle");
        push = transform.Find("push");
        sitdown = transform.Find("sitdown");
        player = GameObject.FindObjectOfType<Player>();
        LoadDirection();
    }

    private void Start()
    {
        oldPos = push.position - direction;
        newPos = push.position;
        TilemapManager.wallIsBlocked[push.position] = 1;
        LoadWallIsBlocked();
    }
    
    protected virtual void LoadWallIsBlocked()
    {
        TilemapManager.wallIsBlocked[newPos] = 1;
        TilemapManager.wallIsBlocked[oldPos] = 1;
    }
    private void Update()
    {
        HandlePush();
    }

    protected virtual void LoadDirection()
    {
        if (push.localRotation.eulerAngles.z == 0)
        {
            direction = new Vector3(-1, 0, 0);
        }
        else if (push.localRotation.eulerAngles.z == 90)
        {
            direction = new Vector3(0, -1, 0);
        }
        else if (push.localRotation.eulerAngles.z == 180)
        {
            direction = new Vector3(1, 0, 0);
        }
        else if (push.localRotation.eulerAngles.z == 270)
        {
            direction = new Vector3(0, 1, 0);
        }
    }
    protected virtual bool IsSitDown()
    {
        return player.GetNextLocation() == sitdown.position || BoxManager.allBoxLocation.ContainsKey(sitdown.position);
    }

    protected virtual void HandlePush()
    {
        if (IsSitDown() && CheckTowBox(newPos,newPos + direction) && player.CheckToPushMoveUp(player.GetNextLocation() + direction, newPos))
        {
            Debug.Log("Sit Down");
            MoveDown();
        }
        else if(!IsSitDown())
        {
            MoveUp();
        }
    }

    bool CheckTowBox(Vector3 begin , Vector3 last)
    {
        if (BoxManager.allBoxLocation.ContainsKey(begin) && BoxManager.allBoxLocation.ContainsKey(last))
        {
            return false;
        }

        return true;
    }
    protected virtual void MoveUp()
    {
        if (push.position == oldPos)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.push);
        }
        push.transform.position = Vector3.MoveTowards(push.transform.position, newPos, (speedPush) * Time.deltaTime);
        CheckToPush(newPos);
        TilemapManager.wallIsBlocked[newPos] = 1;
    }

    protected virtual void MoveDown()
    {
        if (push.position == newPos)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.push);
        }
        push.transform.position = Vector3.MoveTowards(push.transform.position, oldPos, (speedPush) * Time.deltaTime);
        TilemapManager.wallIsBlocked.Remove(newPos);
    }

    protected virtual void CheckToPush(Vector3 posCheck)
    {
        
        if (BoxManager.allBoxLocation.ContainsKey(posCheck))
        {
            Vector3 newPosBox = posCheck + direction;
            BoxManager.Instance.TranslateBox(posCheck, newPosBox);
        }

        if (player.GetNextLocation() == posCheck)
        {
            Vector3 target = posCheck + direction;
            player.TranslatePlayerforPush(target);
        }
    }

    
    
}
